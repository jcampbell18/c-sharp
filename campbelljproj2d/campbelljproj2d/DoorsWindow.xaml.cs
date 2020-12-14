using System.Windows;

namespace campbelljproj2d
{
    /// <summary>
    /// Interaction logic for DoorsWindow.xaml
    /// Window that allows the user to select the doorway of the open room
    /// </summary>
    public partial class DoorsWindow : Window
    {
        private bool[] sides;

        /// <summary>
        /// Constructor that takes in the reported sides (true for closed door, false for wall/locked or open
        /// </summary>
        /// <param name="sides"></param>
        public DoorsWindow(bool[] sides)
        {
            InitializeComponent();
            this.sides = sides;
            this.DoorSelected = "";
            this.Submit = false;
            DisableDoors();
        }

        public string DoorSelected { get; set; }

        public bool Submit { get; set; }

        /// <summary>
        /// If the side is false, then the choice is disabled from the menu
        /// <example>Room 1 in the upper-left hand corner only has a south and east door, the rest are walls. Only choices that should show are south and east (if closed)</example>
        /// </summary>
        private void DisableDoors()
        {
            rbNorth.IsEnabled = sides[0];
            rbEast.IsEnabled = sides[1];
            rbSouth.IsEnabled = sides[2];
            rbWest.IsEnabled = sides[3];
        }

        /// <summary>
        /// takes the user's door selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            if (this.rbNorth.IsChecked == true)
            {
                DoorSelected = "north";
                Submit = true;
            }
            else if (this.rbEast.IsChecked == true)
            {
                DoorSelected = "east";
                Submit = true;
            }
            else if (this.rbSouth.IsChecked == true)
            {
                DoorSelected = "south";
                Submit = true;
            }
            else if (this.rbWest.IsChecked == true)
            {
                DoorSelected = "west";
                Submit = true;
            }
            else
            {
                Submit = false;
            }

            this.Close();
        }

        /// <summary>
        /// a way for the user to back out of door selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
