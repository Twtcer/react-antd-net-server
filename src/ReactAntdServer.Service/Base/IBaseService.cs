using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace ReactAntdServer.Service.Base
{
    public interface IBaseService
    {  
        IMongoDatabase GetMongoDatabase();
    }

    public interface IBaseService<T> : IBaseService
    {
        IMongoCollection<T> GetMongoCollection(string collectionName);
    }
}
