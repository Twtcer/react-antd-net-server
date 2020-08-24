using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using ReactAntdServer.Model.Config;

namespace ReactAntdServer.Api.Jwt
{
    public class PermissionRequirement: IAuthorizationRequirement
    {
        /// <summary>
        /// 用户权限集合
        /// </summary>
        public List<PermissionItem> Permissions { get; set; }

        /// <summary>
        /// 无权限
        /// </summary>
        public string DeniedAction { get; set; }
        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimaType { get; set; }
        /// <summary>
        /// 请求路径
        /// </summary>
        public string LoginPath { get; set; } = "/Api/Login";
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; }
        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        public PermissionRequirement(string deniedAction, List<PermissionItem> permissions, string claimType, string issuer, string audience, SigningCredentials signingCredentials, TimeSpan expiration) 
        {
            this.DeniedAction = deniedAction;
            this.Permissions = permissions;
            this.ClaimaType = claimType;
            this.Issuer = issuer;
            this.Audience = audience;
            this.SigningCredentials = signingCredentials;
            this.Expiration = expiration;
        }

    }
}
