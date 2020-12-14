using System;
using System.Windows;

namespace campbelljproj2d
{

    public partial class PlayerWindow : Window
    {

        /// <summary>
        /// Window for a new user, asking for Name, Category and Grid Size
        /// </summary>
        public PlayerWindow()
        {
            InitializeComponent();
            tbPlayerName.Focus();
        }

        public string PlayerName { get; private set; }

        public string Category { get; private set; }

        public int MazeSize { get; private set; }

        /// <summary>
        /// Verifies on submit that the user entered correct information. If so, closes the window - if not, user needs to put their name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            PlayerName = this.tbPlayerName.Text;
            Category = this.cbCategory.Text;
            MazeSize = Int32.Parse((this.cbMazeSize.Text).Substring(0,1));

            if (PlayerName == null || PlayerName.Length < 3)
            {
                string msg = "Please enter a valid name\n(between 3 and 15 characters)";
                string titleCaption = "Invalid Player name";
                MessageBoxButton messageBoxButton = MessageBoxButton.OK;
                MessageBoxImage messageBoxImage = MessageBoxImage.Error;
                MessageBox.Show(msg, titleCaption, messageBoxButton, messageBoxImage);
            }
            else
            {
                this.Close();
            }
        }
    }
   
}
