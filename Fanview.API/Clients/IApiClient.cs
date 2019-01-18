using System.Threading.Tasks;

namespace Fanview.API.Clients
{
    public interface IApiClient
    {
        Task<string> GetMatchSummary(string matchId);
    }
}