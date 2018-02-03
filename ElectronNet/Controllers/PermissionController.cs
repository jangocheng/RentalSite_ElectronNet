using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ElectronNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static ElectronNet.CommonHelper;

namespace ElectronNet.Controllers
{
    /// <summary>
    /// 权限控制器
    /// </summary>
    public class PermissionController : Controller
    {
        /// <summary>
        /// 权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListPermissions()
        {
            string res = await GetAsync("", "/v1/Permission/ListPermissions");
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
            ResultModel model = await GetAsync<ResultModel>("?id=" + id, "/v1/Permission/GetPermissionById");
            if (model.Status != 200)
            {
                return Redirect("/Home/ErrorView");
            }
            Permission permission = JsonConvert.DeserializeObject<Permission>(model.Data.ToString());
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

            string json = await PostAsync(content, "/v1/Permission/AddPermission");
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;

            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>服务器返回内容</returns>
        [HttpPut]
        public async Task<IActionResult> EditPermission(Permission model)
        {
            string json = await PutAsync(model, "/v1/Permission/EditPermission");
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
            string json = await DeleteAsync("?id=" + id, "/v1/Permission/DeletePermission");
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }
    }
}