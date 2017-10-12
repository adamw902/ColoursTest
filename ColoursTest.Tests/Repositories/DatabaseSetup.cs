using System.Data.SqlClient;
using System.IO;

namespace ColoursTest.Tests.Repositories
{
    public static class DatabaseSetup
    {
        public static void Setup(string databaseName)
        {
            using (var connection = new SqlConnection("data source=localhost;initial catalog=master;integrated security=true;"))
            {
                var createDatabase = new FileInfo("Repositories/DBScripts/CreateDatabase.sql").OpenText().ReadToEnd();
                var createTables = new FileInfo("Repositories/DBScripts/CreateTables.sql").OpenText().ReadToEnd();
                var insertMockData = new FileInfo("Repositories/DBScripts/InsertMockData.sql").OpenText().ReadToEnd();

                var createDatabaseCommand = connection.CreateCommand();
                createDatabase = createDatabase.Replace("@DatabaseName", $"\'{databaseName}\'");
                createDatabaseCommand.CommandText = createDatabase.Replace("[@DBName]", $"[{databaseName}]");

                var createTablesCommand = connection.CreateCommand();
                createTablesCommand.CommandText = createTables.Replace("@DBName", $"{databaseName}");

                var insertMockDataCommand = connection.CreateCommand();
                insertMockDataCommand.CommandText = insertMockData.Replace("@DBName", $"{databaseName}");

                connection.Open();
                createDatabaseCommand.ExecuteNonQuery();
                createTablesCommand.ExecuteNonQuery();
                insertMockDataCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}