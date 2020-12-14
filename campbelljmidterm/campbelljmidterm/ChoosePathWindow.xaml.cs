using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace campbelljmidterm
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ChoosePathWindow : Window
    {
        private string folderPath;
        private string fileExtension;
        private readonly bool darkulaEnabled;
        private readonly Validation validator;

        public ChoosePathWindow(string fp, string fe, bool darkula)
        {
            InitializeComponent();
            this.folderPath = fp;
            this.fileExtension = fe;
            this.tbPath.Text = this.folderPath;
            this.tbExtension.Text = this.fileExtension;
            this.darkulaEnabled = darkula;
            this.validator = new Validation();
            EnableDarkula();
        }

        /// <summary>
        /// if darkula is enabled, then change the appropriate background/foreground colors
        /// </summary>
        private void EnableDarkula()
        {
            if (this.darkulaEnabled)
            {
                this.gridChoosePath.Background = Brushes.DarkGray;
                this.lblInstructions.Background = Brushes.DarkGray;
                this.lblInstructions.Foreground = Brushes.Black;
                this.lblPath.Background = Brushes.DarkGray;
                this.lblPath.Foreground = Brushes.Black;
                this.lblExtension.Background = Brushes.DarkGray;
                this.lblExtension.Foreground = Brushes.Black;
                this.tbPath.Background = Brushes.White;
                this.tbPath.Foreground = Brushes.Black;
                this.tbExtension.Background = Brushes.White;
                this.tbExtension.Foreground = Brushes.Black;
                this.cbExtension.Background = Brushes.White;
                this.cbExtension.Foreground = Brushes.Black;

                this.btnBrowse.Background = Brushes.Black;
                this.btnBrowse.Foreground = Brushes.White;
                this.btnCancel.Background = Brushes.Black;
                this.btnCancel.Foreground = Brushes.White;
                this.btnSubmit.Background = Brushes.Black;
                this.btnSubmit.Foreground = Brushes.White;
            }
            else
            {
                this.gridChoosePath.Background = Brushes.White;
                this.lblInstructions.Background = Brushes.White;
                this.lblInstructions.Foreground = Brushes.Black;
                this.lblPath.Background = Brushes.White;
                this.lblPath.Foreground = Brushes.Black;
                this.lblExtension.Background = Brushes.White;
                this.lblExtension.Foreground = Brushes.Black;
                this.tbPath.Background = Brushes.White;
                this.tbPath.Foreground = Brushes.Black; 
                this.tbExtension.Background = Brushes.White;
                this.tbExtension.Foreground = Brushes.Black;
                this.cbExtension.Background = Brushes.White;
                this.cbExtension.Foreground = Brushes.Black;

                this.btnBrowse.Background = Brushes.LightGray;
                this.btnBrowse.Foreground = Brushes.Black;
                this.btnCancel.Background = Brushes.LightGray;
                this.btnCancel.Foreground = Brushes.Black;
                this.btnSubmit.Background = Brushes.LightGray;
                this.btnSubmit.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// a getter for the folder path name
        /// </summary>
        public string FolderPath
        {
            get { return this.folderPath; }
        }

        /// <summary>
        /// a getter for the file extension name
        /// </summary>
        public string FileExtension
        {
            get {  return this.fileExtension; }
        }

        /// <summary>
        /// displays the operating systems folder browser
        /// <example>
        /// Windows Explorer
        /// </example>
        /// </summary>
        /// <remarks>
        /// FolderBrowserDialog reference: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.folderbrowserdialog?f1url=https%3A%2F%2Fmsdn.microsoft.com%2Fquery%2Fdev16.query%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Windows.Forms.FolderBrowserDialog);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.7.2);k(DevLang-csharp)%26rd%3Dtrue&view=netcore-3.1
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderPath = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };
            
            folderPath.ShowDialog();
            tbPath.Text = folderPath.SelectedPath;
        }

        /// <summary>
        /// event that transfers the folder path and file extension to the main window after validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            if (this.validator.ValidateFolderPath(this.tbPath.Text) && this.validator.ValidateFileExtension(this.tbExtension.Text))
            {

                this.folderPath = this.tbPath.Text;

                if (!(this.tbExtension.Text).Substring(0,2).Equals("*."))
                {
                    this.fileExtension = "*." + this.tbExtension.Text;
                }
                else
                {
                    this.fileExtension = this.tbExtension.Text;
                }

                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid Folder Path\nand/or Invalid File Extension");
                this.tbExtension.Text = "";
            }

        }

        /// <summary>
        /// Event triggered that closes the window and returns to main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
