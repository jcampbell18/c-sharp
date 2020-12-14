using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace campbelljproj2d
{
    /// <summary>
    /// This class builds the foundation of the database. It will read text files, and populate the Categories, Status, and Trivia tables
    /// </summary>
    class DatabaseBuilder
    {
        private readonly SQLiteConnection sqlite_conn;
        private readonly SQLiteCommand sqlite_cmd;
        private readonly FileIO fio;
        private readonly string[] tables;

        /// <summary>
        /// Performs sequence of events: creating (if it doesn't exist) tables, deletes any records, resets the primary keys back to 0, and populates the tables from the text files
        /// </summary>
        public DatabaseBuilder()
        {
            this.fio = new FileIO();
            tables = new string[] { "Categories", "Status", "Trivia" };

            try
            {
                sqlite_conn = new SQLiteConnection("Data Source=TriviaMaze.db;Version=3;New=True;Compress=True;");
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();

                BuildTables();
                DeleteRecords();
                ResetSequence();
                PopulateTables();
            }
            catch (SQLiteException e)
            {
                MessageBox.Show("Unable to connect to Database\nDetails: " + e, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// takes the query and runs it
        /// </summary>
        /// <param name="query">the simple SQL query</param>
        private void RunQuery(string query)
        {
            if (query != null || query.Length > 0)
            {
                sqlite_cmd.CommandText = query;
                sqlite_cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// deletes all records from the categories, status, and trivia questions tables
        /// </summary>
        private void DeleteRecords()
        {
            foreach (string table in tables)
            {
                RunQuery("DELETE FROM " + table + ";");
            }
        }

        /// <summary>
        /// resets the primary keys back to 0, so the auto-increment will start with 1
        /// </summary>
        private void ResetSequence()
        {
            foreach (string table in tables)
            {
                RunQuery("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='" + table + "';");
            }
        }

        /// <summary>
        /// runs a sequence of methods to create tables
        /// </summary>
        private void BuildTables()
        {
            RunQuery( CreateCategoriesTable() );
            RunQuery( CreateTriviaTable()     );
            RunQuery( CreateStatusTable()     );
            RunQuery( CreateRoomsTable()      );
            RunQuery( CreateMazesTable()      );
            RunQuery( CreatePlayersTable()    );
            RunQuery( CreateHighscoresTable() );
        }

        /// <summary>
        /// creates the trivia questions table
        /// </summary>
        /// <returns></returns>
        private string CreateTriviaTable()
        {
            return "CREATE TABLE if not exists Trivia (" +
                    "trivia_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "category_id INTEGER, " +
                    "question VARCHAR(200), " +
                    "choice1 VARCHAR (50), " +
                    "choice2 VARCHAR (50), " +
                    "choice3 VARCHAR (50), " +
                    "answer VARCHAR (50), " +
                    "answerInfo VARCHAR (200)" +
                ");";
        }

        /// <summary>
        /// creates the categories table
        /// </summary>
        /// <returns></returns>
        private string CreateCategoriesTable()
        {
            return "CREATE TABLE if not exists Categories (" +
                    "category_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "category_name VARCHAR(15)" +
                ");";
        }

        /// <summary>
        /// creates the Players table with all necessary data to load a saved game
        /// </summary>
        /// <returns></returns>
        private string CreatePlayersTable()
        {
            return "CREATE TABLE if not exists Players (" +
                    "player_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "player_name VARCHAR(15), " +
                    "category_id INTEGER, " +
                    "maze_id INTEGER, " +
                    "grid_size INTEGER, " +
                    "current_room_id INTEGER, " + 
                    "save_game DEFAULT 0, " +
                    "questions_answered TEXT, " +
                    "correct_answers DEFAULT 0 " +
                ");";
        }

        /// <summary>
        /// creates the mazes table
        /// </summary>
        /// <returns></returns>
        private string CreateMazesTable()
        {
            return "CREATE TABLE if not exists Mazes (" +
                    "maze_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "player_id INTEGER, " +
                    "room_id INTEGER " +
                ");";
        }

        /// <summary>
        /// creates the rooms table
        /// </summary>
        /// <returns></returns>
        private string CreateRoomsTable()
        {
            return "CREATE TABLE if not exists Rooms (" +
                    "room_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "room_name VARCHAR(20), " +
                    "maze_id INTEGER, " +
                    "player_id INTEGER, " +
                    "north_status_id INTEGER, " +
                    "east_status_id INTEGER, " +
                    "south_status_id INTEGER, " +
                    "west_status_id INTEGER, " +
                    "x_coordinate REAL, " +
                    "y_coordinate REAL " +
                ");";
        }

        /// <summary>
        /// creates the status table
        /// </summary>
        /// <returns></returns>
        private string CreateStatusTable()
        {
            return "CREATE TABLE if not exists Status (" +
                    "status_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "status_name VARCHAR(10)" +
                ");";
        }

        /// <summary>
        /// creates the highscores table
        /// </summary>
        /// <remarks>this is for version 1.1</remarks>
        /// <returns></returns>
        private string CreateHighscoresTable()
        {
            return "CREATE TABLE if not exists Highscores (" +
                    "highscore_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "player_id VARCHAR(15) " +
                ");";
        }

        /// <summary>
        /// runs a sequence of methods to populate particular tables
        /// </summary>
        private void PopulateTables()
        {
            RunQuery( PopulateCategories("db_categories.txt")   );
            RunQuery( PopulateStatus("db_status.txt")           );
            RunQuery( PopulateTrivia("db_trivia.txt")           );
        }

        /// <summary>
        /// populates the categories table
        /// </summary>
        /// <param name="filename">text file that contains categories</param>
        /// <returns>the SQL query to be run</returns>
        private string PopulateCategories(string filename)
        {
            List<string> categories = fio.ReadFile(filename);
            string query = "INSERT INTO Categories (\'category_name\') VALUES ";

            for (int ix = 0; ix < categories.Count; ix++)
            {
                query += "(";
                query += "\'" + categories[ix] + "\'";
                query += ")";

                if (ix < categories.Count - 1)
                {
                    query += ",";
                } else
                {
                    query += ";";
                }
            }

            return query;
        }

        /// <summary>
        /// populates the status table
        /// </summary>
        /// <param name="filename">text file that contains different statuses of the door</param>
        /// <returns>the SQL query to be run</returns>
        private string PopulateStatus(string filename)
        {
            List<string> status = fio.ReadFile(filename);
            string query = "INSERT INTO Status (\'status_name\') VALUES ";

            for (int ix = 0; ix < status.Count; ix++)
            {
                query += "(";
                query += "\'" + status[ix] + "\'";
                query += ")";

                if (ix < status.Count - 1)
                {
                    query += ",";
                }
                else
                {
                    query += ";";
                }
            }

            return query;
        }

        /// <summary>
        /// populates the trivia questions table
        /// </summary>
        /// <param name="filename">text file that contains trivia questions</param>
        /// <returns>the SQL query to be run</returns>
        private string PopulateTrivia(string filename)
        {
            List<string> list = fio.ReadFile(filename);
            string query = "INSERT INTO Trivia (\'category_id\',\'question\', \'choice1\', \'choice2\', \'choice3\', \'answer\', \'answerInfo\') VALUES ";

            for (int ix = 0; ix < list.Count; ix++)
            {
                string[] questions = list[ix].Split(',');
                query += "(";

                for (int jx = 0; jx < questions.Length; jx++)
                {
                    if (jx == 0)
                    {
                        query += Int32.Parse(questions[jx]) + ", ";
                    }
                    else
                    {
                        query += questions[jx];

                        if (jx < questions.Length - 1)
                        {
                            query += ", ";
                        }
                    }
                    
                }

                query += ")";

                if (ix < list.Count - 1)
                {
                    query += ", ";
                }
            }

            query += ";";
            return query;
        }

    }
}
