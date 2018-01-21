using Exceptionless;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronNet
{
    public class HttpMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public HttpMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="httpContext">HttpContext上下文对象</param>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch (Exception exception)
            {
                //管道异常处理
                exception.ToExceptionless().Submit();
            }
        }
    }
}
