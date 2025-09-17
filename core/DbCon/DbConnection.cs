using System;
using System.Data;
using System.Data.SqlClient;

namespace PersonApp.core.DbCon
{
    public class DbConnection
    {
     private readonly string masterConnectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

        
        private readonly string databaseName = "PersonDB";
        private readonly string personTableName = "Persons";

        public string ConnectionString => $"Server=localhost\\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;";


        public DbConnection()
        {
            EnsureDatabase();
            EnsureTable();
        }

        // Ensure database exists, if not create it
        private void EnsureDatabase()
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                var cmdText = $@"
                IF DB_ID('{databaseName}') IS NULL
                    CREATE DATABASE [{databaseName}];";

                using (var cmd = new SqlCommand(cmdText, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Ensure table exists, if not create it
        private void EnsureTable()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = $@"
                IF OBJECT_ID('{personTableName}', 'U') IS NULL
                CREATE TABLE {personTableName} (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    FullName NVARCHAR(100) NOT NULL,
                    Address NVARCHAR(200),
                    Email NVARCHAR(100) NOT NULL,
                    Phone NVARCHAR(20) NOT NULL
                );";

                using (var cmd = new SqlCommand(cmdText, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Helper method to create SqlCommand with parameters
        public SqlCommand CreateCommand(string query, SqlConnection connection, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand(query, connection);
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);
            return cmd;
        }
    }
}
