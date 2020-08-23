using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReactAntdServer.Model.Data
{
    public class Product: BaseModel
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }   
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("descriptions")]
        public string Descriptions { get; set; }
        [BsonElement("onSale")]
        public bool OnSale { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        [BsonElement("price")]
        [BsonRepresentation(BsonType.Double)]
        public decimal Price { get; set; }
        [BsonElement("coverImg")]
        public string CoverImg { get; set; }
        [BsonElement("productCategory")]
        public string ProductCategory { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreationTime { get; set; }
        [BsonElement("updatedAt")]
        public DateTime UpdationTime { get; set; } 
        [BsonElement("__v")]
        public int Version { get; set; }
    }
}
