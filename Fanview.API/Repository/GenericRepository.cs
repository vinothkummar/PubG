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

namespace FanviewPollingService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {   
        private IMongoDatabase database;
        private IConfiguration _configuration;

        public GenericRepository()
        {  
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();          
            var client = new MongoClient(this._configuration["Logging:AppSettings:ConnectionString"]);
            database = client.GetDatabase("FanviewPubGDb");            
        }
        
        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate, string collectionName)
        {
            throw new NotImplementedException();
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

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

       
    }
  
}
