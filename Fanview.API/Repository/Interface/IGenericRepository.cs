using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Fanview.API.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string collectionName);
        Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate, string collectionName);
        void Insert(IEnumerable<T> entity, string collectionName);
        //void Insert(IAsyncEnumerable<T> entity, string collectionName);
        void Insert(T entity, string collectionName);
        void Delete(T entity);
        void Update(T entity);
        void Save();
    }
}
