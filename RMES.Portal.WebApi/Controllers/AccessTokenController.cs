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
            var cacheTempKey = $"OldRefreshToken:{request.Token}";
            var temp = GetAccessTokenFromOldCache();

            if (temp != null)
            {
                return Ok(temp);
            }

            var token = request.Token;
            var cacheStr = _cache.GetString($"RefreshToken:{token}");
            if (string.IsNullOrWhiteSpace(cacheStr))
            {
                return Ok(new
                {
                    Code = 0,
                    Message = "Token不存在或已过期"
                });
            }

            var cacheUser = JsonConvert.DeserializeObject<SessionUser>(cacheStr);
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id);

            if (userId == null || cacheUser.Id.ToString() != userId.Value)
            {
                return Ok(new
                {
                    Code = 0,
                    Message = "用户不匹配"
                });
            }

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

            lock (Lock)
            {
                temp = GetAccessTokenFromOldCache();
                if (temp != null)
                {
                    return Ok(temp);
                }

                var refreshToken = Guid.NewGuid().ToString("N");
                var cacheKey = $"RefreshToken:{refreshToken}";
                var refreshTokenExpiredTime = DateTime.Today.AddDays(7);


                var accessTokenExpiredTime = DateTime.Now.AddHours(2);
                var accessToken = GetAccessToken(cacheUser, accessTokenExpiredTime);

                cacheUser.ExpireTime = accessTokenExpiredTime;
                cacheUser.AccessToken = accessToken;

                _cache.SetString(cacheKey, JsonConvert.SerializeObject(cacheUser), 
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = refreshTokenExpiredTime
                    });

                var result = new AccessTokenModel
                {
                    AccessToken = accessToken,
                    Code = 200,
                    Expired = DateTimeHelper.ConvertToLong(accessTokenExpiredTime),
                    RefreshToken = refreshToken
                };

                _cache.SetString(cacheTempKey, JsonConvert.SerializeObject(result), 
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(10)
                    });

                _cache.Remove($"RefreshToken:{request.Token}");

                return Ok(result);
            }

            // 内部函数，从
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