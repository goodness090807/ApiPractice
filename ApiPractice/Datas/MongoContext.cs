using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPractice.Datas
{
    public class MongoContext
    {

        private MongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private string _collectionName;

        public MongoContext(string CollectionName)
        {
            //設定MongoDb初始值
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _mongoDatabase = _mongoClient.GetDatabase("ApiPracticeData");
            _collectionName = CollectionName;
        }

        public List<T> QueryAll<T>()
        {
            IMongoCollection<T> mongoCollection = _mongoDatabase.GetCollection<T>(_collectionName);
            return mongoCollection.Find(new BsonDocument()).ToList();
        }

        public List<T> QueryWithFilter<T>(FilterDefinition<T> filters)
        {
            IMongoCollection<T> mongoCollection = _mongoDatabase.GetCollection<T>(_collectionName);

            return mongoCollection.Find(filters).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">Filter是可以帶參數查詢的</param>
        /// <returns></returns>
        public T QueryOne<T>(FilterDefinition<T> filters)
        {
            IMongoCollection<T> mongoCollection = _mongoDatabase.GetCollection<T>(_collectionName);

            return mongoCollection.Find(filters).FirstOrDefault();
        }

        public bool InsertOne<T>(T Model)
        {
            try
            {
                IMongoCollection<T> mongoCollection = _mongoDatabase.GetCollection<T>(_collectionName);
                mongoCollection.InsertOne(Model);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool UpdateOne<T>(FilterDefinition<T> filters, UpdateDefinition<T> updates)
        {

            IMongoCollection<T> mongoCollection = _mongoDatabase.GetCollection<T>(_collectionName);

            var result = mongoCollection.UpdateOne(filters, updates);

            long count = 0;

            if (result.IsModifiedCountAvailable)
                count = result.ModifiedCount;
            else
                count = 0;

            return count > 0 ? true : false;
        }

        public bool DeleteOne<T>(FilterDefinition<T> filters)
        {

            IMongoCollection<T> mongoCollection = _mongoDatabase.GetCollection<T>(_collectionName);

            var result = mongoCollection.DeleteOne(filters);

            long count = 0;

            if (result.IsAcknowledged)
                count = result.DeletedCount;
            else
                count = 0;

            return count > 0 ? true : false;
        }
    }
}