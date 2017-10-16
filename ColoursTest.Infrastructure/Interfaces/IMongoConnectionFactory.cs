using MongoDB.Driver;

namespace ColoursTest.Infrastructure.Interfaces
{
    public interface IMongoConnectionFactory
    {
        IMongoDatabase GetDatabase();
    }
}