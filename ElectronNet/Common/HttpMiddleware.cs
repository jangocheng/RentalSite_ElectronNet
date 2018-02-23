using ElectronNet.Models;
using Exceptionless;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ElectronNet
{
    public class HttpMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="httpContext">HttpContext上下文对象</param>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                //判断结果是否为404
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    if (httpContext.IsAjax())
                    {
                        httpContext.Response.ContentType = "application/json";
                        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ResultModel(404, "地址错误！", null), CommonHelper.JsonSerializerSettings));
                    }
                    else
                    {
                        httpContext.Response.Redirect("/Home/NotFoundView");
                    }
                }
            }
            catch (Exception exception)
            {
                //提交日志
                exception.ToExceptionless().Submit();
                //判断是否为Ajax
                if (httpContext.IsAjax())
                {
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ResultModel(500, "程序出错！", null), CommonHelper.JsonSerializerSettings));
                }
                else
                {
                    httpContext.Response.Redirect("/Home/ErrorView");
                }

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
