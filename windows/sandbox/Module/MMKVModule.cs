using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReactNative.Managed;
using sandbox.Common;
using Windows.UI.Xaml.Media.Animation;

namespace sandbox.Module
{
    [ReactModule]
    internal sealed class MMKVModule
    {
        public MMKVModule()
        {
            State.Instance.init();
        }

        [ReactMethod]
        public void setData(string keyName, string value) {
            // JSON value 
            State.Instance.UpdateValue(keyName, value);
        }

        [ReactMethod("getData")]
        public async Task<string> getData(string keyName)
        {
            // JSON value 
            var value = await State.Instance.GetValue(keyName);
            if (value != null) return value;
            return "";
        }

        [ReactMethod]
        public void clearKey(string keyName)
        {
            State.Instance.RemoveKey(keyName);
        }

        [ReactMethod]
        public void clearData()
        {
            State.Instance.ClearMemory();
        }
    }
}
