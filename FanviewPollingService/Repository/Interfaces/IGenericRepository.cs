using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FanviewPollingService.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate);
        void Insert(IEnumerable<T> entity, string collectionName);
        void Delete(T entity);
        void Update(T entity);
        void Save();
    }
}
