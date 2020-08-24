using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ReactAntdServer.Api.Utils
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class ConfigurationHelper
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static readonly ConfigurationHelper Instance = new ConfigurationHelper();
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        IConfiguration Configuration { get; set; }

          ConfigurationHelper()
        {
            Configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
        }

        public string Section(params string[] sections)
        {
            try
            {
                var key = string.Join(':', sections);
                return Configuration[key];
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
