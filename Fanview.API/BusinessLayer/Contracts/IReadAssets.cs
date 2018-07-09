using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface IReadAssets 
    {
        string GetDamageCauserName(string damageCauserKey);
    }
}
