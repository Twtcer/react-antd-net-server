using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Service
{
    public class BaseContextService<T>:IBaseService,IDisposable
    {
        private IMongoDatabase Database;

        public IMongoCollection<T> Collection;
        public BaseContextService(IBookstoreDatabaseSettings settings,string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);
            Collection = Database.GetCollection<T>(collectionName);
        }

        //public IMongoDatabase GetMongoDatabase()
        //{
        //    return this.Database;
        //} 

        //public IMongoCollection<T> GetCollection(string name)
        //{
        //    return this.Database.GetCollection<T>(name);
        //}

        public void Dispose()
        {
            //Database.
        }
    }
}
