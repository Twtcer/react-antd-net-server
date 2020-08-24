using System;
using System.Collections.Generic;
using System.Text;
using ReactAntdServer.Model.Dto;

namespace ReactAntdServer.Service.Base
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(LoginRequest request, out string token);
    }
}
