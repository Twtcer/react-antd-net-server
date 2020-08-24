using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReactAntdServer.Model
{
    public class Manager
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("userName")]
        public string UserName { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("nickName")]
        public string NickName { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreationTime { get; set; }
        [BsonElement("updatedAt")]
        public DateTime UpdationTime { get; set; }
        [BsonElement("roles")]
        public string Roles { get; set; }
        [BsonElement("__v")]
        public int Version { get; set; }
    }
}
