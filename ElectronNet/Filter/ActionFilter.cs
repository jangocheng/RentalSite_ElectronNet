using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ElectronNet
{
    public class ActionFilter : IActionFilter
    {
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

        }
    }
}
