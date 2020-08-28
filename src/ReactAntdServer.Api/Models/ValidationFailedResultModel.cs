using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using ReactAntdServer.Model.Dto;

namespace ReactAntdServer.Api.Models
{
    public class ValidationFailedResultModel : ResultWrapper
    {
        public ValidationFailedResultModel(ModelStateDictionary modelState)
        {
            var errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
            Code = ResultCode.InvalidData;
            Message = $"{Code.DisplayName()},{Newtonsoft.Json.JsonConvert.SerializeObject(errors)}"; 
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

    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState):base(new ValidationFailedResultModel(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
