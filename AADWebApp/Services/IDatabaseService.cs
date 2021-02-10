using System.Data.SqlClient;
using static AADWebApp.Services.DatabaseService;

namespace AADWebApp.Services
{
    public interface IDatabaseService
    {
        public bool IsInitialised { get; }

        public void ConnectToMSSQLServer(Databases DatabaseName);
        public int ExecuteNonQuery(string NonQuery);
        public SqlDataReader ExecuteQuery(string Query);
        public SqlDataReader RetrieveTable(string TableName);
    }
}
