﻿using System.Data.SqlClient;

namespace AADWebApp.Services
{
    public interface IDatabaseService
    {
        public bool IsInitialised { get; }

        public void ConnectToMSSQLServer();
        public int ExecuteNonQuery(string NonQuery);
        public SqlDataReader ExecuteQuery(string Query);
        public SqlDataReader RetrieveTable(string TableName);
    }
}
