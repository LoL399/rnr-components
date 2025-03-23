using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace sandbox.Service
{
    class LogService
    {
        private static string currentPath = ApplicationData.Current.LocalFolder.Path;
        private static string keyFileName = "log.dat";
        private Dictionary<string, string> keyValueStore;
        public static string filePath   // property
        {
            get { return Path.Combine(dirPath, keyFileName); }   // get method
        }

        public static string dirPath   // property
        {
            get { return Path.Combine(currentPath, "log"); }   // get method
        }


        public static void Init()
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath); // Create the directory if it doesn't exist
                Console.WriteLine("Directory created successfully.");
                using (FileStream fs = File.Create(filePath))
                {
                    Console.WriteLine("File created successfully.");
                }
            }
        }

    }
}
