using ApiPractice.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPractice.ViewModels
{
    public class ApiResultViewModel
    {
        public bool Result { get; set; }

        public string Status { get; set; } = ResponseCode.Success.ToString();

        public string Message { get; set; }

        public object ObjectData { get; set; }
    }
}