using MongoDB.Driver;

namespace Fanview.API.Clients
{
    public interface IMongoDbClient
    {
        IMongoDatabase Database { get; }
    }
}