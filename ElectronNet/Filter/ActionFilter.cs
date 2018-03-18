using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace ElectronNet
{
    public class ActionFilter : IActionFilter
    {
        private readonly IMemoryCache _cache;

        public ActionFilter(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //判断是否为ajax请求
            if (context.HttpContext.IsAjax())
            {
                return;
            }

            //判断响应码是否为200
            if (context.HttpContext.Response.StatusCode == 200)
            {
                return;
            }

            //判断Result是否可以转换为ViewResult
            if (context.Result is ViewResult viewResult && context.HttpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                viewResult.ViewName = "/Home/PromptView";
                context.Result = viewResult;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var routeDict = context.ActionDescriptor.RouteValues;
            if (routeDict["controller"] == "Home" && routeDict["action"] == "Login")
                return;

            bool res = _cache.TryGetValue("adminUserId", out long adminUserId);
            if (!res)
            {
                context.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}
