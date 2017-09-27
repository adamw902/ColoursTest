using System.Data.SqlClient;

namespace ColoursTest.Data
{
    public class DatabaseConnection
    {
        public SqlConnection SqlConnection = new SqlConnection("data source=localhost;initial catalog=TechTest;integrated security=true;");
    }
}