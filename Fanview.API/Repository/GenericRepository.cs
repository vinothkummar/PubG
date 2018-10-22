using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver.Linq;

namespace FanviewPollingService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {   
        private IMongoDatabase database;
        private IMongoCollection<T> _collection;
        private IConfiguration _configuration;

        public GenericRepository()
        {  
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();          
            var client = new MongoClient(this._configuration["Logging:AppSettings:ConnectionString"]);
            database = client.GetDatabase("FanviewPubGDb");
           
        }

        public IMongoCollection<T> GetMongoDbCollection(string collectionName)
        {
            return database.GetCollection<T>(collectionName);
        }

        public async Task<DeleteResult> DeleteOne(FilterDefinition<T> filter, string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);
            return await collection.DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> DeleteMany(FilterDefinition<T> filter, string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);
            return await collection.DeleteManyAsync(filter);
        }
         
        public async Task<IEnumerable<T>> GetAll(string collectionName)
        {
            return await Task.FromResult(database.GetCollection<T>(collectionName).AsQueryable());           
        }
        
        public async void Insert(IEnumerable<T> entity, string collectionName)
        {
           var collection = database.GetCollection<T>(collectionName);
           await collection.InsertManyAsync(entity);
        }

        public async void Insert(T entity, string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(entity);
        }

   
        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            var collection = database.GetCollection<T>(collectionName);
            collection.UpdateOne(filter, update);
        }

        public async void Replace(T entity, FilterDefinition<T> filter, string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);
            await collection.ReplaceOneAsync(filter, entity);
            
        }
    }
  
}
