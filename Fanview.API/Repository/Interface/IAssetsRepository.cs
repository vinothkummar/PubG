using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository.Interface
{
    public interface IAssetsRepository
    {
        string GetDamageCauserName(string damageCauserKey);
    }
}
