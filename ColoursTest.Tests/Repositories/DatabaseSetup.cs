using System.Data.SqlClient;
using System.IO;

namespace ColoursTest.Tests.Repositories
{
    public static class DatabaseSetup
    {
        private const string ConnectionString = "data source=localhost;initial catalog=master;integrated security=true;";

        public static void Setup(string databaseName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var createDatabase = File.ReadAllText("Repositories/DBScripts/CreateDatabase.sql");
                var createTables = File.ReadAllText("Repositories/DBScripts/CreateTables.sql");
                var insertMockData = File.ReadAllText("Repositories/DBScripts/InsertMockData.sql");

                var createDatabaseCommand = connection.CreateCommand();
                createDatabase = createDatabase.Replace("@DatabaseName", $"'{databaseName}'");
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

        public static void TearDown(string databaseName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var dropDatabaseCommand = connection.CreateCommand();
                dropDatabaseCommand.CommandText = 
                    $@"IF EXISTS(SELECT name FROM dbo.sysdatabases WHERE name = '{databaseName}')
                       BEGIN
                           EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = '{databaseName}'
                           ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                           DROP DATABASE[{databaseName}]
                       END";

                connection.Open();
                dropDatabaseCommand.ExecuteNonQuery();
            }
        }
    }
}