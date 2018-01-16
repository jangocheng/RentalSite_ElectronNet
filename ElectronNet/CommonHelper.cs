using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElectronNet
{
    public class CommonHelper
    {
        static CommonHelper()
        {
            HttpClient = new HttpClient(new HttpClientHandler
            {
                Proxy = null,
                DefaultProxyCredentials = null,
                UseProxy = false
            }, false);

            IConfigurationBuilder configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfigurationRoot configRoot = configuration.Build();
            ServerUrl = configRoot.GetSection("ServerUrl").Value;
        }

        /// <summary>
        /// HttpClient对象
        /// </summary>
        public static HttpClient HttpClient { get; private set; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string ServerUrl { get; private set; }

        /// <summary>
        /// JWT密钥
        /// </summary>
        public static string Secret => "{107C5AEF-6B1F-4BE5-A47F-B20994CF1288@$$%__}";
    }
}
