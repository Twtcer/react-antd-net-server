using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReactAntdServer.Model
{
    public class Manager
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserName { get; set; } 
        public string Password { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        //public long TimeStamps { get; set; }
    }
}
