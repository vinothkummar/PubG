using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Repository.Interface
{
    public interface ILiveRepository
    { 
        Task<LiveDamageList> GetLiveDamageList();
        void DeleteAllEventKillTable();
        void DeleteAllTeamStates();
        void DeleteEventMatchStates();
        void DeleteEventLiveMatchDamage();
        void DeleteAll();
    }
}
