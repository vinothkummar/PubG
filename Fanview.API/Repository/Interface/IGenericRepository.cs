using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Fanview.API.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IMongoCollection<T> GetMongoDbCollection(string collectionName);
        Task<IEnumerable<T>> GetAll(string collectionName);
        void Insert(IEnumerable<T> entity, string collectionName);
        void Insert(T entity, string collectionName);
        Task<DeleteResult> DeleteOne(FilterDefinition<T> filter, string collectionName);
        Task<DeleteResult> DeleteMany(FilterDefinition<T> filter, string collectionName);
        void Update(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update);
        void Save();
        void Replace(T entity, FilterDefinition<T> filter, string collectionName);
    }
}
