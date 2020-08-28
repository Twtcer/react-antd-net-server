using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReactAntdServer.Model.Dto;

namespace ReactAntdServer.Api.Models
{
    public class CustomExceptionResultModel : ResultWrapper
    {
        public CustomExceptionResultModel(int? code, Exception exception)
        {
            //Code = code;
            var message = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
            Message = $"Error Code: {code},{Newtonsoft.Json.JsonConvert.SerializeObject(message)}";
        }
    }

    public class CustomExceptionResult : ObjectResult
    {
        public CustomExceptionResult(int? code,Exception exception) : base(new CustomExceptionResultModel(code,exception))
        {
            StatusCode = code;
        }
    }
}
