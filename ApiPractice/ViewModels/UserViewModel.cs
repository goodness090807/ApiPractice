using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiPractice.ViewModels
{
    [BsonIgnoreExtraElements]
    public class UserViewModel
    {
        [BsonId]
        public string UID { get; set; }

        [Required(ErrorMessage = "使用者名稱不能為空")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "性別不能為空")]
        public bool? Gender { get; set; }

        [Required(ErrorMessage = "信箱不能為空")]
        public string Mail { get; set; }
    }
}