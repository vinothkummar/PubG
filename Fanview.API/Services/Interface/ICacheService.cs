using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Services.Interface
{
    public interface ICacheService
    {
        Task SaveToCache<T>(string key, T item, int absoluteExpirationRelativeToNow, int slidingExpiration);
        Task<T> RetrieveFromCache<T>(string key);

        //Task<T> RefreshFromCache<T>(string key);
        //Task<T> RemoveFromCache<T>(string key);
    }
}
