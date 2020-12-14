using System.Windows;
using System.Collections.Generic;

namespace campbelljcscd371hw4
{
    /// <summary>
    /// A window that holds the top ten highscores of the game
    /// </summary>
    public partial class HighscoresWindow : Window
    {

        private readonly List<Highscore> highscores;

        /// <summary>
        /// initializes the emptydatabase command as false and calls to display the scores to teh listview
        /// </summary>
        /// <param name="highscores"></param>
        internal HighscoresWindow(List<Highscore> highscores)
        {
            InitializeComponent();

            this.highscores = highscores;
            EmptyDatabase = false;

            if (highscores.Count > 0)
            {
                DisplayScores();
            }           
        }

        /// <summary>
        /// Boolean value that tells whether the Empty database command is called
        /// </summary>
        public bool EmptyDatabase { get; private set; }

        /// <summary>
        /// Clears the Listview holding the top 10 scores
        /// </summary>
        public void ClearScores()
        {
            this.lvHighScores.Items.Clear();
        }

        /// <summary>
        /// Displays the top 10 scores in a listview
        /// </summary>
        public void DisplayScores()
        {
            int placement = 1;
            this.EmptyDatabase = false;

            foreach (Highscore hs in highscores)
            {
                this.lvHighScores.Items.Add(new Highscore
                {
                    Placement = placement + "",
                    Score = hs.Score,
                    Name = hs.Name,
                    Timestamp = hs.Timestamp
                }) ;

                placement++;
            }
        }

        /// <summary>
        /// Event that is triggered by the Empty Database button, and clears the listview and database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReset(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                this.lvHighScores.Items.Clear();
                this.EmptyDatabase = true;
                this.Close();
            }
            
        }

        /// <summary>
        /// Event that is triggered by the Close button, and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
