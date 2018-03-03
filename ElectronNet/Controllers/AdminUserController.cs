using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ElectronNet.ApiSettings;
using ElectronNet.Common;
using ElectronNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static ElectronNet.CommonHelper;

namespace ElectronNet.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly FirstVersion _urlSettings;

        public AdminUserController(IMemoryCache memoryCache, IOptions<FirstVersion> options)
        {
            _cache = memoryCache;
            _urlSettings = options.Value;
        }

        /// <summary>
        /// 后台用户列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ListAdminUsers()
        {
            return View();
        }

        /// <summary>
        /// 获取后台用户列表数据
        /// </summary>
        /// <param name="searchStr">查询条件</param>
        /// <param name="page">页码</param>
        /// <param name="limit">显示行数</param>
        /// <returns>Json数据</returns>
        [HttpGet]
        public async Task<IActionResult> GetAdminUsers(string searchStr, int page, int limit)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(page),page.ToString()),
                new KeyValuePair<string, string>(nameof(limit),limit.ToString())
            };

            string url;
            if (!string.IsNullOrEmpty(searchStr))
            {
                url = _urlSettings.AdminUser.Search;
                list.Add(new KeyValuePair<string, string>(nameof(searchStr), searchStr));
            }
            else
                url = _urlSettings.AdminUser.ListAdminUsers;
            QueryString query = QueryString.Create(list);

            ResultModel model = await GetAsync<ResultModel>(query, url);
            LayuiTableModel<AdminUserListModel> layuiTable;
            if (model.Data != null)
                layuiTable = JsonConvert.DeserializeObject<LayuiTableModel<AdminUserListModel>>(model.Data.ToString());
            else
                layuiTable = new LayuiTableModel<AdminUserListModel>
                {
                    Count = 0,
                    Data = new List<AdminUserListModel>()
                };

            layuiTable.StatusCode = model.Status;
            layuiTable.Message = model.Message;
            Response.StatusCode = model.Status;

            return Json(layuiTable);
        }

        /// <summary>
        /// 添加后台用户页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddAdminUser()
        {
            ResultModel roleRes = await GetAsync<ResultModel>("", _urlSettings.Role.ListRoles);
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(roleRes.Data.ToString());

            ResultModel cityRes = await GetAsync<ResultModel>("", _urlSettings.City.ListCities);
            List<City> cities = JsonConvert.DeserializeObject<List<City>>(cityRes.Data.ToString());
            cities.Insert(0, new City());
            ViewBag.Cities = new SelectList(cities, "Id", "Name");

            return View(roles);
        }

        /// <summary>
        /// 添加后台用户
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <param name="headImg">头像文件</param>
        /// <returns>Json提示</returns>
        [HttpPost]
        public async Task<IActionResult> AddAdminUser(AdminUserFormModel model, IFormFile headImg)
        {
            string headImgUrl = string.Empty;
            if (headImg?.Length > 1024 * 1024)
            {
                return Json(new ResultModel((int)HttpStatusCode.BadRequest, "上传的图片不能超过1MB", null));
            }
            else if (headImg?.Length > 0)
            {
                //判断文件格式
                string[] extArr = { ".jpg", ".png", ".gif", ".jpeg" };
                string fileExt = Path.GetExtension(headImg.FileName);
                if (!extArr.Contains(fileExt))
                {
                    return Json(new ResultModel((int)HttpStatusCode.BadRequest, "选择的图片中包含不支持的格式", null));
                }

                //计算文件的MD5值并拼接文件路径
                string fileMd5 = CalcMD5(headImg.OpenReadStream());
                string fileName = $"{fileMd5}{fileExt}";
                DateTime now = DateTime.Now;
                string fullPath = $"/uploadFile/{now.Year}/{now.Month}/{now.Day}/{fileName}";

                //文件上传至又拍云中
                UpYun upYun = new UpYun
                {
                    Operator = "rentalsite",
                    Password = "tianxin070221203"
                };

                byte[] fileByteArr = StreamToBytes(headImg.OpenReadStream());  //Stream转换byte[]
                bool isOK = await upYun.WriteFileAsync(fileByteArr, fullPath);
                if (!isOK)
                {
                    return Json(new ResultModel((int)HttpStatusCode.BadRequest, "文件上传失败", null));
                }

                headImgUrl = $"https://aspnetweb.b0.upaiyun.com{fullPath}";
            }

            model.HeadImgUrl = headImgUrl;
            string json = await PostAsync(model, _urlSettings.AdminUser.AddAdminUser);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 编辑后台用户页面
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditAdminUser(long id)
        {
            ResultModel allRoleRes = await GetAsync<ResultModel>("", _urlSettings.Role.ListRoles);
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(allRoleRes.Data.ToString());

            ResultModel selRoleRes = await GetAsync<ResultModel>("?adminUserId=" + id, _urlSettings.Role.GetByAdminUserId);
            List<Role> selRoles = JsonConvert.DeserializeObject<List<Role>>(selRoleRes.Data.ToString());

            ResultModel res = await GetAsync<ResultModel>("?id=" + id, _urlSettings.AdminUser.GetAdminUserById);
            Response.StatusCode = res.Status;
            AdminUserFormModel formModel = JsonConvert.DeserializeObject<AdminUserFormModel>(res.Data.ToString());

            ResultModel cityRes = await GetAsync<ResultModel>("", _urlSettings.City.ListCities);
            List<City> cities = JsonConvert.DeserializeObject<List<City>>(cityRes.Data.ToString());
            cities.Insert(0, new City());
            var selectList = new SelectList(cities, "Id", "Name", formModel.CityId);

            AdminUserEditViewModel viewModel = new AdminUserEditViewModel
            {
                AdminUser = formModel,
                Cities = selectList,
                Roles = roles,
                SelRoles = selRoles
            };

            return View(viewModel);
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <param name="headImg">头像文件</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditAdminUser(AdminUserFormModel model, IFormFile headImg)
        {
            string headImgUrl = string.Empty;
            if (headImg?.Length > 1024 * 1024)
            {
                return Json(new ResultModel((int)HttpStatusCode.BadRequest, "上传的图片不能超过1MB", null));
            }
            else if (headImg?.Length > 0)
            {
                //判断文件格式
                string[] extArr = { ".jpg", ".png", ".gif", ".jpeg" };
                string fileExt = Path.GetExtension(headImg.FileName);
                if (!extArr.Contains(fileExt))
                {
                    return Json(new ResultModel((int)HttpStatusCode.BadRequest, "选择的图片中包含不支持的格式", null));
                }

                //计算文件的MD5值并拼接文件路径
                string fileMd5 = CalcMD5(headImg.OpenReadStream());
                string fileName = $"{fileMd5}{fileExt}";
                DateTime now = DateTime.Now;
                string fullPath = $"/uploadFile/{now.Year}/{now.Month}/{now.Day}/{fileName}";

                //文件上传至又拍云中
                UpYun upYun = new UpYun
                {
                    Operator = "rentalsite",
                    Password = "tianxin070221203"
                };

                byte[] fileByteArr = StreamToBytes(headImg.OpenReadStream());  //Stream转换byte[]
                bool isOK = await upYun.WriteFileAsync(fileByteArr, fullPath);
                if (!isOK)
                {
                    return Json(new ResultModel((int)HttpStatusCode.BadRequest, "文件上传失败", null));
                }

                headImgUrl = $"https://aspnetweb.b0.upaiyun.com{fullPath}";
            }

            model.HeadImgUrl = headImgUrl;
            string json = await PutAsync(model, _urlSettings.AdminUser.EditAdminUser);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idStr">Id数组</param>
        /// <returns>服务器返回内容</returns>
        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAdminUsers(string idStr)
        {
            int[] idArr = JsonConvert.DeserializeObject<int[]>(idStr);
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (var id in idArr)
            {
                list.Add(new KeyValuePair<string, string>("idArr", id.ToString()));
            }

            QueryString query = QueryString.Create(list);
            string json = await DeleteAsync(query, _urlSettings.AdminUser.BatchDeleteAdminUser);
            Response.StatusCode = JsonConvert.DeserializeObject<ResultModel>(json).Status;
            return Content(json, "application/json", Encoding.UTF8);
        }
    }
}