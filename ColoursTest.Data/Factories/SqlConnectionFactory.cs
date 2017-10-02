using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ColoursTest.Infrastructure.Interfaces;

namespace ColoursTest.Infrastructure.Factories
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        public SqlConnectionFactory(IConfiguration configuration)
        {
            this.TechTestConnectionString = configuration.GetConnectionString("TechTest");
        }

        public string TechTestConnectionString { get; }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(TechTestConnectionString);
        }
    }
}