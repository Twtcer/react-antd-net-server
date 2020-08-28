using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ReactAntdServer.Api.Models;

namespace ReactAntdServer.Api.Filters
{
    /// <summary>
    /// 模型验证切面
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        { 
            if (!context.ModelState.IsValid)
            {  
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }

        public class ValidationError
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Field { get; }
            public string Message { get; }
            public ValidationError(string field, string message)
            {
                Field = field != string.Empty ? field : null;
                Message = message;
            }
        }
    }
}
