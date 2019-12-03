using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace RMES.Portal.WebApi.Extensions.Authorizations
{
    public class JwtTokenManager<TUser>
    {
        private readonly string _issuer;

        private readonly string _audience;

        private readonly string _securityKey;

        private readonly Func<TUser, List<Claim>> _userToClaimList;

        private readonly int _accessTokenLifeTime;

        private readonly int _refreshTokenLifeTime;

        public JwtTokenManager(string issuer, string audience, string securityKey, Func<TUser, List<Claim>> func, int accessTokenLife = 120, int refreshTokenLife = 10080)
        {
            _issuer = issuer;
            _audience = audience;
            _securityKey = securityKey;
            _accessTokenLifeTime = accessTokenLife;
            _refreshTokenLifeTime = refreshTokenLife;
            _userToClaimList = func;
        }

        public JwtTokenManager(JwtTokenManageOptions options, Func<TUser, List<Claim>> func)
        {
            _issuer = options.Issuer;
            _audience = options.Audience;
            _securityKey = options.SecurityKey;
            _accessTokenLifeTime = options.AccessTokenLifeTime;
            _refreshTokenLifeTime = options.RefreshTokenLifeTime;
            _userToClaimList = func;
        }

        public JwtTokenManager(IConfiguration configuration, Func<TUser, List<Claim>> func, int accessTokenLife = 120, int refreshTokenLife = 10080)
        {
            _issuer = configuration["Authentication:JwtBearer:Issuer"];
            _audience = configuration["Authentication:JwtBearer:Audience"];
            _securityKey = configuration["Authentication:JwtBearer:SecurityKey"];
            _accessTokenLifeTime = accessTokenLife;
            _refreshTokenLifeTime = refreshTokenLife;
            _userToClaimList = func;
        }

        private string BuildAccessToken(TUser user)
        {
            var claims = _userToClaimList(user);
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_securityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_issuer, _audience,
                claims,
                expires: DateTime.Now.AddMinutes(_accessTokenLifeTime),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class JwtTokenManageOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecurityKey { get; set; }

        /// <summary>
        /// AccessToken的生命周期，单位 分钟，默认2小时
        /// </summary>
        public int AccessTokenLifeTime { get; set; } = 120;

        /// <summary>
        /// RefreshToken的生命周期，单位 分钟，默认7天
        /// </summary>
        public int RefreshTokenLifeTime { get; set; } = 10080;
    }

    public class RefreshTokenCacheEntry
    {
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenLifeTime { get; set; }

        public List<Claim> Claims { get; set; }
    }

    public class AccessTokenCacheEntry
    {
        public string AccessToken { get; set; }

        public DateTime ExpireTime { get; set; }

        public string Sign { get; set; }
    }

    public class SessionUserCacheEntry
    {
        public string Sign { get; set; }
    }
}
