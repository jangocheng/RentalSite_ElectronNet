using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static ElectronNet.CommonHelper;
using ElectronNet.Models;
using Newtonsoft.Json;

namespace ElectronNet.Controllers
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class RoleController : Controller
    {
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            string json = await GetAsync("", "/v1/Role/ListRoles");
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
            ResultModel res = await GetAsync<ResultModel>("", "/v1/Role/ListRoles");
            Permission[] perArr = JsonConvert.DeserializeObject<Permission[]>(res.Data.ToString());
            return View(perArr);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditRole(long id)
        {
            ResultModel res = await GetAsync<ResultModel>("?id=" + id, "/v1/Role/GetRoleById");
            Role role = JsonConvert.DeserializeObject<Role>(res.Data.ToString());
            return View(role);
        }
    }
}