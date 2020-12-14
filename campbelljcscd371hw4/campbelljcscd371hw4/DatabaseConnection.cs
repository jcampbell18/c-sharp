using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace campbelljcscd371hw4
{
    /// <summary>
    /// Creates or opens the database, and starts/stops the connection along with other actions
    /// </summary>
    class DatabaseConnection
    {
        private readonly SQLiteConnection sqlite_conn;
        private SQLiteCommand sqlite_cmd;
        private SQLiteDataReader sqlite_datareader;
        private readonly int LIMIT = 10;

        /// <summary>
        /// Constructor that starts the connection, and creates/opens the database
        /// </summary>
        public DatabaseConnection()
        {
            try
            {
                sqlite_conn = new SQLiteConnection("Data Source=highscores.db;Version=3;New=True;Compress=True;");
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
            string table = "CREATE TABLE if not exists highscores" +
                " (" +
                "hs_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "hs_name VARCHAR(5), " +
                "hs_score INTEGER, " +
                "hs_timestamp VARCHAR (40)" +
                ");";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = table;
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// queries the entire database for the top 10 scores
        /// </summary>
        /// <returns>a list of results</returns>
        public List<Highscore> QueryDatabase()
        {
            if (sqlite_datareader != null)
            {
                sqlite_datareader.Close();
            }

            List<Highscore> results = new List<Highscore>();
            string query = "SELECT * FROM highscores ORDER BY hs_score DESC LIMIT " + LIMIT + ";";
            sqlite_cmd.CommandText = query;
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                results.Add(new Highscore()
                {
                    Timestamp = $"{sqlite_datareader["hs_timestamp"]}",
                    Name = $"{sqlite_datareader["hs_name"]}",
                    Score = Int32.Parse($"{sqlite_datareader["hs_score"]}")
                });
            }

            return results;
        }

        /// <summary>
        /// queries the database for the top 10 scores and returns the first and last
        /// </summary>
        /// <returns>the highest score and the 10th best score</returns>
        public int[] GetHighAndLowScores()
        {
            int[] highlow = new int[2];
            int ix = 0;
            int index = 0;

            if (sqlite_datareader != null)
            {
                sqlite_datareader.Close();
            }

            string query = "SELECT * FROM highscores ORDER BY hs_score DESC LIMIT " + LIMIT + ";";
            sqlite_cmd.CommandText = query;
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                if (ix == 0 || ix == LIMIT - 1)
                {
                    highlow[index] = Int32.Parse($"{sqlite_datareader["hs_score"]}");
                    index++;
                }

                ix++;
            }

            return highlow;

        }

        /// <summary>
        /// inserts new data to the database
        /// </summary>
        /// <param name="newData">a collection of the each row</param>
        public void WriteToDatabase(Highscore player)
        {
            string query = "";
            query += "INSERT INTO highscores (\'hs_name\',\'hs_score\', \'hs_timestamp') VALUES ";
            query += "(";
            query += "\'" + player.Name + "\', ";
            query += player.Score + ", ";
            query += "\'" + player.Timestamp + "\'";
            query += ");";

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
            string query = "DELETE FROM highscores";

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
