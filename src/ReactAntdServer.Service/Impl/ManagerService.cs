using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;
using MongoDB.Driver;
using ReactAntdServer.Model;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Model.Dto;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Service
{
    public class ManagerService : BaseContextService<Manager>, IManagerService
    {
        public ManagerService(IBookstoreDatabaseSettings settings) : base(settings, settings.ManagersCollectionName)
        {

        }

        public bool IsValild(LoginRequestDTO req)
        {
            var find = Collection.Find(a => a.UserName == req.Username).FirstOrDefault();
            if (find == null)
            {
                return false;
            }
            if (BCrypt.Net.BCrypt.Verify(req.Password, find.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
