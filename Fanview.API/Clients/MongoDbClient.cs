using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Fanview.API.Clients
{
    public class MongoDbClient : IMongoDbClient
    {
        public IMongoDatabase Database { get; private set; }

        public MongoDbClient(IConfiguration config)
        {
            var client = new MongoClient(config["Database:ConnectionString"]);
            Database = client.GetDatabase(config["Database:Name"]);
        }
    }
}
