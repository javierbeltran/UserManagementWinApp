using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserManagement.Library.Database
{
    public class DBBoostrap : IDBBoostrap
    {
        private readonly DBConfiguration dbConfig;

        public DBBoostrap(DBConfiguration databaseConfig)
        {
            this.dbConfig = databaseConfig;
        }
        public void Initialize()
        {
            StringBuilder query = new StringBuilder();
            using (var connection = new SqliteConnection(dbConfig.Name))
            {
                //Try to get the table name
                //A table name empty means that the table does not exist
                var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'User';");
                var tableName = table.FirstOrDefault();

                if (!string.IsNullOrEmpty(tableName) && tableName == "User")
                    return;

                connection.Execute("Create Table Product (" +
                    "Name VARCHAR(100) NOT NULL," +
                    "Description VARCHAR(1000) NULL);");

                query.AppendLine("CREATE TABLE User");
                query.AppendLine("(Id INTEGER PRIMARY KEY AUTOINCREMENT,");
                query.AppendLine("Name VARCHAR(50) NOT NULL, LastName VARCHAR(60) NOT NULL,");
                query.AppendLine("Email VARCHAR(255) NOT NULL)");

                connection.Execute(query.ToString());
            }
        }
    }
}
