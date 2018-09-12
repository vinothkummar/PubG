using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;

namespace Fanview.API.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public IEnumerable<object> GetMatchDetails()
        {
            throw new NotImplementedException();
        }
    }
}
