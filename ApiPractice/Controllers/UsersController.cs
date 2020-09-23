using ApiPractice.Attributes;
using ApiPractice.Enums;
using ApiPractice.Models;
using ApiPractice.Repositorys;
using ApiPractice.Shared;
using ApiPractice.UpdateModels;
using ApiPractice.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiPractice.Controllers
{
    /// <summary>
    /// 使用者資訊
    /// </summary>
    //可以注意一下各種不同回傳方式
    public class UsersController : ApiController
    {
        //資料庫連線
        private UserRepository userRepository = new UserRepository();

        #region 取得所有使用者資料
        /// <summary>
        /// 取得所有使用者資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUsers()
        {

            List<UserViewModel> userModels = userRepository.GetUsers();

            ApiResultViewModel result = new ApiResultViewModel()
            { 
                Result = true,
                Status = ResponseCode.Success.ToString(),
                ObjectData = userModels
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region 取得其中一筆使用者資料
        /// <summary>
        /// 取得其中一筆使用者資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUser(string Id)
        {
            ApiResultViewModel result = new ApiResultViewModel();

            UserViewModel user = userRepository.GetUser(Id);

            if (user == null)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "查無資料!!!";
            }
            else
            {
                result.Result = true;
                result.ObjectData = user;
            }

            return Ok(result);
        }
        #endregion

        #region 新增使用者(管理者使用)
        /// <summary>
        /// 新增使用者(管理者使用)
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ManagerOnly]
        public HttpResponseMessage AddUser([FromBody] UserModel userModel)
        {
            ApiResultViewModel result = new ApiResultViewModel();

            if (!ModelState.IsValid)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = AllShared.GetModelStateError(ModelState); 
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            //取得加密密碼
            userModel.UserPwd = userModel.UserPwd.GetMD5();

            bool IsSuccess = userRepository.AddUser(userModel);

            if(IsSuccess)
            {
                result.Result = true;
                result.Status = ResponseCode.Success.ToString();
                result.Message = "新增成功!!!";
            }
            else
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "新增資料時發生錯誤!!!";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);

            //HttpResponseMessage httpRequestMessage = Request.CreateResponse(HttpStatusCode.OK, result);
            //return Request.CreateResponse(httpRequestMessage);
        }
        #endregion

        #region 更新使用者資料(個人使用者使用)
        /// <summary>
        /// 更新使用者資料(個人使用者使用)
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPut]
        [PersonalOnly]
        public IHttpActionResult UpdateUser(string Id, [FromBody]UserUpdateModel userInfo)
        {
            ApiResultViewModel result = new ApiResultViewModel();

            if (!ModelState.IsValid)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = AllShared.GetModelStateError(ModelState);
                return Ok(result);
            }

            if(userInfo == null)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "無修改資料相關參數";
                return Ok(result);
            }

            //取得使用者ID
            //string UserID = JWTShared.GetUserID(Request.Headers.Authorization.Parameter);
            //if (Id != UserID)
            //{
            //    result.Result = false;
            //    result.Status = ResponseCode.Fail.ToString();
            //    result.Message = "錯誤的使用者ID";
            //    return Ok(result);
            //}

            bool IsSuccess = userRepository.UpdateUser(Id, userInfo);

            if (IsSuccess)
            {
                result.Result = true;
                result.Status = ResponseCode.Success.ToString();
                result.Message = "更新資料成功!!!";
            }
            else
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "更新資料失敗，查無使用者資訊!!!";
            }

            return Ok(result);
        }
        #endregion

        #region 刪除使用者
        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PersonalOnly]
        public HttpResponseMessage DeleteUser(string Id)
        {
            ApiResultViewModel result = new ApiResultViewModel();


            bool IsSuccess = userRepository.DeleteUser(Id);

            if (IsSuccess)
            {
                result.Result = true;
                result.Status = ResponseCode.Success.ToString();
                result.Message = "刪除成功!!!";
            }
            else
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "刪除使用者發生錯誤!!!";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion
    }
}
