using System;
using System.Data.SqlClient;

namespace ColoursTest.Tests.Repositories
{
    public class BaseRepositoryTest : IDisposable
    {
        protected BaseRepositoryTest(string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName), "Please provide a database name");
            }
            this.DatabaseName = databaseName;
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