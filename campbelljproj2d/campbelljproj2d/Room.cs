using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace campbelljproj2d
{
    /// <summary>
    /// Each room is unique in a maze, attributes of the room are declared here
    /// </summary>
    class Room
    {
        private readonly SolidColorBrush WALL;
        private readonly SolidColorBrush CLOSED;
        private readonly SolidColorBrush OPEN;
        private readonly SolidColorBrush LOCKED;
        private readonly SolidColorBrush[] STATUS = new SolidColorBrush[4] { Brushes.Black, Brushes.Red, Brushes.Green, Brushes.Black };
        private readonly double canvas_width = 720;
        private readonly double length;
        private readonly double thick;
        private readonly int gridSize;
        private readonly int rectPerSide = 3;
        private readonly double actualLength;

        /// <summary>
        /// constructor for a new game. the rooms are created from the set properties from the user and canvas
        /// </summary>
        /// <param name="sides"></param>
        /// <param name="canvas_width"></param>
        /// <param name="gridSize"></param>
        /// <param name="xy"></param>
        /// <param name="name"></param>
        public Room(bool[] sides, double canvas_width, int gridSize, double[] xy, string name)
        {
            this.WALL = STATUS[0];
            this.CLOSED = STATUS[1];
            this.OPEN = STATUS[2];
            this.LOCKED = STATUS[3];

            this.canvas_width = canvas_width;
            this.gridSize = gridSize;
            this.length = this.canvas_width / this.gridSize;
            this.thick = this.length / 5;
            this.actualLength = this.length / rectPerSide;
            this.Name = name;
            this.Sides = sides; // north, east, south, west
            this.SideStatus = new string[4] {
                sides[0] ? "closed" : "wall",
                sides[1] ? "closed" : "wall",
                sides[2] ? "closed" : "wall",
                sides[3] ? "closed" : "wall"
            };
            
            this.Coordinates = new Point {
                X = xy[0],
                Y = xy[1]
            };

            Paths = new List<Path>();

            BuildRoom();
        }
        
        /// <summary>
        /// constructor for a loaded game, the rooms are created based on the data from the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roomName"></param>
        /// <param name="coordinates"></param>
        /// <param name="sides"></param>
        /// <param name="sideStatus"></param>
        /// <param name="gridSize"></param>
        public Room(int id, string roomName, Point coordinates, bool[] sides, string[] sideStatus, int gridSize)
        {
            this.WALL = STATUS[0];
            this.CLOSED = STATUS[1];
            this.OPEN = STATUS[2];
            this.LOCKED = STATUS[3];

            this.Id = id;
            this.Name = roomName;
            this.Coordinates = coordinates;
            this.Sides = sides;
            this.SideStatus = sideStatus;

            this.gridSize = gridSize;
            this.length = this.canvas_width / this.gridSize;
            this.thick = this.length / 5;
            this.actualLength = this.length / rectPerSide;

            Paths = new List<Path>();

            BuildRoom();
        }
        

        public List<Path> Paths { get; private set; }

        public int Id { get; set; }

        public string Name { get; private set; }

        public Point Coordinates { get; private set; }

        public bool[] Sides { get; private set; }

        public string[] SideStatus { get; set; }
      
        /// <summary>
        /// changes the selected door from true to false
        /// </summary>
        /// <remarks>this can be from answering a question either right or wrong, but makes the door no longer accessible</remarks>
        /// <param name="door"></param>
        public void ChangeSide(string door)
        {
            switch (door)
            {   
                case "north":
                    Sides[0] = false; 
                    break;
                case "east":
                    Sides[1] = false;
                    break;
                case "south":
                    Sides[2] = false;
                    break;
                default: // west
                    Sides[3] = false;
                    break;
            }
        }

        /// <summary>
        /// draws a new rectangle (door) over the existing one to represent either a locked or open door
        /// </summary>
        /// <param name="door"></param>
        /// <param name="answer">if the answer is right (true) or not (false)</param>
        /// <returns></returns>
        public Path ReDrawDoor(string door, bool answer)
        {
            double startX = this.Coordinates.X;
            double startY = this.Coordinates.Y;
            int index;
            Path p;

            switch(door)
            {
                case "north":
                    startX += length / 3;
                    index = 0;
                    break;
                case "east":
                    startX += length - thick;
                    startY += length / 3;
                    index = 1;
                    break;
                case "south":
                    startX += length / 3;
                    startY += length - thick;
                    index = 2;
                    break;
                default: // west
                    startY += length / 3;
                    index = 3;
                    break;
            }

            RectangleGeometry rect = new RectangleGeometry();

            if (door.Equals("north") || door.Equals("south"))
            {
                rect.Rect = new Rect(startX, startY, this.actualLength, this.thick);
            }
            else
            {
                rect.Rect = new Rect(startX, startY, this.thick, this.actualLength);
            }
            
            p = new Path
            {
                Fill = answer ? OPEN : LOCKED,
                Data = rect,
            };

            //Paths.Add(p);
            this.SideStatus[index] = answer ? "open" : "locked";
            return p;
        }

        /// <summary>
        /// Sequence of methods that start in clockwise motion (north, east, south, west)
        /// </summary>
        private void BuildRoom()
        {
            BuildCaps(this.Coordinates.X, this.Coordinates.Y, this.Sides[0], 0); // north
            BuildSides(this.Coordinates.X + this.length - this.thick, this.Coordinates.Y + this.thick, this.Sides[1], 1); // east
            BuildCaps(this.Coordinates.X, this.Coordinates.Y + this.length - this.thick, this.Sides[2], 2); // south
            BuildSides(this.Coordinates.X, this.Coordinates.Y + this.thick, this.Sides[3], 3); // west
        }

        /// <summary>
        /// creates 3 rectangles per side which is either the north or south part of the side
        /// </summary>
        /// <param name="x">x coordinate/point that is the upper-left part of the rectangle</param>
        /// <param name="y">y coordinate/point that is the upper-left part of the rectangle</param>
        /// <param name="side"></param>
        /// <param name="index"></param>
        private void BuildCaps(double x, double y, bool side, int index) // north and south
        {
            double startX = x;

            for (int ix = 0; ix < this.rectPerSide; ix++)
            {
                RectangleGeometry rect = new RectangleGeometry
                {
                    Rect = new Rect(startX, y, this.actualLength, this.thick)
                };

                Path path = new Path();

                if (ix != 1)
                {
                    path.Fill = Brushes.Black;
                }
                else
                {
                    switch(this.SideStatus[index])
                    {
                        case "wall":
                            path.Fill = WALL;
                            break;
                        case "closed":
                            path.Fill = CLOSED;
                            break;
                        case "open":
                            path.Fill = OPEN;
                            break;
                        default:
                            path.Fill = LOCKED;
                            break;
                    }
                }

                path.Data = rect;
                Paths.Add(path);

                startX += this.actualLength;
            }

        }

        /// <summary>
        /// creates 3 rectangles per side which is either the east or west part of the side
        /// </summary>
        /// <param name="x">x coordinate/point that is the upper-left part of the rectangle</param>
        /// <param name="y">y coordinate/point that is the upper-left part of the rectangle</param>
        /// <param name="side"></param>
        /// <param name="index"></param>
        private void BuildSides(double x, double y, bool side, int index) // east and west
        {
            double startY = y;
            double shortLength = this.actualLength - this.thick;

            for (int ix = 0; ix < this.rectPerSide; ix++)
            { 
                RectangleGeometry rect = new RectangleGeometry
                {
                    Rect = new Rect(x, startY, this.thick, ix == 1 ? this.actualLength : shortLength)
                };

                Path path = new Path();

                if (ix != 1)
                {
                    path.Fill = Brushes.Black;
                }
                else
                {
                    switch (this.SideStatus[index])
                    {
                        case "wall":
                            path.Fill = WALL;
                            break;
                        case "closed":
                            path.Fill = CLOSED;
                            break;
                        case "open":
                            path.Fill = OPEN;
                            break;
                        default:
                            path.Fill = LOCKED;
                            break;
                    }
                }

                path.Data = rect;
                Paths.Add(path);

                startY += ix == 1 ? this.actualLength : shortLength;
            }

        }

    }
}
