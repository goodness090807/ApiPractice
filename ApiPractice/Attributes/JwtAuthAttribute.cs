using ApiPractice.Enums;
using ApiPractice.Shared;
using ApiPractice.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ApiPractice.Attributes
{
    public class JwtAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            #region IgnoreValidateAttribute判斷
            //有掛上這個Attribute的，直接就return掉
            if (actionContext.ActionDescriptor.GetCustomAttributes<IgnoreValidateAttribute>(false).Any())
            {
                return;
            }
            #endregion

            ApiResultViewModel result = new ApiResultViewModel();

            #region 判斷有沒有JWT的驗證
            //判斷有沒有Header和判斷是不是JWT的Scheme(Bearer)            
            if (actionContext.Request.Headers.Authorization == null || actionContext.Request.Headers.Authorization.Scheme != "Bearer")
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "驗證Token失敗!!!請確認驗證資料是否正確";

                //回傳Response的一種寫法
                setErrorResponse(actionContext, result);
                return;
            }
            #endregion

            //取得Token 這裡主要取得Header的Authorization屬性
            var token = actionContext.Request.Headers.Authorization.Parameter;
            result = JWTShared.TokenVerification(token);

            #region 驗證Token是否正確
            //驗證Token
            if (!result.Result)
            {
                //回傳Response另一種寫法
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, result);
                return;
            }
            #endregion

            //取出Token資料
            JObject jObject = JObject.Parse(result.ObjectData.ToString());
            string UID = jObject["Id"].ToString();
            string Role = jObject["role"].ToString();

            #region 驗證是否為管理者
            //有掛上這個Attribute的，要驗證是否為管理者
            if (actionContext.ActionDescriptor.GetCustomAttributes<ManagerOnlyAttribute>(false).FirstOrDefault() != null)
            {
                if (!Role.Equals("Manager"))
                {
                    result.Result = false;
                    result.Status = ResponseCode.Fail.ToString();
                    result.Message = "此功能需要管理者權限!!!";
                    result.ObjectData = null;
                    setErrorResponse(actionContext, result);
                    return;
                }
            }
            #endregion

            #region 驗證是否是同一個人
            if (actionContext.ActionDescriptor.GetCustomAttributes<PersonalOnlyAttribute>(false).FirstOrDefault() != null)
            {
                //取得參數中的Id
                string Id = actionContext.ActionArguments["Id"].ToString();

                if (!Role.Equals("Manager"))
                {
                    if (!Id.Equals(UID))
                    {
                        result.Result = false;
                        result.Status = ResponseCode.Fail.ToString();
                        result.Message = "您沒有權限變更他人資料!!!";
                        result.ObjectData = null;
                        setErrorResponse(actionContext, result);
                        return ;
                    }
                }
            }
            #endregion



            //if (actionContext.ModelState.IsValid == false)
            //{
            //    actionContext.Response = actionContext.Request.CreateErrorResponse(
            //        HttpStatusCode.BadRequest, actionContext.ModelState);
            //}


            base.OnActionExecuting(actionContext);

        }

        private static void setErrorResponse(HttpActionContext actionContext, ApiResultViewModel result)
        {
            var response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, result);
            //最主要是將Response這個帶入要回傳的資料
            actionContext.Response = response;
        }
    }
    public class IgnoreValidateAttribute : Attribute
    {
    }

    /// <summary>
    /// 判斷是不是管理者
    /// </summary>
    public class ManagerOnlyAttribute : Attribute
    {
    }

    /// <summary>
    /// 判斷是不是相同使用者
    /// </summary>
    public class PersonalOnlyAttribute : Attribute
    {
    }
}