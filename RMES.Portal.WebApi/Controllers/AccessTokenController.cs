using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RMES.Portal.WebApi.Models;
using RMES.Util;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using RMES.Services.Bbs;

namespace RMES.Portal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly UserService _service;

        private static readonly object Lock = new object();

        public AccessTokenController(IConfiguration configuration, IDistributedCache cache, UserService service)
        {
            _configuration = configuration;
            _cache = cache;
            _service = service;
        }

        /// <summary>
        /// 登录，获取后原来RefreshToken将失效。
        /// AccessToken有效时间30分钟
        /// RefreshToken有效时间60分钟
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromBody]LoginModel model)
        {
            var result = _service.Login(model.Account, model.Pw);
            if (result.Code != 200)
            {
                return Ok(new {Code = 0, Message = result.Message});
            }

            var refreshToken = Guid.NewGuid().ToString("N");
            var refreshTokenExpiredTime = DateTime.Today.AddDays(7);

            var accessTokenExpiredTime = DateTime.Now.AddHours(2);

            var user = new SessionUser
            {
                Id = result.Body.Id,
                Name = result.Body.NickName,
                Role = "user",
                ExpireTime = accessTokenExpiredTime
            };

            var accessToken = GetAccessToken(user, accessTokenExpiredTime);
            user.AccessToken = accessToken;

            var cacheKey = $"RefreshToken:{refreshToken}";
            var cacheValue = JsonConvert.SerializeObject(user);

            _cache.SetString(cacheKey, cacheValue,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = refreshTokenExpiredTime
                });

            return Ok(new AccessTokenModel
            {
                AccessToken = GetAccessToken(user, accessTokenExpiredTime),
                Code = 200,
                Expired = DateTimeHelper.ConvertToLong(accessTokenExpiredTime),
                RefreshToken = refreshToken
            });
        }

        /// <summary>
        /// 刷新AccessToken
        /// </summary>
        /// <param name="request">刷新的请求 {"token": "refresh_token"}</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Refresh")]
        public IActionResult Refresh(RefreshTokenRequest request)
        {
            // 如果暂存缓存中存在当前请求的Token，直接返回；
            // 这里有个问题，没有进行用户ID的对比
            var cacheTempKey = $"OldRefreshToken:{request.Token}";

            var temp = GetAccessTokenFromOldCache();
            if (temp != null)
            {
                return Ok(temp);
            }

            // 从RefreshToken缓存中尝试获取当前请求传递的Token
            var token = request.Token;
            var cacheStr = _cache.GetString($"RefreshToken:{token}");

            // 如果缓存不存在，直接返回
            if (string.IsNullOrWhiteSpace(cacheStr))
            {
                return Ok(new
                {
                    Code = 0,
                    Message = "Token不存在或已过期"
                });
            }

            // 反序列化缓存中保存的用户信息
            var cacheUser = JsonConvert.DeserializeObject<SessionUser>(cacheStr);
            
            // 获取当前请求中的用户ID，用于和缓存中的用户ID比对
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id);

            // 如果当前请求中的用户ID与缓存中用户信息的ID不符，直接返回
            if (userId == null || cacheUser.Id.ToString() != userId.Value)
            {
                return Ok(new
                {
                    Code = 0,
                    Message = "用户不匹配"
                });
            }

            // 如果AccessToken 5分钟后不会过期，直接返回现存的AccessToken
            if (cacheUser.ExpireTime.AddMinutes(-5) > DateTime.Now)
            {
                return Ok(new AccessTokenModel
                {
                    AccessToken = cacheUser.AccessToken,
                    Code = 200,
                    Expired = DateTimeHelper.ConvertToLong(cacheUser.ExpireTime),
                    RefreshToken = request.Token
                });
            }

            // 加锁，以免重复刷新
            lock (Lock)
            {
                // 再次检查暂存缓存，如果存在（说明刚刚刷新成功），直接返回
                temp = GetAccessTokenFromOldCache();
                if (temp != null)
                {
                    return Ok(temp);
                }

                // 下面开始构造新的AccessToken
                var refreshToken = Guid.NewGuid().ToString("N");
                var cacheKey = $"RefreshToken:{refreshToken}";
                var refreshTokenExpiredTime = DateTime.Today.AddDays(7);

                var accessTokenExpiredTime = DateTime.Now.AddHours(2);
                var accessToken = GetAccessToken(cacheUser, accessTokenExpiredTime);

                // 更新当前用户信息
                cacheUser.ExpireTime = accessTokenExpiredTime;
                cacheUser.AccessToken = accessToken;

                // 添加新的RefreshToken缓存
                _cache.SetString(cacheKey, JsonConvert.SerializeObject(cacheUser), 
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = refreshTokenExpiredTime
                    });

                // 构造返回结果
                var result = new AccessTokenModel
                {
                    AccessToken = accessToken,
                    Code = 200,
                    Expired = DateTimeHelper.ConvertToLong(accessTokenExpiredTime),
                    RefreshToken = refreshToken
                };

                // 把新的AccessToken结果保存到暂存缓存，10秒后过期
                _cache.SetString(cacheTempKey, JsonConvert.SerializeObject(result), 
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(10)
                    });

                // 删除旧的RefreshToken
                _cache.Remove($"RefreshToken:{request.Token}");

                return Ok(result);
            }

            // 内部函数，检查暂存缓存是否存在当前请求的RefreshToken
            AccessTokenModel GetAccessTokenFromOldCache()
            {
                var tempValue = _cache.GetString(cacheTempKey);

                return !string.IsNullOrWhiteSpace(tempValue) ?
                    JsonConvert.DeserializeObject<AccessTokenModel>(tempValue) :
                    null;
            }
        }

        /// <summary>
        /// 通过SessionUser获取AccessToken
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accessTokenExpiredTime">AccessToken过期时间</param>
        /// <returns></returns>
        private string GetAccessToken(SessionUser user, DateTime accessTokenExpiredTime)
        {
            var claims = new[]
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.Name, user.Name),
                new Claim(JwtClaimTypes.Role, "user")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:SecurityKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Authentication:JwtBearer:Issuer"],
                _configuration["Authentication:JwtBearer:Audience"],
                claims,
                expires: accessTokenExpiredTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 刷新AccessToken的请求
        /// </summary>
        public class RefreshTokenRequest
        {
            /// <summary>
            /// RefreshToken，登录后获取
            /// </summary>
            public string Token { get; set; }
        }

        public class AccessTokenModel
        {
            public int Code { get; set; }

            public string AccessToken { get; set; }

            public string RefreshToken { get; set; }

            public long Expired { get; set; }
        }
    }
}