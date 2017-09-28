using System.Data.SqlClient;

namespace ColoursTest.Data.Factories
{
    public class ConnectionFactory
    {
        public static string ConnectionString { get; } = "data source=localhost;initial catalog=TechTest;integrated security=true;";

        public SqlConnection GetNewConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}