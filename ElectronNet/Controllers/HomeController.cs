using ElectronNet.ApiSettings;
using ElectronNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ElectronNet.Controllers
{
    /// <summary>
    /// 程序主控制器
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly FirstVersion _urlSettings;

        public HomeController(IMemoryCache memoryCache, IOptions<FirstVersion> options)
        {
            _cache = memoryCache;
            _urlSettings = options.Value;
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
            //拼接参数并发送Http请求
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("PhoneNumber", phoneNum),
                new KeyValuePair<string, string>("Password", password)
            };
            HttpContent content = new FormUrlEncodedContent(values);
            string value = await CommonHelper.PostAsync(content, _urlSettings.Home.Login);
            //获取服务器返回数据
            ResultModel model = JsonConvert.DeserializeObject<ResultModel>(value);
            if (model.Status == (int)HttpStatusCode.OK && model.Data != null)
            {
                _cache.Set("adminUserId", model.Data);
                CommonHelper.HttpClient.DefaultRequestHeaders.Add("AccessToken", CommonHelper.TokenHeader(Convert.ToInt64(model.Data)));
            }

            Response.StatusCode = model.Status;
            return Content(value, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            ResultModel res = await CommonHelper.PostAsync<ResultModel>(new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()), "/v1/Home/Logout");
            if (res.Status == 200)
            {
                CommonHelper.HttpClient.DefaultRequestHeaders.Clear();
                Response.StatusCode = res.Status;
                _cache.Remove("adminUserId");
            }
            
            return Json(res);
        }

        /// <summary>
        /// 404页面
        /// </summary>
        /// <returns></returns>
        public IActionResult NotFoundView()
        {
            return View();
        }

        /// <summary>
        /// 500页面
        /// </summary>
        /// <returns></returns>
        public IActionResult ErrorView()
        {
            return View();
        }

        /// <summary>
        /// 提示页面
        /// </summary>
        /// <returns></returns>
        public IActionResult PromptView()
        {
            return View();
        }
    }
}