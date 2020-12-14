using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace campbelljproj2d
{
    /// <summary>
    /// Interaction logic for LoadGameWindow.xaml
    /// Window that allows the user to select a saved game from a potential list
    /// </summary>
    public partial class LoadGameWindow : Window
    {
        List<Player> players;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="players"></param>
        internal LoadGameWindow(List<Player> players)
        {
            InitializeComponent();
            this.players = players;
            this.dgPlayers.ItemsSource = players;
            this.PlayerId = -1;
        }

        public int PlayerId { get; private set; }

        /// <summary>
        /// the user's selection of the saved game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelect(object sender, RoutedEventArgs e)
        {
            Player p = ((Button)sender).DataContext as Player;
            string msg = "Are you sure you want to load \'" + p.Name + "\' game?";
            string titleCaption = "Confirm";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(msg, titleCaption, btn, img);

            if (result == MessageBoxResult.Yes)
            {
                PlayerId = p.Id;
                this.Close();
            } 
        }
    }
}
