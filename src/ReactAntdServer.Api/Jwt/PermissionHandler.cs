using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using MongoDB.Libmongocrypt;
using ReactAntdServer.Api.Utils;
using ReactAntdServer.Model.Config;

namespace ReactAntdServer.Api.Jwt
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="provider"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes)
        {
            Schemes = schemes;
        }

        /// <summary>
        /// 重写异步权限处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            requirement.Permissions = new List<PermissionItem>()
            {
                  new PermissionItem (){Role="Admin",Url="/api/v1/admin/Products" },
                new PermissionItem (){Role="Admin",Url="/api/values/post" },
                new PermissionItem (){Role="Admin",Url="/api/values/put" },
                new PermissionItem (){Role="Admin",Url="/api/values/delete" },
            };

            //从上下文中取出信息
            var httpContext = (context.Resource as AuthorizationFilterContext).HttpContext;
            var requestUrl = httpContext.Request.Path.Value.ToLower();
            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                //不为空及登录成功
                if (result?.Principal != null)
                {
                    httpContext.User = result.Principal;
                    var isMathUrl = false;
                    requirement.Permissions.GroupBy(a => a.Url).ForEach(g =>
                     {
                         try
                         {
                             if (Regex.Match(requestUrl, g.Key?.ObjToString().ToLower()).Value == requestUrl)
                             {
                                 isMathUrl = true;
                             }
                         }
                         catch (Exception e)
                         {

                             throw;
                         }
                     });
                    if (isMathUrl)
                    {
                        var currentUserRoles = httpContext.User.Claims.Where(a => a.Type == requirement.ClaimaType).Select(a => a.Value).ToList();
                        var isMatchRole = false;
                        requirement.Permissions.Where(a => currentUserRoles.Contains(a.Role)).ForEach(a =>
                        {
                            try
                            {
                                if (Regex.Match(requestUrl, a.Url?.ObjToString().ToLower())?.Value == requestUrl)
                                {
                                    isMatchRole = true;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        });

                        //验证权限
                        if (currentUserRoles.Count <= 0 || !isMatchRole)
                        {
                            context.Fail();
                            return;
                        }
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                    //判断过期时间
                    if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                    return;
                }
            }
            //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
            if (!requestUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST")
               || !httpContext.Request.HasFormContentType))
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }
}
