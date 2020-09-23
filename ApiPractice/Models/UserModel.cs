using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiPractice.Models
{
    public class UserModel
    {
        [BsonId]
        [Required(ErrorMessage = "UID欄位為必填!!!")]
        public string UID { get; set; }

        [Required(ErrorMessage = "UserPwd欄位為必填!!!")]
        public string UserPwd { get; set; }

        [Required(ErrorMessage = "Role欄位為必填!!!")]
        public string Role { get; set; }

        [Required(ErrorMessage = "UserName欄位為必填!!!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Gender欄位為必填!!!")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Mail欄位為必填!!!")]
        public string Mail { get; set; }
    }
}