using System;
using System.Data.SqlClient;

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
            if (Initialised == false)
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
    }
}
