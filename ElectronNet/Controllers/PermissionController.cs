using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronNet.Controllers
{
    public class PermissionController : Controller
    {
        public async Task<IActionResult> ListPermissions()
        {
            var response = await CommonHelper.HttpClient.GetAsync(CommonHelper.ServerUrl + "/v1/Permission/ListPermissions");
            string json = await response.Content.ReadAsStringAsync();
            QueryString
            return View();
        }
    }
}