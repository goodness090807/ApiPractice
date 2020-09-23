using ApiPractice.Attributes;
using ApiPractice.Enums;
using ApiPractice.Models;
using ApiPractice.Repositorys;
using ApiPractice.Shared;
using ApiPractice.ViewModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiPractice.Controllers
{
    /// <summary>
    /// 使用者登入
    /// </summary>
    public class LoginController : ApiController
    {
        private UserRepository userRepository = new UserRepository();

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreValidate]
        public HttpResponseMessage Login([FromBody]LoginModel loginModel)
        {
            ApiResultViewModel result = new ApiResultViewModel();

            UserModel userModel = userRepository.GetUser(loginModel.UID, loginModel.Pwd.GetMD5());

            if (userModel == null)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "查無帳號資訊!!!";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }                

            string token = JWTShared.CreateToken(userModel);

            result.Result = true;
            result.Status = ResponseCode.Success.ToString();
            result.Message = "登入成功!!!";
            result.ObjectData = new { token = token };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
