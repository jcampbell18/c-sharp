/*
 * Author: Jason Campbell
 * Project Name: Programming Project 2d - Trivia Maze
 * 
 * Project Description: The goal of this game is that the user
 *      gets through the maze of rooms to the exit. The user 
 *      starts in a maze of rooms with the intention of answering 
 *      questions correctly, in order to move to the next room.
 *      The user starts in room 1, and has to open doors to get to
 *      the next room. Should the user get the answer wrong, the 
 *      door is permanently locked. If the user is unable to go any
 *      further because all the doors are locked, then the game is over.
 *      
 *  Complete documentation found below
*/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace campbelljproj2d
{
    /// <summary>
    /// This window contains a menu bar, toolbar (shortcuts from the menubar), the canvas which holds the rooms and doors, and status bar
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DatabaseConnection databaseConnection;
        private PlayerWindow playerWindow;
        private LoadGameWindow loadGameWindow;
        private Maze maze;
        private List<Room> rooms;
        private double roomSize;
        private TextBlock[] textBlocks;
        private TextBlock tbCurrentPosition;
        private TextBlock tbStart;
        private TextBlock tbEnd;

        /// <summary>
        /// makes the connection to the database and disables clicking on the canvas
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            UpdateDatabase();
            databaseConnection = new DatabaseConnection();
            myCanvas.IsEnabled = false;
        }

        /// <summary>
        /// reveals instructions in a modal
        /// </summary>
        private void GetInstructions()
        {
            string msg = "";
            msg += "Trivia Maze\n";
            msg += "---------------------------------------------------------------------\n\n";
            msg += "The maze is composed of multiple rooms.\n\n";
            msg += "Each room has 2 or more doors.\n\n";
            msg += "The doors are color-coded:\n";
            msg += "\tRed = closed door\n";
            msg += "\tGreen = open door\n";
            msg += "\tBlack = wall or locked door\n\n";
            msg += "In order for you to pass through a door,\n";
            msg += "\tyou must correctly answer the question\n\n";
            msg += "If you are unable to answer the question correctly,\n";
            msg += "\tthat door is permanently locked.\n\n";
            msg += "If all doors are permanently locked in the\n";
            msg += "\tavailable/open rooms, then the game is over\n\n";
            msg += "Answering the question correctly will allow you to move\n";
            msg += "\tinto the next room.\n";
            msg += "\n\n";
            string titleBar = "Instructions";
            MessageBoxButton messageBoxButtons = MessageBoxButton.OK;
            MessageBoxImage messageBoxImage = MessageBoxImage.Question;
            MessageBox.Show(msg, titleBar, messageBoxButtons, messageBoxImage);
        }

        /// <summary>
        /// Sequence of methods that gets the necessary information to start a game, sync'ing with the database
        /// </summary>
        private void StartNewGame()
        {
            if (GetPlayer())
            {
                myCanvas.Children.Clear();
                GetLevel();
                GetMaze();
                GetStart();
                GetEnd();
                GetRoomTextBlocks();
                GetPaths();
                this.menuItemUpdateDB.IsEnabled = false;
                this.menuItemLoad.IsEnabled = false;
                this.btnLoad.IsEnabled = false;
                this.menuItemSave.IsEnabled = true;
                this.btnSave.IsEnabled = true;
                myCanvas.IsEnabled = true;
                this.menuItemQuit.IsEnabled = true;
                this.sbStatusContent.Content = "Click the current room";
            }
        }

        /// <summary>
        /// Sequence of methods that gets the necessary information to load a saved game, sync'ing with the database
        /// </summary>
        private void StartLoadedGame()
        {
            myCanvas.Children.Clear();
            GetLevel();
            this.maze = new Maze(this.databaseConnection.player.GridSize, this.myCanvas.Width, this.rooms);
            this.roomSize = this.myCanvas.Width / this.databaseConnection.player.GridSize;
            GetStart();
            GetEnd();
            GetRoomTextBlocks();
            GetPaths();

            this.sbPlayerName.Content = this.databaseConnection.player.Name;
            this.sbCurrentLocation.Content = databaseConnection.GetCurrentRoom();
            this.menuItemUpdateDB.IsEnabled = false;
            this.menuItemLoad.IsEnabled = false;
            this.btnLoad.IsEnabled = false;
            this.menuItemSave.IsEnabled = true;
            this.btnSave.IsEnabled = true;
            myCanvas.IsEnabled = true;
            this.menuItemQuit.IsEnabled = true;
            this.sbStatusContent.Content = "Click the current room";
        }

        /// <summary>
        /// quits the game, and allows the user to start another game or exit
        /// </summary>
        private void QuitGame()
        {
            myCanvas.Children.Clear();
            myCanvas.IsEnabled = false;
            sbStatusContent.Content = "Start or Load Game";
            this.menuItemUpdateDB.IsEnabled = false; //true
            this.menuItemLoad.IsEnabled = true;
            this.btnLoad.IsEnabled = true;
            this.menuItemSave.IsEnabled = false;
            this.btnSave.IsEnabled = false;
            this.menuItemQuit.IsEnabled = false;
        }

        /// <summary>
        /// Saves the game, and tells the user with modal that it has been saved
        /// </summary>
        private void SaveGame()
        {
            this.databaseConnection.SaveGame();
            MessageBox.Show("Game is saved", "Save Game", MessageBoxButton.OK, MessageBoxImage.Information);
            QuitGame();
        }

        /// <summary>
        /// If there are saved games, then a new window will appear showing all saved games
        /// If there are no saved games, a window will let the user know
        /// </summary>
        private void LoadGame()
        {
            List<Player> players = this.databaseConnection.ShowSavedGames();

            if (players.Count > 0)
            {
                this.loadGameWindow = new LoadGameWindow(players);
                while (this.loadGameWindow.ShowDialog() == true) ;

                int id = this.loadGameWindow.PlayerId;
                if (id != -1)
                {
                    this.rooms = this.databaseConnection.LoadGame(id);
                    StartLoadedGame();
                }

            }
            else
            {
                string msg = "There are no saved games";
                string titleCaption = "Load Game";
                MessageBoxButton btn = MessageBoxButton.OK;
                MessageBox.Show(msg, titleCaption, btn);
            }

        }

        /// <summary>
        /// To start, the game needs to know the name of the palyer, the grid size, and trivia category
        /// </summary>
        /// <returns></returns>
        private bool GetPlayer()
        {
            this.playerWindow = new PlayerWindow();
            while (this.playerWindow.ShowDialog() == true);

            if (this.playerWindow.PlayerName != null)
            {
                this.sbPlayerName.Content = this.playerWindow.PlayerName;
                this.roomSize = myCanvas.Width / this.playerWindow.MazeSize;
                databaseConnection.NewGame(this.playerWindow.PlayerName, this.playerWindow.Category, this.playerWindow.MazeSize);
                databaseConnection.CreateGame();
                return true;
            }

            return false;
        }

        /// <summary>
        /// the choice of the grid size are categorized from easy to extreme
        /// </summary>
        private void GetLevel()
        {
            switch (this.databaseConnection.player.GridSize)
            {
                case 2:
                    this.sbPlayerMode.Content = "Easy";
                    break;
                case 3:
                    this.sbPlayerMode.Content = "Normal";
                    break;
                case 4:
                    this.sbPlayerMode.Content = "Hard";
                    break;
                case 5:
                    this.sbPlayerMode.Content = "Extreme";
                    break;
            }
        }

        /// <summary>
        /// creates the maze
        /// </summary>
        private void GetMaze()
        {
            this.maze = new Maze(this.databaseConnection.player.GridSize, this.myCanvas.Width);
            GetRooms();
        }

        /// <summary>
        /// Gets the textblock and places the "Room #" in the middle of each room
        /// </summary>
        private void GetRoomTextBlocks()
        {
            int total = (int) Math.Pow(this.databaseConnection.player.GridSize, 2);
            textBlocks = new TextBlock[total];

            for (int ix = 1; ix <= total; ix++)
            {
                textBlocks[ix-1] = this.maze.LabelRooms(ix);

                double x = rooms[ix-1].Coordinates.X + (this.roomSize / 2) - (textBlocks[ix - 1].Width / 2);
                double y = rooms[ix-1].Coordinates.Y + (this.roomSize / 2) - (textBlocks[ix - 1].Height / 2);

                Canvas.SetLeft(textBlocks[ix - 1], x);
                Canvas.SetTop(textBlocks[ix - 1], y);
                myCanvas.Children.Add(textBlocks[ix - 1]);
            }
        }

        /// <summary>
        /// Gets the start textblock and places under Room 1
        /// </summary>
        private void GetStart()
        {
            this.tbStart = this.maze.CreateStart();

            double x = (this.roomSize / 2) - (this.tbStart.Width / 2);
            double y = (this.roomSize / 2) - (this.tbStart.Height / 2) + (this.tbStart.Height + 4);

            Canvas.SetLeft(this.tbStart, x);
            Canvas.SetTop(this.tbStart, y);
            myCanvas.Children.Add(this.tbStart);

            this.sbCurrentLocationLabel.Content = "Current Location:";
            this.sbCurrentLocation.Content = "Room 1 (Start)";
        }

        /// <summary>
        /// gets the end textblock and places under the last room number
        /// </summary>
        private void GetEnd()
        {
            this.tbEnd = this.maze.CreateEnd();
            double x = (this.myCanvas.Width - (this.roomSize / 2)) - (this.tbStart.Width / 2);
            double y = (this.myCanvas.Width - (this.roomSize / 2)) - (this.tbStart.Height / 2) + (this.tbStart.Height + 4);

            Canvas.SetLeft(this.tbEnd, x);
            Canvas.SetTop(this.tbEnd, y);
            myCanvas.Children.Add(this.tbEnd);
        }

        /// <summary>
        /// shows where the user currently is
        /// </summary>
        /// <param name="index"></param>
        private void ShowCurrentPosition(int index)
        {
            this.tbCurrentPosition = this.maze.CreateCurrentPosition();

            double x = rooms[index].Coordinates.X + (this.roomSize / 2) - (this.tbCurrentPosition.Width / 2);
            double y = rooms[index].Coordinates.Y + (this.roomSize / 2) - (this.tbCurrentPosition.Height / 2) + (this.tbStart.Height + 4);

            Canvas.SetLeft(this.tbCurrentPosition, x);
            Canvas.SetTop(this.tbCurrentPosition, y);
            myCanvas.Children.Add(this.tbCurrentPosition);
            sbCurrentLocationLabel.Content = "Current Position:";
            sbCurrentLocation.Content = "Room 1 - Start";
        }

        /// <summary>
        /// Adds the list of paths onto the canvas
        /// </summary>
        private void GetPaths()
        {
            List<Path> paths = this.maze.Paths;
            foreach(Path path in paths)
            {
                myCanvas.Children.Add(path);
            }
            
        }

        /// <summary>
        /// Gets the newly created rooms from maze creation
        /// </summary>
        private void GetRooms()
        {
            rooms = this.maze.Rooms;
            
            foreach(Room room in rooms)
            {
                databaseConnection.CreateRoom(room);
                room.Id = databaseConnection.GetRoomId(room);
            }

            this.sbCurrentLocation.Content = databaseConnection.GetCurrentRoom();
        }

        /// <summary>
        /// returns the room index number from the actual room number in the list
        /// </summary>
        /// <param name="roomNumber">room #</param>
        /// <returns>the index of the list</returns>
        private int GetRoomIndex(int roomNumber)
        {
            int index = -1;

            for (int ix = 0; ix < rooms.Count; ix++)
            {
                if (rooms[ix].Name.Equals("Room " + roomNumber))
                {
                    index = ix;
                }
            }

            return index;
        }

        /// <summary>
        /// Gets the X and Y coordinates of the where the user clicks on the canvas
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void GetPosition(double x, double y)
        {
            int row = 0;
            int col = 0;
            double roomWidth = myCanvas.Width / this.databaseConnection.player.GridSize;
            double roomHeight = myCanvas.Height / this.databaseConnection.player.GridSize;

            for (int ix = 0; ix < roomSize; ix++)
            {
                if (x > roomWidth * (ix) && x < roomWidth * (ix + 1))
                {
                    col = ix;
                }

                if (y > roomHeight * (ix) && y < roomWidth * (ix + 1))
                {
                    row = ix;
                }
            }

            GetDoor((row * this.databaseConnection.player.GridSize) + (col + 1));
        }

        /// <summary>
        /// once the room has been determined, the user will be presented with choices of available doors
        /// </summary>
        /// <param name="roomNumber"></param>
        private void GetDoor(int roomNumber)
        {
            if (IsCurrentRoom(roomNumber) || IsOpenRoom(roomNumber)) 
            {
                DoorsWindow doorsWindow = new DoorsWindow(GetSides(roomNumber))
                {
                    Title = "Room " + roomNumber
                };

                while (doorsWindow.ShowDialog() == true) ;

                if (doorsWindow.Submit)
                {
                    GetQuestion(doorsWindow.DoorSelected, roomNumber);
                }
            }
            else
            {
                string msg = "This room is unavailable.\nClick on your current room first to unlock any adjacent rooms";
                string titleCaption = "UNAVAILABLE!!";
                MessageBoxButton btn = MessageBoxButton.OK;
                MessageBoxImage img = MessageBoxImage.Error;
                MessageBox.Show(msg, titleCaption, btn, img);
            }
        }

        /// <summary>
        /// returns true if the user is in the current room
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        private bool IsCurrentRoom(int roomNumber)
        {
            if (this.databaseConnection.player.CurrentRoomId == this.databaseConnection.GetRoomIdByName(roomNumber))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// returns true if the room has been opened yet
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        private bool IsOpenRoom(int roomNumber)
        {
            foreach (int num in databaseConnection.player.RoomsOpen)
            {
                if (num == roomNumber)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// retrieves an bool array that represents the sides of the selected room number
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        private bool[] GetSides(int roomNumber)
        {

            foreach(Room room in rooms)
            {
                if (room.Name.Equals("Room " + roomNumber))
                {
                    return room.Sides;
                }
            }

            return new bool[4]{ false, false, false, false };
        }

        /// <summary>
        /// a random trivia question is selected and is sent to the new window for the user to answer
        /// if the answer is correct, then the door is opened and put onto the maze drawing
        /// </summary>
        /// <param name="door"></param>
        /// <param name="roomNumber"></param>
        private void GetQuestion(string door, int roomNumber)
        {
            TriviaQuestion triviaQuestion;
            triviaQuestion = this.databaseConnection.GetRandomQuestion();
            this.databaseConnection.UpdatePlayerQuestionIds(triviaQuestion.QuestionId);

            RoomWindow roomWindow = new RoomWindow(triviaQuestion)
            {
                Title = door + " Door"
            };

            while (roomWindow.ShowDialog() == true);

            // current room
            int index = GetRoomIndex(roomNumber);
            myCanvas.Children.Add(rooms[index].ReDrawDoor(door, roomWindow.Answer));
            databaseConnection.UpdateRoom(door, roomWindow.Answer, rooms[index].Id);
            this.rooms[index].ChangeSide(door);


            // connected room
            int adjRoomNum = GetAdjacentRoomNumber(roomNumber, door);
            index = GetRoomIndex(adjRoomNum);
            string oppDoor = GetOppositeDoor(door);
            this.myCanvas.Children.Add(rooms[index].ReDrawDoor(oppDoor, roomWindow.Answer));
            this.databaseConnection.UpdateRoom(oppDoor, roomWindow.Answer, rooms[index].Id);
            this.rooms[index].ChangeSide(oppDoor);
            

            if (roomWindow.Answer)
            {
                this.databaseConnection.player.CurrentRoomId = rooms[index].Id;
                this.databaseConnection.UpdatePlayer("current_room_id", rooms[index].Id);
                this.databaseConnection.player.CurrentRoomName = rooms[index].Name;
                this.databaseConnection.UpdatePlayerCorrectAnswer();
                this.databaseConnection.player.Rooms.Add(rooms[index].Id);
                this.databaseConnection.player.RoomsOpen.Add(adjRoomNum);
                ShowCurrentPosition(index);
                sbCurrentLocation.Content = "Room " + adjRoomNum;
                GetWinner(adjRoomNum);
            }

            ContinueGame();
        }

        /// <summary>
        /// returns the oppposite door of what is sent. If north is sent, then south is returned
        /// </summary>
        /// <remarks>this is used when a south door is opened. the adjacent door of another room is then opened in the north</remarks>
        /// <param name="door"></param>
        /// <returns></returns>
        private string GetOppositeDoor(string door)
        {
            string opp = "";
            switch(door)
            {
                case "north":
                    opp = "south";
                    break;
                case "east":
                    opp = "west";
                    break;
                case "south":
                    opp = "north";
                    break;
                default:
                    opp = "east";
                    break;
            }
            return opp;
        }

        /// <summary>
        /// gets the room number of the opened door (adjacent room)
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <param name="door"></param>
        /// <returns></returns>
        private int GetAdjacentRoomNumber(int roomNumber, string door)
        {
            int adjRoom = roomNumber;
            switch (door)
            {
                case "north":
                    adjRoom -= this.databaseConnection.player.GridSize;
                    break;
                case "east":
                    adjRoom += 1;
                    break;
                case "south":
                    adjRoom += this.databaseConnection.player.GridSize;
                    break;
                default:
                    adjRoom -= 1;
                    break;
            }
            return adjRoom;
        }

        /// <summary>
        /// If the user is able to get to the last room, then they are declared a winner
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        private bool GetWinner(int roomNumber)
        {
            int end = (int) Math.Pow(this.databaseConnection.player.GridSize, 2);

            if (end == roomNumber)
            {
                // highscore?
                string msg = "You have won! You answered " + this.databaseConnection.player.CorrectAnswers + " questions correctly";
                string titleCaption = "CONGRATULATIONS!!!";
                MessageBoxButton btn = MessageBoxButton.OK;
                MessageBoxImage img = MessageBoxImage.Exclamation;
                MessageBox.Show(msg, titleCaption, btn, img);
                QuitGame();
                return true;
            }
            return false;
        }

        /// <summary>
        /// checks to see if the user still can open doorways, if not then the game is over because they can't reach the last room
        /// </summary>
        private void ContinueGame()
        {
            List<int> openRooms = this.databaseConnection.player.RoomsOpen;
            List<bool> openPaths = new List<bool>();

            for (int ix = 0; ix < openRooms.Count; ix++)
            {

                foreach(Room room in rooms)
                {
                    if (room.Name.Equals("Room " + openRooms[ix]))
                    {
                        foreach(bool side in room.Sides)
                        {
                            openPaths.Add(side);
                        }
                    }
                }
            }

            if (!openPaths.Contains(true))
            {
                GameOver();
            }
            
        }

        /// <summary>
        /// message sent to the user to let them know that the game is over because they are unable to reach the last room
        /// </summary>
        private void GameOver()
        {
            string msg = "Sorry...there are no more doors to access from your open locations.\n\nThank you for playing!";
            string titleCaption = "Game Over!";
            MessageBoxButton btn = MessageBoxButton.OK;
            MessageBoxImage img = MessageBoxImage.Exclamation;
            MessageBox.Show(msg, titleCaption, btn, img);
            QuitGame();
        }
        
        /// <summary>
        /// asks the user again if they want to quit, allowing them to cancel and save the game if needed
        /// </summary>
        /// <param name="value"></param>
        private void Confirmation(string value)
        {
            string msg = "Are you sure?";
            string titleBar = "Confirm " + value;

            if (this.menuItemSave.IsEnabled == true)
            {
                msg += " This will erase your current game.";
            }

            MessageBoxButton messageBoxButtons = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Question;
            MessageBoxResult messageBoxResult = MessageBox.Show(msg, titleBar, messageBoxButtons, messageBoxImage);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (value.Equals("Quit"))
                {
                    QuitGame();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// method used to build or rebuild the database
        /// </summary>
        /// <remarks>can be used if more questions or categories are added</remarks>
        private void UpdateDatabase()
        {
            new DatabaseBuilder();
        }

        /// <summary>
        /// either selected from the menu or toolbar, creates a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNew(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        /// <summary>
        /// either selected from the menu or toolbar, saves the current game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSave(object sender, RoutedEventArgs e)
        {
            SaveGame();
        }

        /// <summary>
        /// either selected from the menu or toolbar, loads a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            LoadGame();
        }

        /// <summary>
        /// either selected from the menu or toolbar, quits the current game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQuit(object sender, RoutedEventArgs e)
        {
            Confirmation("Quit");   
        }

        /// <summary>
        /// event triggered if the user clicks on the canvas to select a room
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDownCanvas(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition((UIElement)sender);
            GetPosition(point.X, point.Y);
        }

        /// <summary>
        /// either selected from the menu or toolbar, exits the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, RoutedEventArgs e)
        {
            Confirmation("Exit");
        }

        /// <summary>
        /// either selected from the menu or toolbar, gets the instructions for the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInstructions(object sender, RoutedEventArgs e)
        {
            GetInstructions();
        }

        /// <summary>
        /// special button (not enabled - used for testing) that allows the database to be rebuilt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpdateDatabase(object sender, RoutedEventArgs e)
        {
            UpdateDatabase();
        }

        /// <summary>
        /// informs the user about the program 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAbout(object sender, RoutedEventArgs e)
        {
            string msg = "";
            msg += "Author: Jason Campbell\n\n";
            msg += "Project Name: Programming Project 2d - Trivia Maze\n\n";
            msg += "Project Description: The goal of this game is that the user\n";
            msg += "\tgets through the maze of rooms to the exit.\n";
            msg += "\tThe user starts in a maze of rooms with the\n";
            msg += "\tintention of answering questions correctly, in\n";
            msg += "\t order to move to the next room.\n\n";
            msg += "Version: 1.0\n";
            string titleBar = "About";
            MessageBoxButton messageBoxButtons = MessageBoxButton.OK;
            MessageBoxImage messageBoxImage = MessageBoxImage.Question;

            MessageBox.Show(msg, titleBar, messageBoxButtons, messageBoxImage);
        }

    }
}
