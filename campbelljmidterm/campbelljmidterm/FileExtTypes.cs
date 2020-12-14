using System.Collections.ObjectModel;

namespace campbelljmidterm
{
    /// <summary>
    /// a collection of file extension types for a dropdown menu
    /// </summary>
    class FileExtTypes : ObservableCollection<string>
    {
        /// <summary>
        /// adds a different file extension
        /// </summary>
        public FileExtTypes()
        {
            Add("*.*");
            Add("*.bmp");
            Add("*.csv");
            Add("*.doc");
            Add("*.docx");
            Add("*.exe");
            Add("*.gif");
            Add("*.htm");
            Add("*.html");
            Add("*.jpg");
            Add("*.jpeg");
            Add("*.pdf");
            Add("*.png");
            Add("*.ppt");
            Add("*.pptx");
            Add("*.rtf");
            Add("*.tif");
            Add("*.tmp");
            Add("*.txt");
            Add("*.xls");
            Add("*.xlsx");
            Add("*.zip");
        }
    }
}
