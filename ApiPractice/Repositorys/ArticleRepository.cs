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
    public class ArticleRepository
    {
        private MongoContext mongoContext = new MongoContext("ArticleModel");
        public List<ArticleViewModel> GetArticles()
        {
            return mongoContext.QueryAll<ArticleViewModel>();
        }

        public ArticleViewModel GetArticle(string ArticleID)
        {
            FilterDefinitionBuilder<ArticleViewModel> filter = Builders<ArticleViewModel>.Filter;

            FilterDefinition<ArticleViewModel> filters = filter.Eq("ArticleID", ArticleID);

            return mongoContext.QueryOne<ArticleViewModel>(filters);
        }
        public ArticleViewModel GetArticle(string ArticleID, string UID)
        {
            FilterDefinitionBuilder<ArticleViewModel> filter = Builders<ArticleViewModel>.Filter;

            FilterDefinition<ArticleViewModel> filters = filter.Eq("ArticleID", ArticleID) & filter.Eq("UID", UID);

            return mongoContext.QueryOne<ArticleViewModel>(filters);
        }

        public bool AddArticle(ArticleModel articleModel)
        {
            bool IsSuccess = mongoContext.InsertOne(articleModel);

            return IsSuccess;
        }
        public bool UpdateArticle(string ArticleID, string UID, ArticleUpdateModel ArticleModel)
        {
            //查詢資料用的過濾器
            FilterDefinition<ArticleModel> filters = Builders<ArticleModel>.Filter.Eq("ArticleID", ArticleID) & Builders<ArticleModel>.Filter.Eq("UID", UID);
            
            //更新資料用的定義
            UpdateDefinition<ArticleModel> updates = Builders<ArticleModel>.Update.Set("Title", ArticleModel.Title)
                                                                            .Set("Content", ArticleModel.Content);

            return mongoContext.UpdateOne<ArticleModel>(filters, updates);
        }

        public bool DeleteArticle(string ArticleID, string UID)
        {
            var filters = Builders<ArticleModel>.Filter.Eq("ArticleID", ArticleID) & Builders<ArticleModel>.Filter.Eq("UID", UID);

            return mongoContext.DeleteOne<ArticleModel>(filters);
        }

        public List<ArticleViewModel> GetUserArticles(string UID)
        {
            FilterDefinitionBuilder<ArticleViewModel> filter = Builders<ArticleViewModel>.Filter;
            FilterDefinition<ArticleViewModel> filters = filter.Eq("UID", UID);

            return mongoContext.QueryWithFilter<ArticleViewModel>(filters);
        }

    }
}