using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
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
            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }
    }
}
