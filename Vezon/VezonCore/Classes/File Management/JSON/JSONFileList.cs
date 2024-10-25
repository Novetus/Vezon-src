using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezonCore
{
    public class JSONFileList
    {
        public List<string> FileNames = new List<string>();
        public List<string> CleanedFileNames = new List<string>();

        public JSONFileList(string folderPath)
        {
            foreach (string fileName in Directory.GetFiles(folderPath, "*.json"))
            {
                FileNames.Add(fileName);
            }

            foreach (string fileName in FileNames)
            {
                CleanedFileNames.Add(Path.GetFileNameWithoutExtension(fileName));
            }
        }
    }
}
