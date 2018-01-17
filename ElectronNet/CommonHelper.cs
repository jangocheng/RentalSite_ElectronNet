using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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

        /// <summary>
        /// 发送Http Get请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="queryString">QueryString对象</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> GetAsync<T>(QueryString queryString, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(ServerUrl + virtualRoute + queryString.ToString());
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送Http Get请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="queryString">参数字符串</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> GetAsync<T>(string queryString, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(ServerUrl + virtualRoute + queryString);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送Http Get请求
        /// </summary>
        /// <param name="queryString">QueryString对象</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> GetAsync(QueryString queryString, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(ServerUrl + virtualRoute + queryString.ToString());
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        /// <summary>
        /// 发送Http Get请求
        /// </summary>
        /// <param name="queryString">参数字符串</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> GetAsync(string queryString, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(ServerUrl + virtualRoute + queryString);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        /// <summary>
        /// 发送Http Get请求
        /// </summary>
        /// <typeparam name="TEntity">需要解析的参数模型类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> GetAsync<TEntity>(TEntity model, string virtualRoute)
        {
            //反射获取模型字段，创建QueryString
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();
            QueryString queryString = new QueryString();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                string propertyValue = property.GetValue(model).ToString();
                queryString.Add(propertyName, propertyValue);
            }

            //发送Http请求
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync($"{ServerUrl}{virtualRoute}{queryString.ToString()}");
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        /// <summary>
        /// 发送Http Get请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <typeparam name="TEntity">需要解析的参数模型类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> GetAsync<T, TEntity>(TEntity model, string virtualRoute)
        {
            //反射获取模型字段，创建QueryString
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();
            QueryString queryString = new QueryString();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                string propertyValue = property.GetValue(model).ToString();
                queryString.Add(propertyName, propertyValue);
            }

            //发送Http请求
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync($"{ServerUrl}{virtualRoute}{queryString.ToString()}");
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送Http Post请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="httpContent">HttpContent对象</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> PostAsync<T>(HttpContent httpContent, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(ServerUrl + virtualRoute, httpContent);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送Http Post请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="httpContent">HttpContent对象</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> PostAsync(HttpContent httpContent, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(ServerUrl + virtualRoute, httpContent);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        /// <summary>
        /// 发送Http Post请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <typeparam name="TEntity">需要解析的参数模型类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> PostAsync<T, TEntity>(TEntity model, string virtualRoute)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            //以反射性形式创建KeyValuePair对象
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                object value = property.GetValue(model);
                list.Add(new KeyValuePair<string, string>(key, value.ToString()));
            }

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(list);
            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(ServerUrl + virtualRoute, formContent);
            return JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// 发送Http Post请求
        /// </summary>
        /// <typeparam name="TEntity">需要解析的参数模型类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> PostAsync<TEntity>(TEntity model, string virtualRoute)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            //以反射性形式创建KeyValuePair对象
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                object value = property.GetValue(model);
                list.Add(new KeyValuePair<string, string>(key, value.ToString()));
            }

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(list);
            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(ServerUrl + virtualRoute, formContent);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
