using System;
using System.Collections.Generic;
using System.Text;
using ReactAntdServer.Model;
using ReactAntdServer.Model.Dto;

namespace ReactAntdServer.Service.Base
{
    public interface IManagerService
    {
        bool IsValild(LoginRequest req,out Manager manager);
    }
}
