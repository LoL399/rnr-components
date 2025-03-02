using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Json;
using Windows.Storage;

namespace sandbox.Service
{
    public class MMKV
    {
        private string currentPath = ApplicationData.Current.LocalFolder.Path;
        private string keyFileName = @"example.dat";
        private Dictionary<string, string> keyValueStore;
        public string filePath   // property
        {
            get { return Path.Combine(dirPath, keyFileName); }   // get method
        }

        public string dirPath   // property
        {
            get { return Path.Combine(currentPath, "storage"); }   // get method
        }
        public void init()
        {
            //if (!File.Exists(filePath))
            //{
            //    // Create the file
            //    using (FileStream fs = File.Create(filePath))
            //    {
            //        Console.WriteLine("File created successfully.");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("File already exists.");
            //}
            keyValueStore = LoadFile();

        }
        public MMKV()
        {
            keyValueStore = LoadFile();
        }
        // Load the key-value pairs from the file
        private Dictionary<string, string> LoadFile()
        {
            var keyValuePairs = new Dictionary<string, string>();

            if (File.Exists(filePath))
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var lines = File.ReadAllLines(filePath, Encoding.Default);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            keyValuePairs[parts[0]] = parts[1];
                        }
                    }
                }
    
            }
            //File.Create(filePath).Close();
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath); // Create the directory if it doesn't exist
                Console.WriteLine("Directory created successfully.");
                using (FileStream fs = File.Create(filePath))
                {
                    Console.WriteLine("File created successfully.");
                }
            }

            return keyValuePairs;
        }
        // Get a value from memory (the dictionary)
        public Task<string> GetValue(string key)
        {
            if (keyValueStore.ContainsKey(key))
            {
                var value = keyValueStore[key];
                
                return Task.FromResult(value);
            }
            return Task.FromResult(String.Empty); 
        }
        // Update a value in memory and on the disk
        public void UpdateValue(string key, string newValue)
        {
            if (keyValueStore.ContainsKey(key))
            {
                keyValueStore[key] = newValue;

            } else
            {
                keyValueStore.Add(key, newValue);
            }
            UpdateFile();

        }
        // Remove a key-value pair from memory and update the file
        public void RemoveKey(string key)
        {
            if (keyValueStore.ContainsKey(key))
            {
                keyValueStore.Remove(key);
                UpdateFile();
            }
        }

        private void UpdateFile()
        {
            var lines = new List<string>();
            foreach (var kvp in keyValueStore)
            {
                lines.Add($"{kvp.Key}={kvp.Value}");
            }
            File.WriteAllLines(filePath, lines);
        }
        public void ClearMemory()
        {
            keyValueStore.Clear();
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }
    }
}
