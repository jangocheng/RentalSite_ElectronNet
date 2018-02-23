using ElectronNet.Models;
using Exceptionless;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ElectronNet
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Exception.ToExceptionless().Submit();
            //判断Result是否可以转换为ViewResult
            if (context.HttpContext.Request.Method.ToLower() == "get")
            {
                context.Result = new RedirectResult("/Home/PromptView");
            }
            else if (context.HttpContext.IsAjax())
            {
                context.Result = new JsonResult(new ResultModel((int)HttpStatusCode.InternalServerError, "程序出错！", null));
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
