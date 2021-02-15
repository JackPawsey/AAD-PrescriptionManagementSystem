using System.Data.SqlClient;
using static AADWebApp.Services.DatabaseService;

namespace AADWebApp.Services
{
    public interface IDatabaseService
    {
        public bool IsInitialised { get; }

        public void ConnectToMSSQLServer(AvailableDatabases DatabaseName);
        public void ChangeDatabase(AvailableDatabases DatabaseName);
        public int ExecuteNonQuery(string NonQuery);
        public SqlDataReader ExecuteQuery(string Query);
        public SqlDataReader RetrieveTable(string TableName);
        public void CloseConnection();
    }
}
