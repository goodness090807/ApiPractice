using ApiPractice.Attributes;
using ApiPractice.Enums;
using ApiPractice.Models;
using ApiPractice.Repositorys;
using ApiPractice.Shared;
using ApiPractice.UpdateModels;
using ApiPractice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiPractice.Controllers
{
    /// <summary>
    /// 文章資訊
    /// </summary>
    public class ArticlesController : ApiController
    {
        private ArticleRepository articleRepository = new ArticleRepository();

        #region 取得所有文章
        /// <summary>
        /// 取得所有文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetArticles()
        {
            List<ArticleViewModel> ArticleViewModels = articleRepository.GetArticles();

            ApiResultViewModel result = new ApiResultViewModel()
            {
                Result = true,
                Status = ResponseCode.Success.ToString(),
                ObjectData = ArticleViewModels
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region 取得特定文章資料
        /// <summary>
        /// 取得特定文章資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetArticle(string Id)
        {
            ArticleViewModel ArticleViewModel = articleRepository.GetArticle(Id);

            ApiResultViewModel result = new ApiResultViewModel()
            {
                Result = true,
                Status = ResponseCode.Success.ToString(),
                ObjectData = ArticleViewModel
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region 新增文章
        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="articleModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddArticle([FromBody] ArticleModel articleModel)
        {
            ApiResultViewModel result = new ApiResultViewModel();

            //驗證欄位資訊
            if (!ModelState.IsValid)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = AllShared.GetModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            //取得使用者ID
            string UserID = JWTShared.GetUserID(Request.Headers.Authorization.Parameter);
            articleModel.UID = UserID;

            bool IsSuccess = articleRepository.AddArticle(articleModel);

            if (IsSuccess)
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
        }
        #endregion

        #region 修改文章
        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="UpdateInfo"></param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage UpdateArticle(string Id, [FromBody] ArticleUpdateModel UpdateInfo)
        {
            ApiResultViewModel result = new ApiResultViewModel();

            if (!ModelState.IsValid)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = AllShared.GetModelStateError(ModelState);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            if(UpdateInfo == null)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "無修改資料相關參數";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            //取得使用者ID
            string UserID = JWTShared.GetUserID(Request.Headers.Authorization.Parameter);

            ArticleViewModel ArticleViewModel = articleRepository.GetArticle(Id, UserID);

            if (UpdateInfo.Title == null)
                UpdateInfo.Title = ArticleViewModel.Title;
            if (UpdateInfo.Content == null)
                UpdateInfo.Content = ArticleViewModel.Content;

            if (ArticleViewModel == null)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "查無此文章資料!!!";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            bool IsSuccess = articleRepository.UpdateArticle(Id, UserID, UpdateInfo);

            if (IsSuccess)
            {
                result.Result = true;
                result.Status = ResponseCode.Success.ToString();
                result.Message = "修改成功!!!";
            }
            else
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "修改文章資料發生錯誤!!!";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region 刪除文章
        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public HttpResponseMessage DeleteArticle(string Id)
        {
            ApiResultViewModel result = new ApiResultViewModel();

            //取得使用者ID
            string UserID = JWTShared.GetUserID(Request.Headers.Authorization.Parameter);

            ArticleViewModel ArticleViewModel = articleRepository.GetArticle(Id, UserID);

            if (ArticleViewModel == null)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "查無此文章資料!!!";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            bool IsSuccess = articleRepository.DeleteArticle(Id, UserID);

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
                result.Message = "刪除文章資料發生錯誤!!!";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region 取得使用者的文章資訊
        /// <summary>
        /// 取得使用者的文章資訊
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("~/api/Users/{Id}/Articles")]
        [HttpGet]
        public HttpResponseMessage GetUserArticles(string Id)
        {
            ApiResultViewModel result = new ApiResultViewModel();

            List<ArticleViewModel> aritcles = articleRepository.GetUserArticles(Id);

            if (aritcles == null)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "查無資料!!!";
            }
            else
            {
                result.Result = true;
                result.ObjectData = aritcles;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion
    }
}
