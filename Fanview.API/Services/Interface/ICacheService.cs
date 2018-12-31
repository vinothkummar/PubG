using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Services.Interface
{
    public interface ICacheService
    {
        Task SaveToCache<T>(string key, T item, int absoluteExpirationRelativeToNow, int slidingExpiration);
        T RetrieveFromCache<T>(string key);
        Task SaveObjToCache<T>(string key, T item, int absoluteExpirationRelativeToNow, int slidingExpiration);
        Task<T> RetrieveObjFromCache<T>(string key) where T : class;
        void RefreshFromCache();
        void RemoveFromCache();
    }
}
