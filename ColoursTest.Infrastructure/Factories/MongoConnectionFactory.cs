using ColoursTest.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ColoursTest.Infrastructure.Factories
{
    public class MongoConnectionFactory : IMongoConnectionFactory
    {
        public MongoConnectionFactory(IConfiguration config)
        {
            this.ConnectionString = config.GetSection("MongoConnection:ConnectionString").Value;
            this.Database = config.GetSection("MongoConnection:Database").Value;
        }
        
        private string ConnectionString { get; }
        private string Database { get; }
        private MongoClient Client => new MongoClient(this.ConnectionString);

        public IMongoDatabase GetDatabase()
        {
            return this.Client.GetDatabase(this.Database);
        }
    }
}