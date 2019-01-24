using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Repository.Interface
{
    public interface ILiveRepository
    { 
        Task<LiveDamageList> GetLiveDamageList();
    }
}
