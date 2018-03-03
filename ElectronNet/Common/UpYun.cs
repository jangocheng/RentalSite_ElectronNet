using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElectronNet.Common
{
    public class UpYun
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Bucket { get; set; } = "aspnetweb";

        /// <summary>
        /// 操作员密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 操作员账号
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 请求Url地址，默认为v0。
        /// </summary>
        public string RestUrl { get; set; } = "https://v0.api.upyun.com";

        /// <summary>
        /// 请求日期时间，默认为当前格林威治时间。
        /// </summary>
        public string Date { get; set; } = DateTime.UtcNow.ToString("r");

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="data">文件字节数组</param>
        /// <param name="path">保存地址</param>
        /// <returns>是否上传成功</returns>
        public async Task<bool> WriteFileAsync(byte[] data, string path)
        {
            string url = RestUrl + "/" + Bucket + path;

            using (HttpClientHandler handler = new HttpClientHandler { UseProxy = false, UseCookies = false })
            using (HttpClient client = new HttpClient(handler))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                var content = new ByteArrayContent(data);
                request.Content = content;
                request.Headers.Add("Date", Date);
                //创建Authentication Header并发送请求
                request.Headers.Authorization = Getsignature("PUT", path);
                var response = await client.SendAsync(request);
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isAsync">是否异步删除</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteFileAsync(string path, bool isAsync = true)
        {
            using (HttpClientHandler handler = new HttpClientHandler { UseProxy = false, UseCookies = false })
            using (HttpClient client = new HttpClient(handler))
            {
                if (isAsync)
                {
                    client.DefaultRequestHeaders.Add("x-upyun-async", "true");
                }

                if (path.StartsWith("http://") || path.StartsWith("https://"))
                {
                    path = path.Replace("http://", "").Replace("https://", "");
                    int index = path.IndexOf('/', 0);
                    string apiUrl = path.Substring(0, index);
                    path = path.Replace(apiUrl, "");
                }
                client.DefaultRequestHeaders.Authorization = Getsignature("DELETE", path);
                string url = RestUrl + "/" + Bucket + path;
                var response = await client.DeleteAsync(url);
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        private AuthenticationHeaderValue Getsignature(string method, string path)
        {
            using (HMACSHA1 sha1 = new HMACSHA1(Encoding.ASCII.GetBytes(Password.GetMd5Hash())))
            {
                //计算signature
                string joinStr = $"{method}&/{Bucket}{path}&{Date}";
                byte[] buffer = Encoding.ASCII.GetBytes(joinStr);
                byte[] shaByteArr = sha1.ComputeHash(buffer);
                string signature = Convert.ToBase64String(shaByteArr);
                return new AuthenticationHeaderValue("UPYUN", $"{Operator}:{signature}");
            }
        }
    }
}
