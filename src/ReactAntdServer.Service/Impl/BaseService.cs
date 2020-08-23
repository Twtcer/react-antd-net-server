using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Service
{
    public class BaseContextService<T>:IBaseService
    {
        public IMongoDatabase Database;
        public BaseContextService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);  
        } 
        public IMongoDatabase GetMongoDatabase()
        {
            return this.Database;
        }

        public IMongoCollection<T> GetCollection(string name)
        {
            return this.Database.GetCollection<T>(name);
        }
    }
}
