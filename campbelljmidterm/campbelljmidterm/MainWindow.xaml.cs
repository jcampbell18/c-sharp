/*
 * Author: Jason Campbell
 * Midterm Project: Filewatcher GUI
 * 
 * Filewatcher is  a window based program that allows the user to monitor events on files 
 * with specific extension types.  Information will be displayed on the window as it occurs.  
 * The user will also have the option of writing the information to a SQLite database and/or
 * read the data on the database.
 * 
 * Complete documentation found below
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Media;

namespace campbelljmidterm
{
    /// <summary>
    /// the Main Window that connects all events (buttons or links) and initializes the database connection
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSystemWatcher watcher;
        private ChoosePathWindow choosePathWindow;
        private QueryDatabaseWindow queryDatabaseWindow;
        private readonly DatabaseConnection databaseConnection;
        private readonly List<DatabaseData> newData;
        private readonly Validation validator;
        private string folderPath;
        private string fileExtension;

        /// <summary>
        /// Constructor that initializes the components, sets up the status bar, and initially toggles all buttons/links
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            MessageBox.Show("To start monitoring, you must choose:\n\'Choose Filewatcher Path/Ext\'\nfrom either the Menu or the Toolbar", "Important!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            this.folderPath = this.tbPath.Text;
            this.fileExtension = this.tbExtension.Text;
            this.databaseConnection = new DatabaseConnection();
            this.newData = new List<DatabaseData>();
            this.validator = new Validation();

            if (!this.validator.ValidateFolderPath(this.folderPath) && !this.validator.ValidateFileExtension(this.fileExtension))
            {
                this.sbMonitorStatus.Content = "Invalid Directory AND File Extension";
                this.btnStart.IsEnabled = false;
                this.btnStop.IsEnabled = false;
                this.menuItemStartFW.IsEnabled = false;
                this.menuItemStopFW.IsEnabled = false;
            }
            else if (!this.validator.ValidateFolderPath(this.folderPath))
            {
                this.sbMonitorStatus.Content = "Invalid Directory";
                this.btnStart.IsEnabled = false;
                this.btnStop.IsEnabled = false;
                this.menuItemStartFW.IsEnabled = false;
                this.menuItemStopFW.IsEnabled = false;
            }
            else if (!this.validator.ValidateFileExtension(this.fileExtension))
            {
                this.sbMonitorStatus.Content = "Invalid File Extension";
                this.btnStart.IsEnabled = false;
                this.btnStop.IsEnabled = false;
                this.menuItemStartFW.IsEnabled = false;
                this.menuItemStopFW.IsEnabled = false;
            }
            else
            {
                this.sbMonitorStatus.Content = "Ready";
                this.btnStart.IsEnabled = true;
                this.menuItemStartFW.IsEnabled = true;
            }
        }

        /// <summary>
        /// inner class for the listview
        /// </summary>
        public class FileWatcherData
        {
            public string Timestamp { get; set; }
            public string Name { get; set; }
            public string Action { get; set; }
            public string AbsolutePath { get; set; }
        }

        /// <summary>
        /// gets the current data and time
        /// </summary>
        /// <returns>timestamp string</returns>
        private static string GetTimeStamp()
        {
            DateTime timestamp = DateTime.Now;
            return timestamp.ToString("g");
        }

        /// <summary>
        /// before exiting the program, the user is asked to save the results of the monitoring 
        /// to the database, or just confirming that user will exit program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, RoutedEventArgs e)
        {
            string msg;
            string titleBar;
            MessageBoxButton messageBoxButtons = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Question;

            if (this.lvData.Items.Count > 0)
            {
                msg = "Before exiting the program, would you like to write the monitored results to the Database?";
                titleBar = "Save Results to Database";
                MessageBoxResult messageBoxResult = MessageBox.Show(msg, titleBar, messageBoxButtons, messageBoxImage);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    WriteToDatabase();
                }
                
                Environment.Exit(0);

            }
            else
            {
                msg = "Are you sure?";
                titleBar = "Confirm Exit";
                MessageBoxResult messageBoxResult = MessageBox.Show(msg, titleBar, messageBoxButtons, messageBoxImage);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                }
                
            }
        }

        /// <summary>
        /// toggles between light and darkula color themes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeTheme(object sender, RoutedEventArgs e)
        {
            if (this.menuItemDarkula.IsChecked)
            {
                this.mainGrid.Background = Brushes.DarkGray;
                this.mainMenu.Background = Brushes.DarkGray;
                this.mainMenu.Foreground = Brushes.Black;

                this.menuItemFile.Foreground = Brushes.Black;
                this.menuItemChoosePath.Foreground = Brushes.Black;
                this.menuItemStartFW.Foreground = Brushes.Black;
                this.menuItemStopFW.Foreground = Brushes.Black;
                this.menuItemReadDB.Foreground = Brushes.Black;
                this.menuItemWriteDB.Foreground = Brushes.Black;
                this.menuItemExit.Foreground = Brushes.Black;
                this.menuItemEdit.Foreground = Brushes.Black;
                this.menuItemToolbarShortcuts.Foreground = Brushes.Black;
                this.menuItemDarkula.Foreground = Brushes.Black;
                this.menuItemHelp.Foreground = Brushes.Black;
                this.menuItemAbout.Foreground = Brushes.Black;

                this.toolBar.Background = Brushes.DarkGray;
                this.toolBar.Foreground = Brushes.White;

                this.statusBar.Background = Brushes.Black;
                this.statusBar.Foreground = Brushes.White;

                this.lblPath.Background = Brushes.DarkGray;
                this.lblPath.Foreground = Brushes.Black;
                this.lblExtension.Background = Brushes.DarkGray;
                this.lblExtension.Foreground = Brushes.Black;
                this.tbPath.Background = Brushes.White;
                this.tbPath.Foreground = Brushes.Black;
                this.tbExtension.Background = Brushes.White;
                this.tbExtension.Foreground = Brushes.Black;
            }
            else
            {
                this.mainGrid.Background = Brushes.LightGray;
                this.mainMenu.Background = Brushes.White;
                this.mainMenu.Foreground = Brushes.Black;

                this.menuItemFile.Foreground = Brushes.Black;
                this.menuItemChoosePath.Foreground = Brushes.Black;
                this.menuItemStartFW.Foreground = Brushes.Black;
                this.menuItemStopFW.Foreground = Brushes.Black;
                this.menuItemReadDB.Foreground = Brushes.Black;
                this.menuItemWriteDB.Foreground = Brushes.Black;
                this.menuItemExit.Foreground = Brushes.Black;
                this.menuItemEdit.Foreground = Brushes.Black;
                this.menuItemToolbarShortcuts.Foreground = Brushes.Black;
                this.menuItemDarkula.Foreground = Brushes.Black;
                this.menuItemHelp.Foreground = Brushes.Black;
                this.menuItemAbout.Foreground = Brushes.Black;

                this.toolBar.Background = Brushes.White;
                this.toolBar.Foreground = Brushes.Black;

                this.statusBar.Background = Brushes.LightGray;
                this.statusBar.Foreground = Brushes.Black;

                this.lblPath.Background = Brushes.LightGray;
                this.lblPath.Foreground = Brushes.Black;
                this.lblExtension.Background = Brushes.LightGray;
                this.lblExtension.Foreground = Brushes.Black;
                this.tbPath.Background = Brushes.White;
                this.tbPath.Foreground = Brushes.Black;
                this.tbExtension.Background = Brushes.White;
                this.tbExtension.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// event that triggers a shortcut for disabling/enabling the toolbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnToolbarShortcuts(object sender, RoutedEventArgs e)
        {
            if (this.menuItemToolbarShortcuts.IsChecked)
            {
                this.menuItemToolbarShortcuts.IsChecked = false;
                this.toolBar.Visibility = Visibility.Hidden;

                // if toolbar is not present, then move the extension and folder path up to reduce white space
                this.lblPath.Margin = new Thickness(10, 35, 0, 0);
                this.tbPath.Margin = new Thickness(103, 35, 0, 0);
                this.lblExtension.Margin = new Thickness(501, 35, 0, 0);
                this.tbExtension.Margin = new Thickness(569, 35, 0, 0);
                this.lvData.Margin = new Thickness(20, 70, 0, 0);
                this.lvData.Height = 455;
            } 
            else
            {
                this.menuItemToolbarShortcuts.IsChecked = true;
                this.toolBar.Visibility = Visibility.Visible;

                // if toolbar is present, then keep the extension and folder path at specific height and position
                this.lblPath.Margin = new Thickness(10, 65, 0, 0);
                this.tbPath.Margin = new Thickness(103, 65, 0, 0);
                this.lblExtension.Margin = new Thickness(501, 65, 0, 0);
                this.tbExtension.Margin = new Thickness(569, 65, 0, 0);
                this.lvData.Margin = new Thickness(20, 100, 0, 0);
                this.lvData.Height = 425;
            }
        }

        /// <summary>
        /// event that triggers a new window where the user can select the folder 
        /// path and file extension
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChoosePath(object sender, RoutedEventArgs e)
        {
            this.choosePathWindow = new ChoosePathWindow(this.folderPath, this.fileExtension, this.menuItemDarkula.IsChecked);
            while (this.choosePathWindow.ShowDialog() == true);

            this.folderPath = choosePathWindow.FolderPath;
            this.tbPath.Text = this.folderPath;
            this.fileExtension = choosePathWindow.FileExtension;
            this.tbExtension.Text = this.fileExtension;

            if (this.folderPath.Length > 0)
            {
                this.btnStart.IsEnabled = true;
                this.menuItemStartFW.IsEnabled = true;
                this.sbMonitorStatus.Content = "Ready";
            }
            else
            {
                this.btnStart.IsEnabled = false;
                this.menuItemStartFW.IsEnabled = false;
                this.sbMonitorStatus.Content = "Choose a folder and file extension to enable start button";
            }
            
        }

        /// <summary>
        /// event that triggers a new window where the user can view the database records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReadDatabase(object sender, RoutedEventArgs e)
        {
            this.queryDatabaseWindow = new QueryDatabaseWindow(this.databaseConnection.QueryDatabase(), this.menuItemDarkula.IsChecked); 
            while (this.queryDatabaseWindow.ShowDialog() == true);

            if (this.queryDatabaseWindow.EmptyDatabase)
            {
                this.databaseConnection.EmptyDatabase();
            }
            
        }

        /// <summary>
        /// event that available only after the folder path has been monitored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWriteDatabase(object sender, RoutedEventArgs e)
        {
            WriteToDatabase();
        }

        /// <summary>
        /// takes the new monitored list and inserts into database
        /// </summary>
        private void WriteToDatabase()
        {
            if (this.lvData.Items.Count > 0)
            {
                this.databaseConnection.WriteToDatabase(this.newData);
            }

            MessageBox.Show("Successful!", "Writing to Database", MessageBoxButton.OK, MessageBoxImage.Information);
            this.newData.Clear();
            this.lvData.Items.Clear();
            this.btnRead.IsEnabled = true;
            this.btnWrite.IsEnabled = false;
            this.menuItemReadDB.IsEnabled = true;
            this.menuItemWriteDB.IsEnabled = false;
        }

        /// <summary>
        /// event that is triggered when the user selects About, and displays information
        /// about the program in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAbout(object sender, RoutedEventArgs e)
        {
            string str = "";
            str += "Author: Jason Campbell\n\n";
            str += "Project Name: Midterm Project - Filewatcher GUI with WPF\n\n";
            str += "Project Description: Filewatcher is  a window based program\n";
            str += "\tthat allows the user to monitor events on files\n";
            str += "\twith specific extension types. Information will\n";
            str += "\tbe displayed onthe window as it occurs. The user\n";
            str += "\twill also have the option of writing the information\n";
            str += "\tto a SQLite database.\n\n";
            str += "Version: 1.0\n";
            MessageBox.Show(str, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// starts the filewatcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void OnStart(object sender, RoutedEventArgs e)
        {
            if (this.validator.ValidateFolderPath(this.folderPath) && this.validator.ValidateFileExtension(this.fileExtension))
            {
                this.sbMonitorStatus.Content = "Running";
                this.btnStart.IsEnabled = false;
                this.menuItemStartFW.IsEnabled = false;
                this.btnStop.IsEnabled = true;
                this.menuItemStopFW.IsEnabled = true;
                this.btnRead.IsEnabled = false;
                this.menuItemReadDB.IsEnabled = false;
                this.btnWrite.IsEnabled = false;
                this.menuItemWriteDB.IsEnabled = false;
                this.menuItemChoosePath.IsEnabled = false;
                this.btnChangePath.IsEnabled = false;


                watcher = new FileSystemWatcher(this.folderPath)
                {
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.LastAccess
                                            | NotifyFilters.LastWrite
                                            | NotifyFilters.FileName
                                            | NotifyFilters.DirectoryName,

                    Filter = this.fileExtension
                };
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;
                watcher.EnableRaisingEvents = true;
            }
            else
            {
                this.sbMonitorStatus.Content = "Invalid Folder and/or File Extension";
                this.btnStart.IsEnabled = false;
                this.menuItemStartFW.IsEnabled = false;
            }
        }

        /// <summary>
        /// events triggers by the filewatcher when a folder or file has been changed
        /// <example>
        /// file or folder is created, deleted, or edited
        /// </example>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string timestamp = GetTimeStamp();

            newData.Add(new DatabaseData
            {
                Timestamp = timestamp,
                Name = e.Name,
                Action = e.ChangeType.ToString(),
                AbsolutePath = e.FullPath
            });

            Dispatcher.BeginInvoke(
               (Action)(() =>
               {
                   this.lvData.Items.Add(new FileWatcherData { Timestamp = timestamp, Name = e.Name, Action = e.ChangeType.ToString(), AbsolutePath = e.FullPath });
               })
            );
        }

        /// <summary>
        /// events triggers by the filewatcher when a folder or file has been renamed
        /// and toggles all connected buttons/links
        /// <example>
        /// file or folder is renamed
        /// </example>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            string timestamp = GetTimeStamp();
            string action = "Renamed";
            //string rename = e.OldFullPath + " --> " + e.FullPath;

            newData.Add(new DatabaseData
            {
                Timestamp = timestamp,
                Name = e.Name,
                Action = e.ChangeType.ToString(),
                AbsolutePath = e.FullPath
            });

            Dispatcher.BeginInvoke(
               (Action)(() =>
               {
                   this.lvData.Items.Add(new FileWatcherData { Timestamp = timestamp, Name = e.Name, Action = action, AbsolutePath = e.FullPath });
               })
            );
        }

        /// <summary>
        /// stops the filewatcher and toggles all connected buttons/links
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStop(object sender, RoutedEventArgs e)
        {
            watcher.EnableRaisingEvents = false;

            this.sbMonitorStatus.Content = "Stopped";
            this.btnStart.IsEnabled = true;
            this.menuItemStartFW.IsEnabled = true;
            this.btnStop.IsEnabled = false;
            this.menuItemStopFW.IsEnabled = false;
            this.btnRead.IsEnabled = false;
            this.menuItemReadDB.IsEnabled = false;
            this.btnWrite.IsEnabled = true;
            this.menuItemWriteDB.IsEnabled = true;
            this.menuItemChoosePath.IsEnabled = true;
            this.btnChangePath.IsEnabled = true;
        }
    }
}
