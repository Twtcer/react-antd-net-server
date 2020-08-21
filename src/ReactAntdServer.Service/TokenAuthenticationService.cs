using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Model.Dto;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Service
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserService _userService;
        private readonly TokenModel _token;

        public TokenAuthenticationService( IUserService userService, IOptions<TokenModel> token)
        {
            _userService = userService;
            _token = token.Value;
        }

        public bool IsAuthenticated(LoginRequestDTO request, out string token)
        {
            token = string.Empty;
            if (!_userService.IsValild(request)) return false;
            var claims = new[] {
                new Claim(ClaimTypes.Name,request.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_token.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_token.Issuer, _token.Audience, claims, expires: DateTime.Now.AddMinutes(_token.AccessExpiration));

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
