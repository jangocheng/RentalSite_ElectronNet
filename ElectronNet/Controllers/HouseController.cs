using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ElectronNet.ApiSettings;
using ElectronNet.Common;
using ElectronNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using static ElectronNet.CommonHelper;

namespace ElectronNet.Controllers
{
    public class HouseController : Controller
    {
        private readonly FirstVersion _url;
        public HouseController(IOptions<FirstVersion> options)
        {
            _url = options.Value;
        }

        /// <summary>
        /// 房源列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListHouses()
        {
            ResultModel model = await GetAsync<ResultModel>("?typeName=房屋类型", "/v1/IdName");
            var idNames = JsonConvert.DeserializeObject<List<IdName>>(model.Data.ToString());
            idNames.Insert(0, new IdName { Id = 0, Name = "" });
            SelectList items = new SelectList(idNames, "Id", "Name");
            return View(items);
        }

        /// <summary>
        /// 获取房源信息
        /// </summary>
        /// <param name="type">房屋类别</param>
        /// <param name="page">页码</param>
        /// <param name="limit">显示行数</param>
        /// <returns>Json数据列</returns>
        [HttpGet]
        public async Task<IActionResult> GetHouses(long type, int page, int limit)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(page),page.ToString()),
                new KeyValuePair<string, string>(nameof(limit),limit.ToString())
            };
            if (type > 0)
                list.Add(new KeyValuePair<string, string>("typeId", type.ToString()));

            QueryString query = QueryString.Create(list);
            ResultModel model = await GetAsync<ResultModel>(query, _url.House.ListHouses);
            var data = JsonConvert.DeserializeObject<LayuiTableModel<HouseListModel>>(model.Data.ToString());
            data.StatusCode = model.Status;
            data.Message = model.Message;
            Response.StatusCode = model.Status;
            return Json(data);
        }

        /// <summary>
        /// 添加房源页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddHouse()
        {
            ResultModel resModel = await GetAsync<ResultModel>("", _url.House.GetDict);
            var model = JsonConvert.DeserializeObject<HouseAddViewModel>(resModel.Data.ToString());
            return View(model);
        }

        /// <summary>
        /// 获取社区数据
        /// </summary>
        /// <param name="regionId">区域数据Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCommunityByRegionId(long regionId)
        {
            ResultModel model = await GetAsync<ResultModel>("?regionId=" + regionId.ToString(), _url.Community.GetByRegionId);
            Response.StatusCode = model.Status;
            return Json(model);
        }

        /// <summary>
        /// 添加房屋数据
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddHouse(HouseFormModel model)
        {
            ResultModel result = await PostAsync<ResultModel, HouseFormModel>(model, _url.House.AddHouse);
            Response.StatusCode = result.Status;
            return Json(result);
        }

        /// <summary>
        /// 房屋编辑页面
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditHouse(long id)
        {
            ResultModel result = await GetAsync<ResultModel>("?id=" + id, _url.House.GetEditDict);
            HouseEditViewModel model = JsonConvert.DeserializeObject<HouseEditViewModel>(result.Data.ToString());
            return View(model);
        }

        /// <summary>
        /// 编辑房源数据
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditHouse(HouseFormModel model)
        {
            ResultModel result = await PutAsync<ResultModel, HouseFormModel>(model, _url.House.EditHouse);
            Response.StatusCode = result.Status;
            return Json(result);
        }

        /// <summary>
        /// 删除房源
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteHouse(long id)
        {
            ResultModel result = await DeleteAsync<ResultModel>("?id=" + id, _url.House.DeleteHouse);
            Response.StatusCode = result.Status;
            return Json(result);
        }

        /// <summary>
        /// 图片上传视图
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PicUpload(long id)
        {
            return View(id);
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SavePic(long id)
        {
            //判断文件是否存在
            var files = Request.Form.Files;
            if (!files.Any())
            {
                return Json(new ResultModel { Status = (int)HttpStatusCode.BadRequest, Message = "没有找到文件！" });
            }

            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            string[] extArr = { ".jpg", ".png", ".gif", ".jpeg", ".bmp" };
            UpYun upYun = new UpYun
            {
                Operator = "rentalsite",
                Password = "tianxin070221203"
            };

            string host = "https://aspnetweb.b0.upaiyun.com";
            string[] urlArr = new string[2];
            foreach (var file in files)
            {
                //判断文件后缀
                string fileExt = Path.GetExtension(file.FileName);
                if (!extArr.Contains(fileExt))
                {
                    return Json(new ResultModel((int)HttpStatusCode.BadRequest, "选择的图片中包含不支持的格式", null));
                }

                var (isOk, filePath) = await FileUpload(file.OpenReadStream(), fileExt);
                if (!isOk)
                {
                    return Json(new ResultModel((int)HttpStatusCode.BadRequest, "文件上传失败", null));
                }
                urlArr[0] = $"{host}{filePath}";

                //创建缩略图并上传
                byte[] buffer = default;
                using (Image<Rgba32> image = Image.Load(file.OpenReadStream()))
                using (MemoryStream memStream = new MemoryStream())
                {
                    image.Mutate(x =>
                    {
                        x.Resize(image.Width / 3, image.Height / 3);
                    });
                    image.SaveAsPng(memStream);
                    buffer = memStream.GetBuffer();
                    (isOk, filePath) = await FileUpload(buffer, ".png");
                    if (!isOk)
                    {
                        return Json(new ResultModel((int)HttpStatusCode.BadRequest, "文件上传失败", null));
                    }
                }

                urlArr[1] = $"{host}{filePath}";
                keyValuePairs.Add(new KeyValuePair<string, string>("urls", string.Join(',', urlArr)));
            }

            //向服务端发送请求并返回结果
            var response = await CommonHelper.HttpClient.PutAsync($"{ServerUrl}{_url.House.UploadPicture}/{id}", new FormUrlEncodedContent(keyValuePairs));
            string json = await response.Content.ReadAsStringAsync();
            ResultModel res = JsonConvert.DeserializeObject<ResultModel>(json);
            Response.StatusCode = res.Status;
            return Content(json, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 图片列表视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PicListView(long houseId)
        {
            ResultModel result = await GetAsync<ResultModel>("?houseId=" + houseId, _url.House.PictureList);
            List<HousePictureList> list = JsonConvert.DeserializeObject<List<HousePictureList>>(result.Data.ToString());
            Response.StatusCode = result.Status;
            return View(list);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="picIds">图片的Id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePic(string picArr)
        {
            int[] picIds = JsonConvert.DeserializeObject<int[]>(picArr);
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            foreach (var picId in picIds)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(nameof(picIds), picId.ToString()));
            }
            QueryString queryString = QueryString.Create(keyValuePairs);

            ResultModel result = await DeleteAsync<ResultModel>(queryString, _url.House.DeletePic);
            Response.StatusCode = result.Status;
            return Json(result);
        }
    }
}