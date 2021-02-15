using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
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
            TestService.ConnectToMSSQLServer(AvailableDatabases.program_data);
        }

        [TestMethod()]
        public void ExecuteQueryTest()
        {
            ConnectToMSSQLServerTest();
            string Query = "SELECT * FROM TestData";
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

        [TestMethod()]
        public void QueryResultTest()
        {
            QueryResult TestResult = ConstructQueryResultUsingTestData();
        }

        [TestMethod()]
        public void GetCellTest()
        {
            const string ExpectedName = "Location";
            const string ExpectedValue = "Nottingham";
            QueryResult TestResult = ConstructQueryResultUsingTestData();
            var Result1 = TestResult.GetCell(3, 0);
            var Result2 = TestResult.GetCell(3, "Value");

            Assert.AreEqual(Result1.ToString(), ExpectedName);
            Assert.AreEqual(Result2.ToString(), ExpectedValue);
        }

        [TestMethod()]
        public void GetColumnTest()
        {
            string[] ExpectedValues = { "1", "2", "3", "Nottingham", "SQLSERVER", "ValueUpdated"};
            QueryResult TestResult = ConstructQueryResultUsingTestData();
            ColumnResult TestColumnResult = TestResult.GetColumn(1);
            ColumnResult TestColumnResult2 = TestResult.GetColumn("Value");

            Assert.AreEqual("Value", TestColumnResult.ColumnName);

            for (int i = 0; i < ExpectedValues.Length; i++)
            {
                Assert.AreEqual(ExpectedValues[i], TestColumnResult.Items[i]);
                Assert.AreEqual(ExpectedValues[i], TestColumnResult2.Items[i]);
            }
        }

        [TestMethod()]
        public void GetRowTest()
        {
            string ExpectedName = "Test1";
            string ExpectedValue = "1";
            QueryResult TestResult = ConstructQueryResultUsingTestData();
            RowResult RowResult = TestResult.GetRow(0);

            Assert.AreEqual(ExpectedName, RowResult.Items[0]);
            Assert.AreEqual(ExpectedValue, RowResult.Items[1]);
        }

        private QueryResult ConstructQueryResultUsingTestData()
        {
            ConnectToMSSQLServerTest();
            string Query = "SELECT * FROM TestData";
            SqlDataReader DataReader = TestService.ExecuteQuery(Query);
            return new QueryResult(DataReader);
        }

        private void PrintDataReaderToConsole(SqlDataReader Reader)
        {
            while (Reader.Read())
            {
                string output = "";
                for (int i = 0; i < Reader.VisibleFieldCount; i++)
                {
                    if (i != 0)
                    {
                        output += ", ";
                    }
                    output += Reader.GetValue(i);
                }

                Debug.WriteLine(output);
            }
        }
    }
}