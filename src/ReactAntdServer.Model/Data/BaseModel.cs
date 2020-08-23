using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReactAntdServer.Model.Data
{
    public class BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        //[BsonRepresentation(BsonType.DateTime)]
        //public DateTime CreationTime { get; set; }
        //[BsonRepresentation(BsonType.DateTime)]
        //public DateTime UpdatinTime { get; set; } 
    }
}
