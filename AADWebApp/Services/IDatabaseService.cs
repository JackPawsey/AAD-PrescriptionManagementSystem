using System.Data.SqlClient;

namespace AADWebApp.Services
{
    public interface IDatabaseService
    {
        bool IsInitialised { get; }

        void ConnectToMSSQLServer();
        int ExecuteNonQuery(string NonQuery);
        SqlDataReader ExecuteQuery(string Query);
        SqlDataReader RetrieveTable(string TableName);
    }
}