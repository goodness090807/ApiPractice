using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.ModelBinding;

namespace ApiPractice.Shared
{
    public static class AllShared
    {
        public static string GetModelStateError(ModelStateDictionary ErrorDatas)
        {
            StringBuilder sbErrMsg = new StringBuilder();

            foreach(var value in ErrorDatas.Values)
            {
                if(value.Errors.Count > 0)
                {
                    foreach(var ErrValue in value.Errors)
                    {
                        sbErrMsg.Append($"{ ErrValue.ErrorMessage.ToString()}");
                    }
                }
                    
            }

            return sbErrMsg.ToString();
        }
    }
}