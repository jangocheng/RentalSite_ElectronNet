using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ElectronNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace ElectronNet.Controllers
{
    /// <summary>
    /// 程序主控制器
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;

        public HomeController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        /// <summary>
        /// 程序主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="phoneNum">电话号码</param>
        /// <param name="password">密码</param>
        /// <returns>服务端返回的提示信息</returns>
        [HttpPost]
        public async Task<IActionResult> Login(string phoneNum, string password)
        {
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("phoneNumber", phoneNum));
            values.Add(new KeyValuePair<string, string>("password", password));

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await CommonHelper.HttpClient.PostAsync(CommonHelper.ServerUrl + "/v1/Home/Login", content);
            string value = await response.Content.ReadAsStringAsync();
            ResultModel model = JsonConvert.DeserializeObject<ResultModel>(value);
            _cache.Set("adminUserId", model.Data);

            return Content(value, "application/json", Encoding.UTF8);
        }
    }
}