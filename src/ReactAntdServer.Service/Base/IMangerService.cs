using System;
using System.Collections.Generic;
using System.Text;
using ReactAntdServer.Model.Dto;

namespace ReactAntdServer.Service.Base
{
    public interface IManagerService
    {
        bool IsValild(LoginRequestDTO req);
    }
}
