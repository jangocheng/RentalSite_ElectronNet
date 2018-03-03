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
    /// 角色控制器
    /// </summary>
    public class RoleController : Controller
    {
        private readonly FirstVersion _urlSettings;

        public RoleController(IOptions<FirstVersion> options)
        {
            _urlSettings = options.Value;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            string json = await GetAsync("", _urlSettings.Role.ListRoles);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return View((object)json);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            ResultModel res = await GetAsync<ResultModel>("", _urlSettings.Permission.ListPermissions);
            Models.Permission[] perArr = JsonConvert.DeserializeObject<Models.Permission[]>(res.Data.ToString());
            return View(perArr);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="name">角色名称</param>
        /// <param name="perIds">对应权限项</param>
        /// <returns>服务器返回内容</returns>
        [HttpPost]
        public async Task<IActionResult> AddRole(string name, long[] perIds)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("name",name)
            };
            foreach (var item in perIds)
            {
                list.Add(new KeyValuePair<string, string>("permissionIds", item.ToString()));
            }

            HttpContent content = new FormUrlEncodedContent(list);
            string json = await PostAsync(content, _urlSettings.Role.AddRole);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditRole(long id)
        {
            ResultModel res = await GetAsync<ResultModel>("?id=" + id, _urlSettings.Role.GetRoleById);
            Models.Role role = JsonConvert.DeserializeObject<Models.Role>(res.Data.ToString());
            return View(role);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="name">角色名称</param>
        /// <param name="perIds">对应权限项</param>
        /// <returns>服务器返回内容</returns>
        [HttpPut]
        public async Task<IActionResult> EditRole(long id, string name, long[] perIds)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(id),id.ToString()),
                new KeyValuePair<string, string>(nameof(name),name)
            };
            foreach (var item in perIds)
            {
                list.Add(new KeyValuePair<string, string>("permissionIds", item.ToString()));
            }

            HttpContent content = new FormUrlEncodedContent(list);
            string json = await PutAsync(content, _urlSettings.Role.EditRole);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns>服务器返回内容</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(long id)
        {
            string json = await DeleteAsync($"?id={id}", _urlSettings.Role.DeleteRole);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids">Id数组</param>
        /// <returns>服务器返回内容</returns>
        [HttpDelete]
        public async Task<IActionResult> BatchDeleteRoles(string idStr)
        {
            int[] idArr = JsonConvert.DeserializeObject<int[]>(idStr);
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (var id in idArr)
            {
                list.Add(new KeyValuePair<string, string>("idArr", id.ToString()));
            }

            QueryString query = QueryString.Create(list);
            string json = await DeleteAsync(query, _urlSettings.Role.BatchDeleteRole);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }
    }
}