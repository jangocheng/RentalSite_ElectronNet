using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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


        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
