using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sandbox.Service;

namespace sandbox.Common
{
    class State
    {
        // The static instance of the class (singleton pattern)
        private static MMKV _instance;

        // A lock object for thread safety
        private static readonly object _lock = new object();

        // The state you want to store
        public string SomeState { get; set; }

        // Private constructor to prevent instantiation
        private State()
        {
            _instance = new MMKV();
        }

        // Public method to get the instance of the singleton
        public static MMKV Instance
        {
            get
            {
                // Ensure thread-safety when accessing the instance
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MMKV();
                    }
                    return _instance;
                }
            }
        }
    }
}
