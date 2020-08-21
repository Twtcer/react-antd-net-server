using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ReactAntdServer.Api.Utils;
using ReactAntdServer.Model.Config;
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

            services.AddControllers()
               .AddNewtonsoftJson(options => options.UseMemberCasing());

            //register swagger
            services.AddSwaggerGen(option =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    option.SwaggerDoc(version, new Microsoft.OpenApi.Models.OpenApiInfo
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
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
