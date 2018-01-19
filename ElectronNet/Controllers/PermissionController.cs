using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ElectronNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronNet.Controllers
{
    public class PermissionController : Controller
    {
        public async Task<IActionResult> ListPermissions()
        {
            //ResultModel res = await CommonHelper.GetAsync<ResultModel>("", "/v1/Permission/ListPermissions");
            ResultModel res = new ResultModel();
            res.Status = 200;
            return View(res);
        }
    }
}