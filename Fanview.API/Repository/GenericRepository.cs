using Fanview.API.Repository.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Fanview.API.Clients;

namespace FanviewPollingService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly IMongoDbClient _mongoDbClient;

        public GenericRepository(IMongoDbClient mongoDbClient)
        {  
            _mongoDbClient = mongoDbClient;
        }

        public IMongoCollection<T> GetMongoDbCollection(string collectionName)
        {
            return _mongoDbClient.Database.GetCollection<T>(collectionName);
        }

        public async Task<DeleteResult> DeleteOne(FilterDefinition<T> filter, string collectionName)
        {
            var collection = _mongoDbClient.Database.GetCollection<T>(collectionName);
            return await collection.DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> DeleteMany(FilterDefinition<T> filter, string collectionName)
        {
            var collection = _mongoDbClient.Database.GetCollection<T>(collectionName);
            
            return await collection.DeleteManyAsync(filter);
           
        }
     
         
        public async Task<IEnumerable<T>> GetAll(string collectionName)
        {
            return await Task.FromResult(_mongoDbClient.Database.GetCollection<T>(collectionName).AsQueryable());           
        }
        
        public async void Insert(IEnumerable<T> entity, string collectionName)
        {
           var collection = _mongoDbClient.Database.GetCollection<T>(collectionName);
           await collection.InsertManyAsync(entity);
        }

        public async void Insert(T entity, string collectionName)
        {
            var collection = _mongoDbClient.Database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(entity);
        }

   
        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            var collection = _mongoDbClient.Database.GetCollection<T>(collectionName);
            collection.UpdateOne(filter, update);
        }

        public async void Replace(T entity, FilterDefinition<T> filter, string collectionName)
        {
            var collection = _mongoDbClient.Database.GetCollection<T>(collectionName);
            await collection.ReplaceOneAsync(filter, entity);
            
        }
    }
  
}
