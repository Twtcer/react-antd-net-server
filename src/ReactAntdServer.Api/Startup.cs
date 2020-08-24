using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReactAntdServer.Api.Enums;
using ReactAntdServer.Api.Jwt;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Service;
using ReactAntdServer.Service.Base;
using ReactAntdServer.Service.Impl;
using ReactAntdServer.Services;

namespace ReactAntdServer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string ApiName="ReactAntdServer.Api";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //mvc
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.AddControllers()
               .AddNewtonsoftJson(options => options.UseMemberCasing());

            #region Register Mongodb Config
            services.Configure<BookstoreDatabaseSettings>(Configuration.GetSection(nameof(BookstoreDatabaseSettings)));
            services.AddSingleton<IBookstoreDatabaseSettings>(config =>
                config.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);
            #endregion

            #region Register Singleton 
            //add singleton service
            services.AddSingleton<BookService>();
            services.AddSingleton<ProductService>();
            //add scoped service
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            //改为基类注入，子service继承
            //有问题
            //services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IManagerService, ManagerService>();
            #endregion

            #region Register Swagger 
            services.AddSwaggerGen(option =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    option.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{ApiName} document",
                        Description = $"{ApiName} Http Api {version}",
                        TermsOfService = new Uri("https://www.cnblogs.com/landwind/"),
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = ApiName,
                            Email = "landwind1180@gmail.com",
                            Url = new Uri("https://www.cnblogs.com/landwind/")
                        }
                    });
                    option.OrderActionsBy(a => a.RelativePath);
                }); 

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
                //token绑定
                option.AddSecurityDefinition($"Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization format : Bearer {token} JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
            #endregion 

            #region Authentication  授权

            services.Configure<JwtTokenConfig>(Configuration.GetSection("JwtToken"));
            var jwtToken = Configuration.GetSection("JwtToken").Get<JwtTokenConfig>();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtToken.Secret));

            var permissions = new List<PermissionItem>(); 
            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permissions,
                ClaimTypes.Role,//基于角色的授权
                jwtToken.Issuer,//发行人
                jwtToken.Audience,//订阅者
                new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),//签名凭据
                expiration: TimeSpan.FromMinutes(jwtToken.AccessExpiration)//接口的过期时间
                ); 
            services.AddAuthorization(option =>
            {
                option.AddPolicy("Permission", policy =>
                     policy.Requirements.Add(permissionRequirement)
                 ) ;
            });
            #endregion

            #region Authentication 认证
            services.AddAuthentication(option =>
          {
              option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          .AddJwtBearer(x =>
          {
                //x.RequireHttpsMetadata = false;
                //x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = signingKey,
                  ValidIssuer = jwtToken.Issuer,
                  ValidateIssuer = true,
                  ValidAudience = jwtToken.Audience,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ClockSkew = TimeSpan.Zero,
                  RequireExpirationTime = true
              };
              x.Events = new JwtBearerEvents
              {
                  OnAuthenticationFailed = context =>
                  {
                      // 若过期，则把<是否过期>添加到，返回头信息中
                      if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                      {
                          context.Response.Headers.Add("Token Expired", "true");
                      }
                      return Task.CompletedTask;
                  }
              };
          });

            //注入权限处理
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //根据版本名称倒序 遍历展示
                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                    c.RoutePrefix = string.Empty;//根路径展示swagger
                });
            });   
            #endregion

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
