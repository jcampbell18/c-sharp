using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace campbelljmidterm
{
    /// <summary>
    /// Creates or opens the database, and starts/stops the connection along with other actions
    /// </summary>
    class DatabaseConnection
    {
        private readonly SQLiteConnection sqlite_conn;
        private SQLiteCommand sqlite_cmd;
        private SQLiteDataReader sqlite_datareader;

        /// <summary>
        /// Constructor that starts the connection, and creates/opens the database
        /// </summary>
        public DatabaseConnection()
        {
            try
            {
                sqlite_conn = new SQLiteConnection("Data Source=filewatcher.db;Version=3;New=True;Compress=True;");
                sqlite_conn.Open();
                CreateTable();
            }
            catch (SQLiteException e)
            {
                MessageBox.Show("Unable to connect to Database\nDetails: " + e, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Creates a table:
        ///     log_id: unique id that auto-increment
        ///     log_name: name of the file or folder affected
        ///     log_directory: gets the absolutel path
        ///     log_timestamp: MM/DD/YYYY HH:MM:SS
        ///     log_action: gets the type of action being monitored (delete, change, rename, create, etc)
        /// </summary>
        /// <remarks>If the table exists, then the data will just add to it, and not recreate it</remarks>
        private void CreateTable()
        {
            string table = "CREATE TABLE if not exists Log_details (" +
                "log_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "log_name VARCHAR(100), " +
                "log_path VARCHAR (200), " +
                "log_rename VARCHAR (200), " +
                "log_timestamp VARCHAR (40), " +
                "log_action VARCHAR (10));";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = table;
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// queries the entire database
        /// </summary>
        /// <returns>a list of results</returns>
        public List<DatabaseData> QueryDatabase()
        {
            if (sqlite_datareader != null)
            {
                sqlite_datareader.Close();
            }

            List<DatabaseData> results = new List<DatabaseData>();
            string query = "SELECT * FROM Log_details";
            sqlite_cmd.CommandText = query;
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                results.Add(new DatabaseData()
                {
                    Id = $"{sqlite_datareader["log_id"]}",
                    Timestamp = $"{sqlite_datareader["log_timestamp"]}",
                    Name = $"{sqlite_datareader["log_name"]}",
                    Action = $"{sqlite_datareader["log_action"]}",
                    AbsolutePath = $"{sqlite_datareader["log_path"]}"
                    //RenamePath = $"{sqlite_datareader["log_rename"]}";
                });
            }

            return results;
        }

        /// <summary>
        /// inserts new data to the database
        /// </summary>
        /// <param name="newData">a collection of the each row</param>
        public void WriteToDatabase(List<DatabaseData> newData)
        {
            string query = "";

            foreach(DatabaseData data in newData)
            {
                query += "INSERT INTO Log_details (\'log_name\',\'log_path\', \'log_timestamp\', \'log_action\') VALUES ";
                query += "(";
                query += "\'" + data.Name + "\', ";
                query += "\'" + data.AbsolutePath + "\', ";
                query += "\'" + data.Timestamp + "\', ";
                query += "\'" + data.Action + "\'";
                query += ");";
            }

            if (sqlite_datareader != null)
            {
                sqlite_datareader.Close();
            }

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Empties the database
        /// </summary>
        public void EmptyDatabase()
        {
            string query = "DELETE FROM Log_details";

            if (sqlite_datareader != null)
            {
                sqlite_datareader.Close();
            }

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Closes the database connection
        /// </summary>
        public void StopDatabase()
        {
            if (sqlite_datareader != null)
            {
                sqlite_datareader.Close();
            }
            
            sqlite_conn.Close();
        }
    }
}
