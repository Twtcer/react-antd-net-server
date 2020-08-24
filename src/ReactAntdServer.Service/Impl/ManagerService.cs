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

        public bool IsValild(LoginRequest req,out Manager manager)
        {
            manager = Collection.Find(a => a.UserName == req.Username).FirstOrDefault(); 
            if (manager == null)
            { 
                return false;
            }
            if (BCrypt.Net.BCrypt.Verify(req.Password, manager.Password))
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
