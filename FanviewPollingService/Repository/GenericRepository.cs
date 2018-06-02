using FanviewPollingService.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using Microsoft.Extensions.Configuration;
using FanviewPollingService.Model;
using System.Threading.Tasks;
using System.Net.Http;

namespace FanviewPollingService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private IConfigurationRoot _configuration;
        private IMongoCollection<T> collection;
        private IMongoDatabase database;

        public GenericRepository()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            var client = new MongoClient(this._configuration["Logging:AppSettings:ConnectionString"]);
            database = client.GetDatabase("FanviewPubGDb");
            
        }


        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async void Insert(IEnumerable<T> entity, string collectionName)
        {

           collection = database.GetCollection<T>(collectionName);
           await collection.InsertManyAsync(entity);
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
