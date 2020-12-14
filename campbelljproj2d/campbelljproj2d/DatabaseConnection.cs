using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace campbelljproj2d
{
    /// <summary>
    /// Creates or opens the database, and starts/stops the connection along with other actions
    /// </summary>
    class DatabaseConnection
    {
        private readonly SQLiteConnection sqlite_conn;
        private readonly SQLiteCommand sqlite_cmd;
        private SQLiteDataReader sqlite_datareader;
        public Player player;

        /// <summary>
        /// Constructor that starts the connection, and creates/opens the database
        /// </summary>
        //public DatabaseConnection(string playerName, string category, int gridSize)
        public DatabaseConnection()
        {
            try
            {
                sqlite_conn = new SQLiteConnection("Data Source=TriviaMaze.db;Version=3;New=True;Compress=True;");
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();
            }
            catch (SQLiteException e)
            {
                MessageBox.Show("Unable to connect to Database\nDetails: " + e, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        /// <summary>
        /// Creates a new game using the player's name, category, and gridsize
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="category"></param>
        /// <param name="gridSize"></param>
        public void NewGame(string playerName, string category, int gridSize)
        {
            this.player = new Player
            {
                Name = playerName,
                Category = category,
                GridSize = gridSize,
            };
        }

        /// <summary>
        /// sequence of methods that look up the category id (from the name), creates the player (database) and the maze 
        /// </summary>
        public void CreateGame()
        {
            GetCategoryId();
            CreatePlayer();
            CreateMaze();
        }

        /// <summary>
        /// query that gets all the saved games for the LoadWindow
        /// </summary>
        /// <returns></returns>
        public List<Player> ShowSavedGames()
        {
            CheckSqliteReader();

            List<Player> players = new List<Player>();
            List<Room> rooms = new List<Room>();

            string query = "";
            query += "SELECT p.player_id, p.player_name, p.grid_size, c.category_name ";
            query += "FROM Players AS p, Categories AS c ";
            query += "WHERE c.category_id=p.category_id ";
            query += "AND save_game=1 ";
            query += "ORDER BY p.player_id;";

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                players.Add(new Player
                {
                    Id = Int32.Parse($"{sqlite_datareader["player_id"]}"),
                    Name = $"{sqlite_datareader["player_name"]}",
                    GridSize = Int32.Parse($"{sqlite_datareader["grid_size"]}"),
                    Category = $"{sqlite_datareader["category_name"]}",
                });
            }

            return players;
        }

        /// <summary>
        /// Once the user has selected the game to load, the player_id is used to get all data from the Players and associated Rooms tables
        /// </summary>
        /// <param name="player_id"></param>
        /// <returns></returns>
        public List<Room> LoadGame(int player_id)
        {
            CheckSqliteReader();

            UpdatePlayer("saved_game", 0);

            CheckSqliteReader();

            List<Room> rooms = new List<Room>();
            string query = "";
            query += "SELECT p.player_name, p.grid_size, p.category_id, c.category_name, p.maze_id, p.current_room_id, p.correct_answers, p.questions_answered ";
            query += "FROM Players AS p, Categories AS c ";
            query += "WHERE p.player_id=" + player_id + " ";
            query += "AND c.category_id=p.category_id;";

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                this.player = new Player
                {
                    Id = player_id,
                    Name = $"{sqlite_datareader["player_name"]}",
                    GridSize = Int32.Parse($"{sqlite_datareader["grid_size"]}"),
                    CategoryId = Int32.Parse($"{sqlite_datareader["category_id"]}"),
                    Category = $"{sqlite_datareader["category_name"]}",
                    MazeId = Int32.Parse($"{sqlite_datareader["maze_id"]}"),
                    CurrentRoomId = Int32.Parse($"{sqlite_datareader["current_room_id"]}"),
                    CorrectAnswers = Int32.Parse($"{sqlite_datareader["correct_answers"]}"),
                };

                string[] list = ($"{sqlite_datareader["questions_answered"]}").Split(',');
                foreach (string split in list)
                {
                    this.player.QuestionsAnswered.Add(Int32.Parse(split));
                }

            }

            CheckSqliteReader();

            query = "SELECT room_id, room_name, north_status_id, east_status_id, south_status_id, west_status_id, x_coordinate, y_coordinate ";
            query += "FROM Rooms ";
            query += "WHERE maze_id=" + player.MazeId + " ";
            query += "AND player_id=" + player.Id + ";";

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                int room_id = Int32.Parse($"{sqlite_datareader["room_id"]}");

                this.player.Rooms.Add(room_id);

                if (room_id == this.player.CurrentRoomId)
                {
                    this.player.CurrentRoomName = $"{sqlite_datareader["room_name"]}";
                }

                string room_name = $"{sqlite_datareader["room_name"]}";
                Point point = new Point(Int32.Parse($"{sqlite_datareader["x_coordinate"]}"), Int32.Parse($"{sqlite_datareader["y_coordinate"]}"));

                int north = Int32.Parse($"{sqlite_datareader["north_status_id"]}");
                int east = Int32.Parse($"{sqlite_datareader["east_status_id"]}");
                int south = Int32.Parse($"{sqlite_datareader["south_status_id"]}");
                int west = Int32.Parse($"{sqlite_datareader["west_status_id"]}");

                if (north == 3 || east == 3 || south == 3 || west == 3)
                {
                    string[] splitRoomName = room_name.Split(' ');
                    int roomNumber = Int32.Parse(splitRoomName[1]);
                    if (roomNumber != 1)
                    {
                        this.player.RoomsOpen.Add(roomNumber);
                    }
                    
                }               

                int[] status = new int[4] { north, east, south, west };
                bool[] sides = new bool[4] {
                    north == 2,
                    east == 2,
                    south == 2,
                    west == 2
                };
                string[] sideStatus = new string[4]; 
                
                for (int ix = 0; ix < sideStatus.Length; ix++)
                {
                    switch(status[ix])
                    {
                        case 1:
                            sideStatus[ix] = "wall";
                            break;
                        case 2:
                            sideStatus[ix] = "closed";
                            break;
                        case 3:
                            sideStatus[ix] = "open";
                            break;
                        default:
                            sideStatus[ix] = "locked";
                            break;
                    }
                }

                rooms.Add(new Room(room_id, room_name, point, sides, sideStatus, player.GridSize));
            }

            return rooms;
        }

        /// <summary>
        /// Updates the players table from 0 to 1, to represent a saved game
        /// </summary>
        public void SaveGame()
        {
            CheckSqliteReader();

            string query = "UPDATE Players SET save_game=1;";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// checks to see if the datareader is null or needs to be closed before performing
        /// </summary>
        private void CheckSqliteReader()
        {
            if (sqlite_datareader != null)
            {
                sqlite_datareader.Close();
            }
        }

        /// <summary>
        /// gets the selected category id from the name
        /// </summary>
        private void GetCategoryId()
        {
            CheckSqliteReader();

            string query = "SELECT category_id FROM Categories WHERE category_name=\'" + this.player.Category + "\' LIMIT 1;";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                this.player.CategoryId = Int32.Parse($"{sqlite_datareader["category_id"]}");
            }
        }

        /// <summary>
        /// Creates the player by adding a new row in the database, then retrieving that same record to get the Id of the player
        /// </summary>
        private void CreatePlayer()
        {
            CheckSqliteReader();

            string query = "INSERT INTO Players (\'player_name\',\'category_id\', \'grid_size\') VALUES (";
            query += "\'" + this.player.Name + "\', ";
            query += this.player.CategoryId + ", ";
            query += this.player.GridSize + " ";
            query += ");";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();

            AddPlayerId();
        }

        /// <summary>
        /// retrieves the id of the newly created player, and updates the Player class
        /// </summary>
        private void AddPlayerId()
        {
            CheckSqliteReader();

            string query = "SELECT player_id " +
                "FROM Players " +
                "WHERE player_name=\'" + this.player.Name + "\' " +
                "AND category_id=" + this.player.CategoryId + " " +
                "AND grid_size=" + this.player.GridSize + " " +
                "ORDER BY player_id DESC " +
                "LIMIT 1;";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                this.player.Id = Int32.Parse($"{sqlite_datareader["player_id"]}");
            }
        }

        /// <summary>
        /// Creates a new maze in the database, and then retrieves the newly created id
        /// </summary>
        private void CreateMaze()
        {
            CheckSqliteReader();

            string query = "";
            query += "INSERT INTO Mazes (\'player_id\') VALUES ";
            query += "(";
            query += this.player.Id + " ";
            query += ");";

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();

            AddMazeId();
        }

        /// <summary>
        /// retrieves the id of the newly created maze, and updates the Player class
        /// </summary>
        private void AddMazeId()
        {
            CheckSqliteReader();

            string query = "SELECT maze_id " +
                "FROM Mazes " +
                "WHERE player_id=" + player.Id + " " +
                "ORDER BY maze_id DESC " +
                "LIMIT 1;";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                this.player.MazeId = Int32.Parse($"{sqlite_datareader["maze_id"]}");
                UpdatePlayer("maze_id", this.player.MazeId);
            }
        }

        /// <summary>
        /// method that can be used to update the current players row
        /// </summary>
        /// <param name="key">the column name</param>
        /// <param name="value">updated data for the row</param>
        public void UpdatePlayer(string key, int value)
        {
            CheckSqliteReader();

            string query = "UPDATE Players SET " + key + "=" + value + " WHERE player_id=" + this.player.Id + ";";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the Players questions_answered field to keep track of what has been asked (for loading or duplication)
        /// </summary>
        /// <param name="question_id">the id of the trivia question viewed</param>
        public void UpdatePlayerQuestionIds(int question_id)
        {
            CheckSqliteReader();

            this.player.QuestionsAnswered.Add(question_id);

            string questions_answered = "";
            string query = "";
            query += "SELECT questions_answered ";
            query += "FROM Players ";
            query += "WHERE player_id=" + this.player.Id;

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                questions_answered += $"{sqlite_datareader["questions_answered"]}";
            }

            if (questions_answered.Length > 1)
            {
                questions_answered += "," + question_id;
            }
            else
            {
                questions_answered += question_id;
            }
            

            CheckSqliteReader();

            query = "UPDATE Players SET questions_answered=\'" + questions_answered + "\' WHERE player_id=" + this.player.Id + ";";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
        }
   
        /// <summary>
        /// updates the number of correct answers to the questions
        /// </summary>
        /// <remarks>can be used in version 1.1 for highscores</remarks>
        public void UpdatePlayerCorrectAnswer()
        {
            this.player.CorrectAnswers += 1;

            CheckSqliteReader();

            string query = "UPDATE Players SET correct_answers=" + this.player.CorrectAnswers + " WHERE player_id=" + this.player.Id + ";";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates a new row for the newly created room
        /// </summary>
        /// <param name="room"></param>
        public void CreateRoom(Room room)
        {
            CheckSqliteReader();
            string query = "";
            int[] door_status = new int[room.Sides.Length];

            query += "INSERT INTO Rooms (\'room_name\',\'player_id\',\'north_status_id\',\'east_status_id\',\'south_status_id\',\'west_status_id\',\'maze_id\', \'x_coordinate\', \'y_coordinate\') VALUES ";
            query += "(";
            query += "\'" + room.Name + "\', ";
            query += this.player.Id + ", ";
            
            for (int ix = 0; ix < room.Sides.Length; ix++)
            {
                if (room.Sides[ix])
                {
                    query += 2 + ", ";
                    door_status[ix] = 2; // closed
                } 
                else
                {
                    query += 1 + ", ";
                    door_status[ix] = 1; // wall
                }
            }
            
            query += this.player.MazeId + ", ";
            query += room.Coordinates.X + ", ";
            query += room.Coordinates.Y + " ";
            query += ");";

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();

            GetRoomId(door_status);
        }

        /// <summary>
        /// Gets the Id of the newly created room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public int GetRoomId(Room room)
        {
            CheckSqliteReader();
            int roomId = -1;
            string query = "";
            query += "SELECT room_id FROM Rooms ";
            query += "WHERE room_name=\'" + room.Name + "\' ";
            query += "AND player_id=" + player.Id + " ";
            query += "AND maze_id=" + player.MazeId + " ";

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                roomId = Int32.Parse($"{sqlite_datareader["room_id"]}");
            }

            return roomId;
        }

        /// <summary>
        /// Gets the newly created room based on the door status
        /// </summary>
        /// <param name="door_status"></param>
        private void GetRoomId(int[] door_status)
        {
            CheckSqliteReader();

            string[] directions = new string[4] { "north", "east", "south", "west" };
            string query = "SELECT room_id " +
                "FROM Rooms " +
                "WHERE player_id=" + this.player.Id + " " +
                "AND maze_id=" + this.player.MazeId + " ";

            for (int ix = 0; ix < door_status.Length; ix++)
            {
                query += "AND " + directions[ix] + "_status_id=" + door_status[ix] + " ";
            }

            query += "ORDER BY maze_id DESC " +
                "LIMIT 1;";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                player.Rooms.Add(Int32.Parse($"{sqlite_datareader["room_id"]}"));
            }
        }

        /// <summary>
        /// Gets the Room Id from the supplied name
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        public int GetRoomIdByName(int roomNumber)
        {
            CheckSqliteReader();

            string query = "SELECT room_id " +
                "FROM Rooms " +
                "WHERE player_id=" + this.player.Id + " " +
                "AND maze_id=" + this.player.MazeId + " " +
                "AND room_name=\'Room " + roomNumber + "\'";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                return Int32.Parse($"{sqlite_datareader["room_id"]}");
            }
            return -1;
        }

        /// <summary>
        /// Keeps track of the last room viewed
        /// </summary>
        /// <returns></returns>
        public string GetCurrentRoom()
        {
            CheckSqliteReader();

            string query = "SELECT room_id, room_name " +
                "FROM Rooms " +
                "WHERE player_id=" + this.player.Id + " " +
                "AND maze_id=" + this.player.MazeId + " " +
                "AND room_name=\'Room 1\'" +
                ";";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                this.player.CurrentRoomId = Int32.Parse($"{sqlite_datareader["room_id"]}");
                this.player.CurrentRoomName = $"{sqlite_datareader["room_name"]}";
                UpdatePlayer("current_room_id", this.player.CurrentRoomId);
            }

            return this.player.CurrentRoomName;
        }
        /*
        private string GetDoorStatus(int num)
        {
            CheckSqliteReader();

            string query = "SELECT status_name " +
                "FROM Status " +
                "WHERE status_id=" + num +
                ";";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                return $"{sqlite_datareader["room_name"]}";
            }

            return "";
        }
        */
        /// <summary>
        /// Gets the records of the room that user is attempting to access
        /// </summary>
        /// <param name="roomNum"></param>
        /// <returns></returns>
        public string[] GetAdjacentRoom(int roomNum)
        {
            CheckSqliteReader();

            string[] sides = new string[4];
            string query = "SELECT * " +
                "FROM Rooms " +
                "WHERE player_id=" + this.player.Id + " " +
                "AND maze_id=" + this.player.MazeId + " " +
                "AND room_name=\'Room 1\'" +
                ";";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                this.player.CurrentRoomId = Int32.Parse($"{sqlite_datareader["room_id"]}");
                this.player.CurrentRoomName = $"{sqlite_datareader["room_name"]}";
                UpdatePlayer("current_room_id", this.player.CurrentRoomId);
            }

            return sides;
        }

        /// <summary>
        /// User will get a random question every time, and makes sure the user hasn't answered that question yet for the game
        /// </summary>
        /// <returns>the trivia question</returns>
        public TriviaQuestion GetRandomQuestion()
        {
            CheckSqliteReader();

            TriviaQuestion triviaQuestion = new TriviaQuestion();
            int trivia_id = GetListOfTrivia();
            CheckSqliteReader();

            string query = "";
            query += "SELECT question, choice1, choice2, choice3, answer, answerInfo ";
            query += "FROM Trivia ";

            if (!player.Category.Equals("Mixed Trivia"))
            {
                query += "WHERE category_id=" + this.player.CategoryId + " ";
            }
            
            query += "AND trivia_id=" + trivia_id + ";";

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (sqlite_datareader.Read())
            {
                triviaQuestion.QuestionId = trivia_id;
                triviaQuestion.Question = $"{sqlite_datareader["question"]}";
                triviaQuestion.Choice1 = $"{sqlite_datareader["choice1"]}";
                triviaQuestion.Choice2 = $"{sqlite_datareader["choice2"]}";
                triviaQuestion.Choice3 = $"{sqlite_datareader["choice3"]}";
                triviaQuestion.Answer = $"{sqlite_datareader["answer"]}";
                triviaQuestion.AnswerInfo = $"{sqlite_datareader["answerInfo"]}";
            }

            return triviaQuestion;
        }

        /// <summary>
        /// gets a list of all the trivia questions ids and puts into a list
        /// </summary>
        /// <returns>list of ids</returns>
        private int GetListOfTrivia()
        {
            CheckSqliteReader();

            List<int> triviaIds = new List<int>();
            string query = "";
            query += "SELECT trivia_id ";
            query += "FROM Trivia ";

            if (!player.Category.Equals("Mixed Trivia"))
            {
                query += "WHERE category_id=" + this.player.CategoryId + ";";
            }

            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                triviaIds.Add(Int32.Parse($"{sqlite_datareader["trivia_id"]}"));
            }

            int randomId;

            do
            {
                randomId = RandomInt(0, triviaIds.Count);
            } while (this.player.QuestionsAnswered.Count > 0 && this.player.QuestionsAnswered.Contains(triviaIds[randomId]));

            player.QuestionsAnswered.Add(triviaIds[randomId]);

            return triviaIds[randomId];
        }

        /// <summary>
        /// selects a random number between the mininum and maximum -1 
        /// </summary>
        /// <param name="min">starting number</param>
        /// <param name="max">ending number minus one</param>
        /// <returns>the number</returns>
        private int RandomInt(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// Updates the status of the room's door
        /// </summary>
        /// <param name="door">north, east, south, or west</param>
        /// <param name="answer">true if the answer was right</param>
        /// <param name="id">the room's id</param>
        public void UpdateRoom(string door, bool answer, int id)
        {
            CheckSqliteReader();

            int door_status = answer == true ? 3 : 4;
            string query = "UPDATE Rooms SET " + door + "_status_id=" + door_status + " WHERE room_id=" + id + ";";
            sqlite_cmd.CommandText = query;
            sqlite_cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// Empties the database
        /// </summary>
        public void EmptyDatabase()
        {
            string query = "DELETE FROM Players;";

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
