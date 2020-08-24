using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactAntdServer.Api.Attributes;
using ReactAntdServer.Api.Utils;
using ReactAntdServer.Model.Dto;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Api.Controllers
{
    //[CustomRoute]
    [ApiController]
    public class AuthenticationController: ControllerBase
    {
        private readonly IAuthenticateService _authService;
        public AuthenticationController(IAuthenticateService authenticate)
        {
            this._authService = authenticate;
        } 

        [AllowAnonymous]
        [HttpPost, Route("requestToken")]
        public ActionResult RequestToken([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            var token = string.Empty;
            if (_authService.IsAuthenticated(request, out token))
            {
                return Ok(token);
            }

            return Unauthorized("401 Unauthorized");
        }


        [HttpPost, AllowAnonymous]
        [Route("api/v1/auth/manager_login")]
        public ActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            var token = string.Empty;
            if (_authService.IsAuthenticated(request, out token))
            {
                return Ok(token);
            }

            return Unauthorized("401 Unauthorized");
        }

    }
}
