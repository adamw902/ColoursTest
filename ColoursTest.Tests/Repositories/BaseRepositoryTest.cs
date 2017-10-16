using System;
using MongoDB.Driver;

namespace ColoursTest.Tests.Repositories
{
    public class BaseRepositoryTest<T> : IDisposable where T : class
    {
        protected BaseRepositoryTest()
        {
            this.DatabaseName = $"{typeof(T).Name}DB";
            DatabaseSetup.Setup(this.DatabaseName);
        }

        private string DatabaseName { get; }

        public IMongoDatabase Database => new MongoClient("mongodb://localhost:27017").GetDatabase(this.DatabaseName);

        public void Dispose()
        {
            DatabaseSetup.TearDown(this.DatabaseName);
        }
    }
}