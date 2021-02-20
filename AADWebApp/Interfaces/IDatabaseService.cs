using System.Data.SqlClient;
using static AADWebApp.Services.DatabaseService;

namespace AADWebApp.Interfaces
{
    public interface IDatabaseService
    {
        public bool IsInitialised { get; }

        public void ConnectToMssqlServer(AvailableDatabases databaseName);
        public void ChangeDatabase(AvailableDatabases databaseName);
        public int ExecuteNonQuery(string nonQuery);
        public SqlDataReader ExecuteQuery(string query);
        public SqlDataReader RetrieveTable(string tableName, string whereColumn = null, object whereValue = null);
        public void CloseConnection();
    }
}