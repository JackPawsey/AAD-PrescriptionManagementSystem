﻿using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Diagnostics;
using static AADWebApp.Services.DatabaseService;

namespace AADWebAppTests.Services
{
    [TestClass()]
    public class DatabaseServiceTests
    {
        string Server = "cloud-crusaders-project-database-mssql.c8ratiay2jmd.eu-west-2.rds.amazonaws.com";
        string Username = "admin";
        //To-Do: Figure out secrets, change password.
        string Password = "uPjz58%4";

        DatabaseService TestService;

        private DatabaseService CreateService()
        {
            return new DatabaseService(Server, Username, Password);
        }

        [TestMethod()]
        public void DatabaseServiceTest()
        {
            CreateService();
        }

        [TestMethod()]
        public void ConnectToMSSQLServerTest()
        {
            TestService = CreateService();
            TestService.ConnectToMSSQLServer(Databases.program_data);
        }

        [TestMethod()]
        public void ExecuteQueryTest()
        {
            ConnectToMSSQLServerTest();
            string Query = "SELECT TOP(1) * FROM TestData";
            SqlDataReader DataReader = TestService.ExecuteQuery(Query);
            PrintDataReaderToConsole(DataReader);
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            ConnectToMSSQLServerTest();
            string NonQuery = "UPDATE TestData SET Value = 'ValueUpdated' WHERE Name = 'UpdateMe'";
            Debug.WriteLine(TestService.ExecuteNonQuery(NonQuery));
        }

        [TestMethod()]
        public void RetrieveTableTest()
        {
            ConnectToMSSQLServerTest();
            SqlDataReader DataReader = TestService.RetrieveTable("TestData");
            PrintDataReaderToConsole(DataReader);
        }

        private void PrintDataReaderToConsole(SqlDataReader Reader)
        {
            while (Reader.Read())
            {
                string output = "";
                for (int i = 0; i < Reader.VisibleFieldCount; i++)
                {
                    output += Reader.GetString(i) + ", ";
                }

                Debug.WriteLine(output);
            }
        }
    }
}