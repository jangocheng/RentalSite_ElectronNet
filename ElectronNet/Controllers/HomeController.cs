using ElectronNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;
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
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("phoneNumber", phoneNum),
                new KeyValuePair<string, string>("password", password)
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            string value = await CommonHelper.PostAsync(content, "/v1/Home/Login");
            ResultModel model = JsonConvert.DeserializeObject<ResultModel>(value);
            _cache.Set("adminUserId", model.Data);
            Response.StatusCode = model.Status;
            return Content(value, "application/json", Encoding.UTF8);
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