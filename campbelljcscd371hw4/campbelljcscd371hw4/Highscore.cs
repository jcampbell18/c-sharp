namespace campbelljcscd371hw4
{
    /// <summary>
    /// Class that contains information on a highscore such as
    ///     Placement (1-10), Player's name, Score, and Date/Time
    /// </summary>
    class Highscore
    {
        public string Placement { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string Timestamp { get; set; }
    }
}
