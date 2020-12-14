using System.IO;

namespace campbelljmidterm
{
    /// <summary>
    /// Validates the user-input
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// validates that the chosen folder path exists
        /// </summary>
        /// <returns>true if it is valid</returns>
        public bool ValidateFolderPath(string folderPath)
        {
            if (folderPath == null || folderPath.Length < 2 || !Directory.Exists(folderPath))
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// validates that the chosen file extension is not empty or less than 3 characters
        /// <exanple>
        /// valid: *.*      <-- 3 characters
        /// not valid: *.   <-- 2 characters
        /// </exanple>
        /// </summary>
        /// <returns>true if it is valid</returns>
        public bool ValidateFileExtension(string fileExtension)
        {
            if (fileExtension == null || fileExtension.Length < 3)
            {
                return false;
            }
            
            return true;
        }

    }
}
