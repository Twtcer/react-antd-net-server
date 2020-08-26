using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ReactAntdServer.Api.Filters
{
    /// <summary>
    /// 模型验证切面
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //base.OnActionExecuting(context);
            if (!context.ModelState.IsValid)
            {
                var result = context.ModelState.Keys.SelectMany(key => context.ModelState[key].Errors.Select(a => new ValidationError(key, a.ErrorMessage))).ToList();
                context.Result = new ObjectResult(result);
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
