using System.Data.Common;
using System.Diagnostics;
using AADWebApp.Interfaces;
using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AADWebApp.Services.DatabaseService;

namespace AADWebAppTests.Services
{
    [TestClass]
    public class DatabaseServiceTests : TestBase
    {
        private readonly IDatabaseService _databaseService;

        public DatabaseServiceTests()
        {
            _databaseService = Get<IDatabaseService>();
        }

        [TestMethod]
        public void ConnectToMssqlServerTest()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);
        }

        [TestMethod]
        public void ExecuteQueryTest()
        {
            ConnectToMssqlServerTest();
            var query = "SELECT * FROM TestData";
            var dataReader = _databaseService.ExecuteQuery(query);
            PrintDataReaderToConsole(dataReader);
        }

        [TestMethod]
        public void ExecuteNonQueryTest()
        {
            ConnectToMssqlServerTest();
            var nonQuery = "UPDATE TestData SET Value = 'ValueUpdated' WHERE Name = 'UpdateMe'";
            Debug.WriteLine(_databaseService.ExecuteNonQuery(nonQuery));
        }

        [TestMethod]
        public void RetrieveTableTest()
        {
            ConnectToMssqlServerTest();
            var dataReader = _databaseService.RetrieveTable("TestData");
            PrintDataReaderToConsole(dataReader);
        }

        [TestMethod]
        public void QueryResultTest()
        {
            var testResult = ConstructQueryResultUsingTestData();
        }

        [TestMethod]
        public void GetCellTest()
        {
            var expectedName = "Location";
            var expectedValue = "Nottingham";
            var testResult = ConstructQueryResultUsingTestData();
            var result1 = testResult.GetCell(3, 0);
            var result2 = testResult.GetCell(3, "Value");

            Assert.AreEqual(expectedName, result1.ToString());
            Assert.AreEqual(expectedValue, result2.ToString());
        }

        [TestMethod]
        public void GetColumnTest()
        {
            string[] expectedValues =
            {
                "1",
                "2",
                "3",
                "Nottingham",
                "SQLSERVER",
                "ValueUpdated"
            };
            var testResult = ConstructQueryResultUsingTestData();
            var testColumnResult = testResult.GetColumn(1);
            var testColumnResult2 = testResult.GetColumn("Value");

            Assert.AreEqual("Value", testColumnResult.ColumnName);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.AreEqual(expectedValues[i], testColumnResult.Items[i]);
                Assert.AreEqual(expectedValues[i], testColumnResult2.Items[i]);
            }
        }

        [TestMethod]
        public void GetRowTest()
        {
            var expectedName = "Test1";
            var expectedValue = "1";
            var testResult = ConstructQueryResultUsingTestData();
            var rowResult = testResult.GetRow(0);

            Assert.AreEqual(expectedName, rowResult.Items[0]);
            Assert.AreEqual(expectedValue, rowResult.Items[1]);
        }

        private QueryResult ConstructQueryResultUsingTestData()
        {
            ConnectToMssqlServerTest();
            var query = "SELECT * FROM TestData";
            var dataReader = _databaseService.ExecuteQuery(query);
            return new QueryResult(dataReader);
        }

        private static void PrintDataReaderToConsole(DbDataReader reader)
        {
            while (reader.Read())
            {
                var output = "";
                for (var i = 0; i < reader.VisibleFieldCount; i++)
                {
                    if (i != 0)
                    {
                        output += ", ";
                    }

                    output += reader.GetValue(i);
                }

                Debug.WriteLine(output);
            }
        }
    }
}