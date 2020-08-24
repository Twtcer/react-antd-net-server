using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReactAntdServer.Model;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Model.Dto;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Service
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IManagerService _userService;
        private readonly JwtTokenConfig _tokenConfig;

        public TokenAuthenticationService(IManagerService userService, IOptions<JwtTokenConfig> config)
        {
            _userService = userService;
            _tokenConfig = config.Value;
        }

        public bool IsAuthenticated(LoginRequest request, out string token)
        {
            token = string.Empty;
            Manager manager = null;
            if (!_userService.IsValild(request,out manager)) return false;
            var claims = new List<Claim> { 
                new Claim(JwtRegisteredClaimNames.Jti, request.Username),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") , 
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(_tokenConfig.AccessExpiration)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss,_tokenConfig.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud,_tokenConfig.Audience),
            };
            // 可以将一个用户的多个角色全部赋予； 
            //为了解决一个用户多个角色(比如：Admin,System)，用下边的方法
            claims.AddRange(manager.Roles.Split(',').Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(issuer: _tokenConfig.Issuer,claims: claims,signingCredentials: credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
