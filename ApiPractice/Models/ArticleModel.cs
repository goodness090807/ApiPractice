using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiPractice.Models
{
    public class ArticleModel
    {

        [BsonId]
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ArticleID { get; set; }

        [Required(ErrorMessage = "標題欄位必填!!!")]
        [MaxLength(50, ErrorMessage ="標題長度不能大於50個字!!!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "內文欄位必填!!!")]
        [MaxLength(1000, ErrorMessage = "內文長度不能大於1000個字!!!")]
        public string Content { get; set; }

        [JsonIgnore]
        public string UID { get; set; }

        [JsonIgnore]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}