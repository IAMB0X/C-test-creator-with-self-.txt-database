using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3
{
    internal class FileChecker
    {
        public string checkedFile;
        public static readonly List<string> picExt = new List<string>() { ".png", ".jpg", null };

        public bool FileCheck(string fileName, string directory)
        {
            List<string> ext = picExt;
            bool isMatch = false;
            foreach (var myext in ext)
            {
                FileInfo file = new FileInfo(directory + fileName + myext);
                isMatch = file.Exists;
                if (isMatch)
                {
                    checkedFile = Convert.ToString(file.FullName);
                    return isMatch;
                }
            }

            return isMatch;
        }
    }
}
