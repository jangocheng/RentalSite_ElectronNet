using ElectronNet.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElectronNet
{
    public static class CommonHelper
    {
        static CommonHelper()
        {
            HttpClient = new HttpClient(new HttpClientHandler
            {
                Proxy = null,
                DefaultProxyCredentials = null,
                UseProxy = false
            }, false);
            HttpClient.DefaultRequestHeaders.Add("AccessToken", TokenHeader());
        }

        /// <summary>
        /// HttpClient对象
        /// </summary>
        public static HttpClient HttpClient { get; private set; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string ServerUrl { get; } = "http://localhost:5000";

        /// <summary>
        /// JWT密钥
        /// </summary>
        public static string Secret => "{107C5AEF-6B1F-4BE5-A47F-B20994CF1288@$$%__}";

        /// <summary>
        /// JsonSerializerSettings对象
        /// </summary>
        public static JsonSerializerSettings JsonSerializerSettings { get; } = JsonSerializerSettingsProvider.CreateSerializerSettings();

        /// <summary>
        /// 生成AccessToken
        /// </summary>
        /// <returns>AccessToken字符串</returns>
        public static string TokenHeader()
        {
            //MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            //bool res = cache.TryGetValue("adminUserId", out long adminUserId);
            //if (!res)
            //{
            //    return null;
            //}

            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {
                    "adminUserId",
                    "1"//adminUserId.ToString()
                },
                {
                    "UUID",
                    Guid.NewGuid().ToString()
                }
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            string token = encoder.Encode(dictionary, Secret);

            return token;
        }

        #region HttpClient封装

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
            QueryString queryString = QueryString.Create(GetKeyValuePairToEntity(model));

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
            QueryString queryString = QueryString.Create(GetKeyValuePairToEntity(model));

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
            List<KeyValuePair<string, string>> list = GetKeyValuePairToEntity(model);

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
            List<KeyValuePair<string, string>> list = GetKeyValuePairToEntity(model);

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(list);
            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(ServerUrl + virtualRoute, formContent);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 发送Http Put请求
        /// </summary>
        /// <param name="httpContent">HttpContent对象</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> PutAsync(HttpContent httpContent, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.PutAsync(ServerUrl + virtualRoute, httpContent);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        /// <summary>
        /// 发送Http Put请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="httpContent">HttpContent对象</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> PutAsync<T>(HttpContent httpContent, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.PutAsync(ServerUrl + virtualRoute, httpContent);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送Http Put请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <typeparam name="TEntity">需要解析的参数模型类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> PutAsync<T, TEntity>(TEntity model, string virtualRoute)
        {
            List<KeyValuePair<string, string>> list = GetKeyValuePairToEntity(model);

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(list);
            HttpResponseMessage httpResponseMessage = await HttpClient.PutAsync(ServerUrl + virtualRoute, formContent);
            return JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// 发送Http Put请求
        /// </summary>
        /// <typeparam name="TEntity">需要解析的参数模型类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> PutAsync<TEntity>(TEntity model, string virtualRoute)
        {
            List<KeyValuePair<string, string>> list = GetKeyValuePairToEntity(model);

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(list);
            HttpResponseMessage httpResponseMessage = await HttpClient.PutAsync(ServerUrl + virtualRoute, formContent);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 发送Http Delete请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="queryString">QueryString对象</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> DeleteAsync<T>(QueryString queryString, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.DeleteAsync(ServerUrl + virtualRoute + queryString.ToString());
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送Http Delete请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="queryString">参数字符串</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> DeleteAsync<T>(string queryString, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.DeleteAsync(ServerUrl + virtualRoute + queryString);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送Http Delete请求
        /// </summary>
        /// <param name="queryString">QueryString对象</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> DeleteAsync(QueryString queryString, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.DeleteAsync(ServerUrl + virtualRoute + queryString.ToString());
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        /// <summary>
        /// 发送Http Delete请求
        /// </summary>
        /// <param name="queryString">参数字符串</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> DeleteAsync(string queryString, string virtualRoute)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.DeleteAsync(ServerUrl + virtualRoute + queryString);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        /// <summary>
        /// 发送Http Delete请求
        /// </summary>
        /// <typeparam name="TEntity">需要解析的参数模型类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<string> DeleteAsync<TEntity>(TEntity model, string virtualRoute)
        {
            QueryString queryString = QueryString.Create(GetKeyValuePairToEntity(model));

            //发送Http请求
            HttpResponseMessage httpResponseMessage = await HttpClient.DeleteAsync($"{ServerUrl}{virtualRoute}{queryString.ToString()}");
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        /// <summary>
        /// 发送Http Delete请求
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <typeparam name="TEntity">需要解析的参数模型类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="virtualRoute">虚拟路径</param>
        /// <returns>请求返回内容</returns>
        public async static Task<T> DeleteAsync<T, TEntity>(TEntity model, string virtualRoute)
        {
            QueryString queryString = QueryString.Create(GetKeyValuePairToEntity(model));

            //发送Http请求
            HttpResponseMessage httpResponseMessage = await HttpClient.DeleteAsync($"{ServerUrl}{virtualRoute}{queryString.ToString()}");
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        /// <summary>
        /// 使用反射将模型转化为KeyValuePair集合
        /// </summary>
        /// <typeparam name="TEntity">模型类型</typeparam>
        /// <param name="model">数据模型</param>
        /// <returns>KeyValuePair集合</returns>
        public static List<KeyValuePair<string, string>> GetKeyValuePairToEntity<TEntity>(TEntity model)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            //以反射性形式创建KeyValuePair对象
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                //判断属性类型是否为数组
                bool isArr = property.PropertyType.IsArray;
                if (isArr)
                {
                    Array array = (Array)property.GetValue(model);
                    foreach (var item in array)
                    {
                        list.Add(new KeyValuePair<string, string>(key, item.ToString()));
                    }
                }
                else
                {
                    object value = property.GetValue(model);
                    if (value == null)
                        continue;
                    list.Add(new KeyValuePair<string, string>(key, value.ToString()));
                }
            }

            return list;
        }

        /// <summary>
        /// 判断是否为Ajax请求
        /// </summary>
        /// <param name="httpContext">HttpContext上下文对象</param>
        /// <returns>是否为Ajax请求</returns>
        public static bool IsAjax(this HttpContext httpContext)
        {
            return !string.IsNullOrEmpty(httpContext.Request.Headers["X-Requested-With"].FirstOrDefault());
        }

        /// <summary>
        /// ResultModel转换LayuiTableModel
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="resultModel">ResultModel对象</param>
        /// <returns>LayuiTableModel对象</returns>
        public static LayuiTableModel<T> ToTableModel<T>(ResultModel resultModel)
        {
            var layuiTable = JsonConvert.DeserializeObject<LayuiTableModel<T>>(resultModel.Data.ToString());
            layuiTable.StatusCode = resultModel.Status;
            layuiTable.Message = resultModel.Message;
            return layuiTable;
        }

        /// <summary>
        /// 计算一个文件的MD5值
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns>MD5值</returns>
        public static string CalcMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    sb.Append(computeBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Stream转byte[]
        /// </summary>
        /// <param name="stream">需要转换的文件流</param>
        /// <returns>转换后的byte[]</returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 获取字符串的MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(this string input)
        {
            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串  
            return sBuilder.ToString();
        }
    }
}
