﻿using System;
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
        public enum AvailableDatabases
        {
            Identity,
            program_data
        }

        private readonly string _password;
        private readonly string _serverName;
        private readonly string _username;

        public bool IsInitialised { get; private set; }

        private SqlConnection DbConnection { get; set; }

        /// <summary>
        ///     Construct a DatabaseService object.
        /// </summary>
        /// <param name="serverName">The IP address/endpoint of the server.</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public DatabaseService(string serverName, string username, string password)
        {
            _serverName = serverName;
            _username = username;
            _password = password;
        }

        private void CheckInitialised()
        {
            if (!IsInitialised)
            {
                throw new InvalidOperationException("Connection has not been initialised.");
            }
        }

        /// <summary>
        ///     Open a connection to a MSSQLServer database using the class variables.
        /// </summary>
        public void ConnectToMssqlServer(AvailableDatabases databaseName)
        {
            IsInitialised = false;
            
            if (string.IsNullOrEmpty(_serverName) || string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            {
                throw new InvalidOperationException("ServerName, Username, Password or DatabaseName is NullOrEmpty.");
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = _serverName,
                UserID = _username,
                Password = _password,
                InitialCatalog = databaseName.ToString()
            };

            var connectionString = connectionStringBuilder.ToString();
            DbConnection = new SqlConnection(connectionString);

            try
            {
                DbConnection.Open();
                IsInitialised = true;
            }
            catch (SqlException ex)
            {
                var customException = new Exception("Connection to MSSQLServer failed. See inner exception and ensure arguments are correct.", ex);
                customException.Data.Add("Connection String", connectionString);
                throw customException;
            }
        }

        /// <summary>
        ///     Changes the catalog (database) currently selected for commands.
        /// </summary>
        /// <param name="databaseName">The name of the target database.</param>
        public void ChangeDatabase(AvailableDatabases databaseName)
        {
            DbConnection.ChangeDatabase(databaseName.ToString());
        }

        /// <summary>
        ///     Execute a query (a T-SQL command that returns rows) against the database connection.
        /// </summary>
        /// <param name="query">The T-SQL command.</param>
        /// <returns>Returns a SqlDataReader object containing the results of the query.</returns>
        public SqlDataReader ExecuteQuery(string query)
        {
            CheckInitialised();
            var command = new SqlCommand(query, DbConnection);
            var reader = command.ExecuteReader();
            return reader;
        }

        /// <summary>
        ///     Execute a non-query (a T-SQL command that returns no rows) against the database connection.
        /// </summary>
        /// <param name="nonQuery">The T-SQL command.</param>
        /// <returns>Returns the number of rows affected as an int.</returns>
        public int ExecuteNonQuery(string nonQuery)
        {
            CheckInitialised();
            var command = new SqlCommand(nonQuery, DbConnection);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Performs "SELECT * FROM {TableName}" against the database connection.
        /// </summary>
        /// <param name="tableName">The name of the table to query.</param>
        /// <returns>Returns a SqlDataReader object containing the results of the query.</returns>
        public SqlDataReader RetrieveTable(string tableName)
        {
            CheckInitialised();
            var selectTableCommand = new SqlCommand($"SELECT * FROM {tableName};", DbConnection);
            return selectTableCommand.ExecuteReader();
        }

        /// <summary>
        ///     Close the DBConnection.
        /// </summary>
        public void CloseConnection()
        {
            DbConnection.Close();
        }

        ~DatabaseService()
        {
            DbConnection.Close();
        }
    }

    public class ColumnResult
    {
        public string ColumnName;
        public readonly List<object> Items;

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
        private readonly DataTable _sourceTable;

        public QueryResult(IDataReader sourceReader)
        {
            _sourceTable = new DataTable();
            _sourceTable.Load(sourceReader);
        }


        /// <summary>
        ///     Gets the contents of a cell from the query.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="columnIndex">The index of the column.</param>
        /// <returns>
        ///     Returns the contents of the specified cell. The result will be converted to the type specified for it by the
        ///     SqlDataReader passed into the class.
        /// </returns>
        public object GetCell(int rowIndex, int columnIndex)
        {
            var cellData = _sourceTable.Rows[rowIndex][columnIndex].ToString();
            return cellData;
        }

        /// <summary>
        ///     Gets the contents of a cell from the query.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>
        ///     Returns the contents of the specified cell. The result will be converted to the type specified for it by the
        ///     SqlDataReader passed into the class.
        /// </returns>
        public object GetCell(int rowIndex, string columnName)
        {
            var columnIndex = ConvertColumnNameToIndex(columnName);
            return GetCell(rowIndex, columnIndex);
        }

        /// <summary>
        ///     Gets the contents of an entire column from the query.
        /// </summary>
        /// <param name="columnIndex">The index of the column.</param>
        /// <returns>Returns a ColumnResult containing the cells of the column.</returns>
        public ColumnResult GetColumn(int columnIndex)
        {
            var output = new ColumnResult
            {
                ColumnName = _sourceTable.Columns[columnIndex].ColumnName
            };

            var rowCount = _sourceTable.Rows.Count;
            for (var i = 0; i < rowCount; i++)
            {
                output.Items.Add(GetCell(i, columnIndex));
            }

            return output;
        }

        /// <summary>
        ///     Gets the contents of an entire column from the query.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>Returns a ColumnResult containing the cells of the column.</returns>
        public ColumnResult GetColumn(string columnName)
        {
            var columnIndex = ConvertColumnNameToIndex(columnName);
            return GetColumn(columnIndex);
        }

        /// <summary>
        ///     Gets the contents of an entire row from the query.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <returns>Returns a RowResult containing the cells of the row.</returns>
        public RowResult GetRow(int rowIndex)
        {
            var output = new RowResult();
            var row = _sourceTable.Rows[rowIndex];
            output.Items = row.ItemArray.ToList();
            return output;
        }

        private int ConvertColumnNameToIndex(string columnName)
        {
            var columns = _sourceTable.Columns;
            return columns.IndexOf(columnName);
        }
    }
}