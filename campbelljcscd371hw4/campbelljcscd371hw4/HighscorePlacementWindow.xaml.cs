using System.Windows;

namespace campbelljcscd371hw4
{
    /// <summary>
    /// Interaction logic for HighscorePlacement.xaml
    /// </summary>
    public partial class HighscorePlacementWindow : Window
    {
        /// <summary>
        /// Window opens when the user gets a highscore
        /// </summary>
        public HighscorePlacementWindow()
        {
            InitializeComponent();
            PlayerName = "";
        }

        /// <summary>
        /// getter and setter for player's name
        /// </summary>
        public string PlayerName { get; private set; }

        /// <summary>
        /// gets the input field, and if empty, assigns a question mark for unknown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            this.PlayerName = tbPlayerName.Text;

            if (PlayerName == null)
            {
                PlayerName = "?";
            }
            
            this.Close();
        }
    }
}
