using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using ReactAntdServer.Model;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Model.Dto;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Service
{
    public class ManagerService : BaseContextService<Manager>, IManagerService
    {
        private readonly IMongoCollection<Manager> _mangers;
        public ManagerService(IBookstoreDatabaseSettings settings):base(settings)
        {
            _mangers = GetCollection(settings.ManagersCollectionName);
        } 

        public bool IsValild(LoginRequestDTO req)
        {
            var find = _mangers.Find(a => a.UserName == req.Username && a.Password == req.Password); 
            return find==null?false:true;
        }
    }
}
