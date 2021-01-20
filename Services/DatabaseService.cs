using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AADWebApp.Services
{
    public class DatabaseService
    {
        public string ServerName;
        public string Username;
        public string Password;
        public string DatabaseName;

        private bool Initialised = false;
        public bool IsInitialised => Initialised;

        private MySqlConnection DBConnection { get; set; }

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
        /// Open a connection to a MySQL database using the class variables.
        /// </summary>
        public void ConnectToMySQLDB()
        {
            Initialised = false;
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(DatabaseName))
            {
                throw new InvalidOperationException("ServerName, Username, Password or DatabaseName is NullOrEmpty.");
            }

            MySqlConnectionStringBuilder ConnectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = ServerName,
                UserID = Username,
                Password = Password
            };
            string ConnectionString = ConnectionStringBuilder.ToString();
            DBConnection = new MySqlConnection(ConnectionString);

            try
            {
                DBConnection.Open();
                Initialised = true;
                DBConnection.ChangeDatabase(DatabaseName);
            }
            catch (MySqlException Ex)
            {
                Exception CustomException = new Exception("Connection to MySQL Server failed. See inner exception and ensure arguments are correct.", Ex);
                CustomException.Data.Add("Connection String", ConnectionString);
                throw CustomException;
            }
        }

        /// <summary>
        /// Execute a query (a MySQL command that returns rows) against the database connection.
        /// </summary>
        /// <param name="Query">The MySQL command.</param>
        /// <returns>Returns a MySqlDataReader object containing the results of the query.</returns>
        public MySqlDataReader ExecuteQuery(string Query)
        {
            CheckInitialised();
            MySqlCommand Command = new MySqlCommand(Query, DBConnection);
            MySqlDataReader Reader = Command.ExecuteReader();
            return Reader;
        }

        /// <summary>
        /// Execute a non-query (a MySQL command that returns no rows) against the database connection.
        /// </summary>
        /// <param name="NonQuery">The MySql command.</param>
        /// <returns>Returns the number of rows affected as an int.</returns>
        public int ExecuteNonQuery(string NonQuery)
        {
            CheckInitialised();
            MySqlCommand Command = new MySqlCommand(NonQuery, DBConnection);
            return Command.ExecuteNonQuery();
        }

        /// <summary>
        /// Performs "SELECT * FROM {TableName}" against the database connection.
        /// </summary>
        /// <param name="TableName">The name of the table to query.</param>
        /// <returns>Returns a MySqlDataReader object containing the results of the query.</returns>
        public MySqlDataReader RetrieveTable(string TableName)
        {
            CheckInitialised();
            MySqlCommand SelectTableCommand = new MySqlCommand($"SELECT * FROM {TableName};", DBConnection);
            return SelectTableCommand.ExecuteReader();
        }
    }
}
