using ElectronNet.ApiSettings;
using ElectronNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ElectronNet.CommonHelper;

namespace ElectronNet.Controllers
{
    /// <summary>
    /// 权限控制器
    /// </summary>
    public class PermissionController : Controller
    {
        private readonly FirstVersion _urlSettings;

        public PermissionController(IOptions<FirstVersion> options)
        {
            _urlSettings = options.Value;
        }

        /// <summary>
        /// 权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListPermissions()
        {
            string res = await GetAsync("", _urlSettings.Permission.ListPermissions);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(res).Status;
            return View((object)res);
        }

        /// <summary>
        /// 添加权限页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddPermission()
        {
            return View();
        }

        /// <summary>
        /// 编辑权限项页面
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditPermission(long id)
        {
            ResultModel model = await GetAsync<ResultModel>("?id=" + id, _urlSettings.Permission.GetPermissionById);
            if (model.Status != 200)
            {
                return Redirect("/Home/ErrorView");
            }
            Models.Permission permission = JsonConvert.DeserializeObject<Models.Permission>(model.Data.ToString());
            return View(permission);
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <param name="description">权限描述</param>
        /// <returns>服务器返回内容</returns>
        [HttpPost]
        public async Task<IActionResult> AddPermission(string name, string description)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(name),name),
                new KeyValuePair<string, string>(nameof(description),description)
            };

            HttpContent content = new FormUrlEncodedContent(list);

            string json = await PostAsync(content, _urlSettings.Permission.AddPermission);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;

            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>服务器返回内容</returns>
        [HttpPut]
        public async Task<IActionResult> EditPermission(Models.Permission model)
        {
            string json = await PutAsync(model, _urlSettings.Permission.EditPermission);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 删除权限项
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns>服务器返回内容</returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePermission(long id)
        {
            string json = await DeleteAsync("?id=" + id, _urlSettings.Permission.DeletePermission);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">Id数组</param>
        /// <returns>服务器返回内容</returns>
        [HttpDelete]
        public async Task<IActionResult> BatchDeletePermissions(string idStr)
        {
            int[] idArr = JsonConvert.DeserializeObject<int[]>(idStr);
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (var id in idArr)
            {
                list.Add(new KeyValuePair<string, string>("idArr", id.ToString()));
            }

            QueryString query = QueryString.Create(list);
            string json = await DeleteAsync(query, _urlSettings.Permission.BatchDeletePermission);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }
    }
}