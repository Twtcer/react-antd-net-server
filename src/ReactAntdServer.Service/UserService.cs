using System;
using System.Collections.Generic;
using System.Text;
using ReactAntdServer.Model.Dto;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Service
{
    public class UserService : IUserService
    {
        public bool IsValild(LoginRequestDTO req)
        {
            return true;
        }
    }
}
