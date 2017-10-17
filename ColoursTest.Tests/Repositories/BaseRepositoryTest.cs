using System;
using System.Data.SqlClient;

namespace ColoursTest.Tests.Repositories
{
    public class BaseRepositoryTest<T> : IDisposable
    {
        protected BaseRepositoryTest()
        {
            this.DatabaseName = $"{typeof(T).Name}DB";
            DatabaseSetup.Setup(this.DatabaseName);
        }

        private string DatabaseName { get; }

        public SqlConnection Connection => new SqlConnection($"data source=localhost;initial catalog={this.DatabaseName};integrated security=true;");

        public void Dispose()
        {
            DatabaseSetup.TearDown(this.DatabaseName);
        }
    }
}