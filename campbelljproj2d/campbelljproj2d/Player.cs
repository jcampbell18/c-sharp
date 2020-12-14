using System.Collections.Generic;

namespace campbelljproj2d
{
    /// <summary>
    /// Player class represents the SQLite table Players
    /// </summary>
    class Player
    {
        /// <summary>
        /// initializes empty lists
        /// </summary>
        public Player()
        {
            this.Rooms = new List<int>();
            this.RoomsOpen = new List<int>
            {
                1
            };
            this.QuestionsAnswered = new List<int>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public int GridSize { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public int MazeId { get; set; }

        public List<int> QuestionsAnswered { get; set; }

        public List<int> Rooms { get; set; }

        public List<int> RoomsOpen { get; set; }

        public int CurrentRoomId { get; set; }

        public string CurrentRoomName { get; set; }

        public int CorrectAnswers { get; set; }
    }
}
