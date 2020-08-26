using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ReactAntdServer.Model
{
    public class Book
    {
        /// <summary>
        /// ObjectId
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        //[JsonIgnore]
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [BsonElement("Name")]
        [Required(ErrorMessage = "图书名称不能为空")]
        public string BookName { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [Range(1, 999999, ErrorMessage = "价格必须介于1~999999之间")]
        public decimal Price { get; set; }
        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Author
        /// </summary>
        public string Author { get; set; }
    }
}
