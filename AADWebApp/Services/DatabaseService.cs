using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AADWebAppTests.Services")]
namespace AADWebApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        public string ServerName;
        public string Username;
        public string Password;
        public string DatabaseName;

        private bool Initialised = false;
        public bool IsInitialised => Initialised;

        private SqlConnection DBConnection { get; set; }

        /// <summary>
        /// Construct a DatabaseService object.
        /// </summary>
        /// <param name="ServerName">The IP address/endpoint of the server.</param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        public DatabaseService(string ServerName, string Username, string Password, string DatabaseName)
        {
            this.ServerName = ServerName;
            this.Username = Username;
            this.Password = Password;
            this.DatabaseName = DatabaseName;
        }

        private void CheckInitialised()
        {
            if (!Initialised)
            {
                throw new InvalidOperationException("Connection has not been initialised.");
            }
        }

        /// <summary>
        /// Open a connection to a MSSQLServer database using the class variables.
        /// </summary>
        public void ConnectToMSSQLServer()
        {
            Initialised = false;
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(DatabaseName))
            {
                throw new InvalidOperationException("ServerName, Username, Password or DatabaseName is NullOrEmpty.");
            }

            SqlConnectionStringBuilder ConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = ServerName,
                UserID = Username,
                Password = Password,
                InitialCatalog = DatabaseName
            };
            string ConnectionString = ConnectionStringBuilder.ToString();
            DBConnection = new SqlConnection(ConnectionString);

            try
            {
                DBConnection.Open();
                Initialised = true;
            }
            catch (SqlException Ex)
            {
                Exception CustomException = new Exception("Connection to MSSQLServer failed. See inner exception and ensure arguments are correct.", Ex);
                CustomException.Data.Add("Connection String", ConnectionString);
                throw CustomException;
            }
        }

        /// <summary>
        /// Changes the catalog (database) currently selected for commands.
        /// </summary>
        /// <param name="DatabaseName">The name of the target database.</param>
        public void ChangeDatabase(string DatabaseName)
        {
            DBConnection.ChangeDatabase(DatabaseName);
        }

        /// <summary>
        /// Execute a query (a T-SQL command that returns rows) against the database connection.
        /// </summary>
        /// <param name="Query">The T-SQL command.</param>
        /// <returns>Returns a SqlDataReader object containing the results of the query.</returns>
        public SqlDataReader ExecuteQuery(string Query)
        {
            CheckInitialised();
            SqlCommand Command = new SqlCommand(Query, DBConnection);
            SqlDataReader Reader = Command.ExecuteReader();
            return Reader;
        }

        /// <summary>
        /// Execute a non-query (a T-SQL command that returns no rows) against the database connection.
        /// </summary>
        /// <param name="NonQuery">The T-SQL command.</param>
        /// <returns>Returns the number of rows affected as an int.</returns>
        public int ExecuteNonQuery(string NonQuery)
        {
            CheckInitialised();
            SqlCommand Command = new SqlCommand(NonQuery, DBConnection);
            return Command.ExecuteNonQuery();
        }

        /// <summary>
        /// Performs "SELECT * FROM {TableName}" against the database connection.
        /// </summary>
        /// <param name="TableName">The name of the table to query.</param>
        /// <returns>Returns a SqlDataReader object containing the results of the query.</returns>
        public SqlDataReader RetrieveTable(string TableName)
        {
            CheckInitialised();
            SqlCommand SelectTableCommand = new SqlCommand($"SELECT * FROM {TableName};", DBConnection);
            return SelectTableCommand.ExecuteReader();
        }

        ~DatabaseService()
        {
            DBConnection.Close();
        }
    }

    public class ColumnResult
    {
        public string ColumnName;
        public List<object> Items;

        public ColumnResult()
        {
            ColumnName = string.Empty;
            Items = new List<object>();
        }
    }

    public class RowResult
    {
        public List<object> Items;

        public RowResult()
        {
            Items = new List<object>();
        }
    }

    public class QueryResult
    {
        private DataTable SourceTable;
        private SqlDataReader SourceReader;

        public QueryResult(SqlDataReader SourceReader)
        {
            this.SourceReader = SourceReader;
            SourceTable = new DataTable();
            SourceTable.Load(this.SourceReader);
        }

        

        /// <summary>
        /// Gets the contents of a cell from the query.
        /// </summary>
        /// <param name="RowIndex">The index of the row.</param>
        /// <param name="ColumnIndex">The index of the column.</param>
        /// <returns>Returns the contents of the specified cell. The result will be converted to the type specified for it by the SqlDataReader passed into the class.</returns>
        public object GetCell(int RowIndex, int ColumnIndex)
        {
            string CellData = SourceTable.Rows[RowIndex][ColumnIndex].ToString();
            return CellData;
        }

        /// <summary>
        /// Gets the contents of a cell from the query.
        /// </summary>
        /// <param name="RowIndex">The index of the row.</param>
        /// <param name="ColumnName">The name of the column.</param>
        /// <returns>Returns the contents of the specified cell. The result will be converted to the type specified for it by the SqlDataReader passed into the class.</returns>
        public object GetCell(int RowIndex, string ColumnName)
        {
            int ColumnIndex = ConvertColumnNameToIndex(ColumnName);
            return GetCell(RowIndex, ColumnIndex);
        }

        /// <summary>
        /// Gets the contents of an entire column from the query.
        /// </summary>
        /// <param name="ColumnIndex">The index of the column.</param>
        /// <returns>Returns a ColumnResult containing the cells of the column.</returns>
        public ColumnResult GetColumn(int ColumnIndex)
        {
            ColumnResult Output = new ColumnResult();
            Output.ColumnName = SourceTable.Columns[ColumnIndex].ColumnName;
            var RowCount = SourceTable.Rows.Count;
            for (int i = 0; i < RowCount; i++)
            {
                Output.Items.Add(GetCell(i, ColumnIndex));
            }

            return Output;
        }

        /// <summary>
        /// Gets the contents of an entire column from the query.
        /// </summary>
        /// <param name="ColumnName">The name of the column.</param>
        /// <returns>Returns a ColumnResult containing the cells of the column.</returns>
        public ColumnResult GetColumn(string ColumnName)
        {
            int ColumnIndex = ConvertColumnNameToIndex(ColumnName);
            return GetColumn(ColumnIndex);
        }

        /// <summary>
        /// Gets the contents of an entire row from the query.
        /// </summary>
        /// <param name="RowIndex">The index of the row.</param>
        /// <returns>Returns a RowResult containing the cells of the row.</returns>
        public RowResult GetRow(int RowIndex)
        {
            RowResult Output = new RowResult();
            var Row = SourceTable.Rows[RowIndex];
            Output.Items = Row.ItemArray.ToList();
            return Output;
        }

        private int ConvertColumnNameToIndex(string ColumnName)
        {
            DataColumnCollection Columns = SourceTable.Columns;
            return Columns.IndexOf(ColumnName);
        }
    }
}
