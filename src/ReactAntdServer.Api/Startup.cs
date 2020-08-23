using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReactAntdServer.Api.Utils;
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
        private const string ApiName="React Mongodb Api";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            //add appsetting   configuration
            services.Configure<BookstoreDatabaseSettings>(Configuration.GetSection(nameof(BookstoreDatabaseSettings)));
            services.AddSingleton<IBookstoreDatabaseSettings>(config =>
                config.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);

            //add singleton service
            services.AddSingleton<BookService>();
            services.AddSingleton<ProductService>();
            //add scoped service
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            //改为基类注入，子service继承
            //有问题
            //services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IManagerService, ManagerService>(); 
            services.AddControllers()
               .AddNewtonsoftJson(options => options.UseMemberCasing());

            //register jwt token
            services.Configure<TokenModel>(Configuration.GetSection("JwtToken"));
            var token = Configuration.GetSection("JwtToken").Get<TokenModel>();
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false 
                };
            });

            //register swagger
            services.AddSwaggerGen(option =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    option.SwaggerDoc(version, new OpenApiInfo
                    {   
                        Version = version,
                        Title = $"{ApiName} document",
                        Description  =$"{ApiName} Http Api {version}",
                        TermsOfService = new Uri("https://www.cnblogs.com/landwind/"),
                        Contact  = new Microsoft.OpenApi.Models.OpenApiContact { 
                            Name = ApiName,
                            Email = "landwind1180@gmail.com",
                            Url = new Uri("https://www.cnblogs.com/landwind/")
                        }
                    });
                    option.AddSecurityDefinition($"Bearer{version}", new OpenApiSecurityScheme
                    {
                        Description = "Authorization format : Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });//api界面新增authorize按钮
                });

               //option.OperationFilter<ad>
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 

            app.UseAuthentication();

            //config swagger
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
