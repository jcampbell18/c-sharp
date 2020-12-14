using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace campbelljproj2d
{
    /// <summary>
    /// Class that focuses on File Input/Output
    /// </summary>
    class FileIO
    {
        public FileIO() { }

        /// <summary>
        /// Reads the file, and adds to a string List
        /// </summary>
        /// <param name="filename">the text file</param>
        /// <returns>the list</returns>
        public List<string> ReadFile(string filename)
        {
            string line;
            List<string> list = new List<string>();

            StreamReader file = new StreamReader(@filename);

            while( (line = file.ReadLine()) != null)
            {
                list.Add(line);
            }

            return list;
        }
    }
}
