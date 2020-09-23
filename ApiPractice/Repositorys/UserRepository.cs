using ApiPractice.Datas;
using ApiPractice.Models;
using ApiPractice.UpdateModels;
using ApiPractice.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPractice.Repositorys
{
    public class UserRepository
    {
        private MongoContext mongoContext = new MongoContext("UserModel");

        public List<UserViewModel> GetUsers()
        {
            return mongoContext.QueryAll<UserViewModel>();
        }

        public UserViewModel GetUser(string UID)
        {
            FilterDefinitionBuilder<UserViewModel> filter = Builders<UserViewModel>.Filter;

            FilterDefinition<UserViewModel> filters = filter.Eq("UID", UID);

            return mongoContext.QueryOne<UserViewModel>(filters);
        }

        /// <summary>
        /// 透過帳號密碼取得使用者資訊
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="UserPwd"></param>
        /// <returns></returns>
        public UserModel GetUser(string UID, string UserPwd)
        {
            FilterDefinitionBuilder<UserModel> filter = Builders<UserModel>.Filter;

            FilterDefinition<UserModel> filters = filter.Eq("UID", UID) & filter.Eq("UserPwd", UserPwd);

            return mongoContext.QueryOne<UserModel>(filters);
        }

        public bool AddUser(UserModel userModel)
        {
            bool IsSuccess = mongoContext.InsertOne(userModel);

            return IsSuccess;
        }
        public bool UpdateUser(string UID, UserUpdateModel userModel)
        {
            //查詢資料用的過濾器
            FilterDefinition<UserModel> filters = Builders<UserModel>.Filter.Eq("UID", UID);
            //更新資料用的定義
            UpdateDefinition<UserModel> updates = Builders<UserModel>.Update.Set("UserName", userModel.UserName)
                                                                            .Set("Gender", userModel.Gender)
                                                                            .Set("Mail", userModel.Mail);

            return mongoContext.UpdateOne<UserModel>(filters, updates);
        }
        public bool DeleteUser(string UID)
        {
            var filters = Builders<UserModel>.Filter.Eq("UID", UID);

            return mongoContext.DeleteOne<UserModel>(filters);
        }
    }
}