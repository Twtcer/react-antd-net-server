using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using ReactAntdServer.Api.Models;

namespace ReactAntdServer.Api.Filters
{
    public class CustomExceptionAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;

            //处理其他异常

            context.ExceptionHandled = true;
            context.Result = new CustomExceptionResult((int)httpStatus, context.Exception);
        }
    }
}
