using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace campbelljmidterm
{
    /// <summary>
    /// Interaction logic for QueryDatabaseWindow.xaml
    /// </summary>
    public partial class QueryDatabaseWindow : Window
    {
        private readonly List<DatabaseData> databaseData;
        private readonly bool darkulaEnabled;

        /// <summary>
        /// Query and display results from Database
        /// </summary>
        public QueryDatabaseWindow(List<DatabaseData> databaseData, bool darkula)
        {
            InitializeComponent();

            this.databaseData = databaseData;
            this.EmptyDatabase = false;
            this.darkulaEnabled = darkula;

            if (databaseData.Count == 0)
            {
                this.btnApply.IsEnabled = false;
                this.btnEmptyDB.IsEnabled = false;
            }
            else
            {
                this.btnApply.IsEnabled = true;
                this.btnEmptyDB.IsEnabled = true;
            }

            EnableDarkula();
            GetDatabaseResults();
        }

        /// <summary>
        /// if darkula is enabled, then change the appropriate background/foreground colors
        /// </summary>
        private void EnableDarkula()
        {
            if (this.darkulaEnabled)
            {
                this.gridQueryDatabase.Background = Brushes.DarkGray;
                this.lvDatabaseResults.Background = Brushes.White;
                this.lvDatabaseResults.Foreground = Brushes.Black;

                this.btnApply.Background = Brushes.Black;
                this.btnApply.Foreground = Brushes.White;
                this.btnCancel.Background = Brushes.Black;
                this.btnCancel.Foreground = Brushes.White;
                this.btnEmptyDB.Background = Brushes.Black;
                this.btnEmptyDB.Foreground = Brushes.White;
            }
            else
            {
                this.gridQueryDatabase.Background = Brushes.LightGray;
                this.lvDatabaseResults.Background = Brushes.White;
                this.lvDatabaseResults.Foreground = Brushes.Black;

                this.btnApply.Background = Brushes.White;
                this.btnApply.Foreground = Brushes.Black;
                this.btnCancel.Background = Brushes.White;
                this.btnCancel.Foreground = Brushes.Black;
                this.btnEmptyDB.Background = Brushes.White;
                this.btnEmptyDB.Foreground = Brushes.Black;
            }
        }

        public bool EmptyDatabase { get; private set; }

        /// <summary>
        /// Prints out results of the database to the listview
        /// </summary>
        private void GetDatabaseResults()
        {
            this.EmptyDatabase = false;

            foreach (DatabaseData row in databaseData)
            {
                this.lvDatabaseResults.Items.Add(new DatabaseData
                {
                    Id = row.Id,
                    Timestamp = row.Timestamp,
                    Name = row.Name,
                    Action = row.Action,
                    AbsolutePath = row.AbsolutePath
                });
            }

        }

        /// <summary>
        /// checks the textbox for sql injection by seeing if the string contains any sql keywords
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns>returns true if sql keyword is found</returns>
        private bool CheckForSQLKeywords(string fileExt)
        {
            string[] keywords =
            {
                "select",
                "insert",
                "create",
                "update",
                "alter",
                "delete",
                "drop"
            };

            if (this.tbExtension.Text != null && fileExt.Length < 3)
            {
                foreach (string keyword in keywords)
                {
                    if (this.tbExtension.Text.ToLower().Contains(keyword))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// This method takes the user input, and filters the current database results
        /// </summary>
        /// <param name="fileExt">the user-specified file extension</param>
        private void SearchByFileExt(string fileExt)
        {
            if (!fileExt.Equals("*.*"))
            {
                string tempFileExt;

                if (fileExt[0].Equals('*') && fileExt[1].Equals('.'))
                {
                    tempFileExt = "";

                    for (int ix = 2; ix < fileExt.Length; ix++)
                    {
                        if (fileExt[ix].Equals(" "))
                        {
                            tempFileExt += "_";
                        }
                        else
                        {
                            tempFileExt += fileExt[ix];
                        }

                    }

                    this.tbExtension.Text = tempFileExt;
                }
                else
                {
                    tempFileExt = "";

                    for (int ix = 0; ix < fileExt.Length; ix++)
                    {
                        if (fileExt[ix].Equals(" "))
                        {
                            tempFileExt += "_";
                        }
                        else
                        {
                            tempFileExt += fileExt[ix];
                        }

                    }

                    this.tbExtension.Text = tempFileExt;
                }

                List<DatabaseData> tempList = this.databaseData;

                foreach (DatabaseData tempItem in tempList)
                {

                    if ((tempItem.Name.ToLower()).Contains((tempFileExt.ToLower())))
                    {
                        this.lvDatabaseResults.Items.Add(new DatabaseData
                        {
                            Id = tempItem.Id,
                            Timestamp = tempItem.Timestamp,
                            Name = tempItem.Name,
                            Action = tempItem.Action,
                            AbsolutePath = tempItem.AbsolutePath
                        });
                    }
                }


            }
            else
            {
                List<DatabaseData> tempList = this.databaseData;

                foreach (DatabaseData tempItem in tempList)
                {
                    this.lvDatabaseResults.Items.Add(new DatabaseData
                    {
                        Id = tempItem.Id,
                        Timestamp = tempItem.Timestamp,
                        Name = tempItem.Name,
                        Action = tempItem.Action,
                        AbsolutePath = tempItem.AbsolutePath
                    });
                }
            }
        }

        /// <summary>
        /// Event that is triggered by the Empty Database button, and clears the listview and database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEmptyDatabase(object sender, RoutedEventArgs e)
        {
            this.EmptyDatabase = true;
            this.lvDatabaseResults.Items.Clear();
            this.btnApply.IsEnabled = false;
            this.btnEmptyDB.IsEnabled = false;
        }

        /// <summary>
        /// event that closes the windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">unused but contains name and event</param>
        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event that is triggered when the apply button for the search by file extension is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApply(object sender, RoutedEventArgs e)
        {
            string fileExt = this.tbExtension.Text;

            this.tbExtension.Text = "";
            this.lvDatabaseResults.Items.Clear();

            if (CheckForSQLKeywords(fileExt))
            {
                MessageBox.Show("SQL Injection Detected!\n\nTry a different search term", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (fileExt != null && fileExt.Length <= 2)
            {
                MessageBox.Show("Search term not recognized\n\nDefaulting to \'*.*\'", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Warning);
                SearchByFileExt("*.*");
            }
            else
            {
                SearchByFileExt(fileExt);
            }
        }
    }
}
