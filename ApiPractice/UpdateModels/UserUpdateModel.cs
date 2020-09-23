using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiPractice.UpdateModels
{
    public class UserUpdateModel
    {
        [Required(ErrorMessage = "UserName欄位為必填!!!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Gender欄位為必填!!!")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Mail欄位為必填!!!")]
        public string Mail { get; set; }
    }
}