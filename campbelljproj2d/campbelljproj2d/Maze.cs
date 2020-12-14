using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace campbelljproj2d
{
    /// <summary>
    /// Builds the maze by creating a new list of rooms (doors are logically selected) or from a saved list of rooms
    /// </summary>
    class Maze
    {
        private readonly int rowsAndCols;
        private readonly double canvas_width;  

        /// <summary>
        /// In a new game, this constructor is used to start the sequence of building the maze and it's rooms
        /// </summary>
        /// <param name="rowsAndCols">the grid size</param>
        /// <param name="canvas_size">size of the canvas</param>
        public Maze(int rowsAndCols, double canvas_size)
        {
            this.rowsAndCols = rowsAndCols;
            this.Rooms = new List<Room>();
            this.canvas_width = canvas_size;
            Paths = new List<Path>();

            BuildMaze();
            CreateStart();
            CreateEnd();
        }

        /// <summary>
        /// In a loaded game, this constructor is used to start the sequence of building the maze from it's rooms
        /// </summary>
        /// <param name="rowsAndCols">the grid size</param>
        /// <param name="canvas_size">the size of the canvas</param>
        /// <param name="rooms">a list of rooms taken from the database</param>
        public Maze(int rowsAndCols, double canvas_size, List<Room> rooms)
        {
            this.rowsAndCols = rowsAndCols;
            this.Rooms = rooms;
            this.canvas_width = canvas_size;
            Paths = new List<Path>();

            foreach(Room room in rooms)
            {
                GetPaths(room);
            }

            CreateStart();
            CreateEnd();
        }

        public List<Path> Paths { get; private set; }

        public List<Room> Rooms { get; private set; }

        /// <summary>
        /// Using a grid system (rows and columns), the rooms are built and the doors are logically found
        /// </summary>
        private void BuildMaze()
        {
            double[] xy = new double[2] { 0, 0 };

            for (int row = 0; row < this.rowsAndCols; row++)
            {

                for (int col = 0; col < this.rowsAndCols; col++)
                {
                    bool[] sides = new bool[4]; // north (0), east (1), south (2), west (3)
                    string name = "Room " + ((row * this.rowsAndCols) + (col + 1));

                    if (row == 0) // north (no door)
                    {
                        sides[0] = false;       // north
                        sides[2] = true;        // south

                        if (col == 0) // west (no door)
                        {
                            sides[3] = false;   // west
                            sides[1] = true;    // east
                        }
                        else if (col == this.rowsAndCols - 1) // east (no door)
                        {
                            sides[3] = true;    // west
                            sides[1] = false;   // east
                        }
                        else
                        {
                            sides[1] = true;    // east
                            sides[3] = true;    // west
                        }
                    }
                    else if (row == this.rowsAndCols - 1) // south (no door)
                    {
                        sides[0] = true;        // north
                        sides[2] = false;       // south

                        if (col == 0) // west (no door)
                        {
                            sides[3] = false;   // west
                            sides[1] = true;    // east
                        }
                        else if (col == this.rowsAndCols - 1) // east (no door)
                        {
                            sides[3] = true;    // west
                            sides[1] = false;   // east
                        }
                        else
                        {
                            sides[1] = true;    // east
                            sides[3] = true;    // west
                        }
                    }
                    else // all doors
                    {
                        sides[0] = true;        // north
                        sides[2] = true;        // south

                        if (col == 0) // west (no door)
                        {
                            sides[3] = false;   // west
                            sides[1] = true;    // east
                        }
                        else if (col == this.rowsAndCols - 1) // east (no door)
                        {
                            sides[3] = true;    // west
                            sides[1] = false;   // east
                        }
                        else
                        {
                            sides[1] = true;    // east
                            sides[3] = true;    // west
                        }
                    }
                    
                    if (col == 0)
                    {
                        xy[0] = 0;
                    }
                    else
                    {
                        xy[0] += (this.canvas_width / this.rowsAndCols);
                    }

                    Room room = new Room(sides, this.canvas_width, this.rowsAndCols, xy, name);
                    GetPaths(room);
                    this.Rooms.Add(room);
    
                }
                
                xy[1] += (this.canvas_width / this.rowsAndCols);
                
            }
        }

        /// <summary>
        /// For the drawing portion of the maze, a path is created from the room
        /// </summary>
        /// <param name="room">the room</param>
        public void GetPaths(Room room)
        {
            foreach(Path path in room.Paths)
            {
                Paths.Add(path);
            }
        }

        /// <summary>
        /// places a textblock in each room, labeling it with its name
        /// <example>Room 9</example>
        /// </summary>
        /// <param name="roomNumber">the room number associated with the grid</param>
        /// <returns>the textblock</returns>
        public TextBlock LabelRooms(int roomNumber)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = "Room " + roomNumber,
                TextAlignment = TextAlignment.Center,
                Width = 50,
                Height = 20,
            };

            return textBlock;
        }

        /// <summary>
        /// places a textblock just under the first room number, labeling it the start
        /// </summary>
        /// <returns>the textblock</returns>
        public TextBlock CreateStart()
        {
            TextBlock textBlock = new TextBlock
            {
                Text = "Start",
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.Green,
                Width = 30,
                Height = 18,
            };

            return textBlock;
        }

        /// <summary>
        /// places a textblock just under the last room number, labeling it the end
        /// </summary>
        /// <returns>the textblock</returns>
        public TextBlock CreateEnd()
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = "End",
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.Purple,
                Width = 30,
                Height = 18,
            };

            return textBlock;
        }

        /// <summary>
        /// creates a textblock under the room number, labeling it Open for the user
        /// </summary>
        /// <returns></returns>
        public TextBlock CreateCurrentPosition()
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = "Open",
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.Blue,
                Width = 30,
                Height = 18,
            };

            return textBlock;
        }
    }
}
