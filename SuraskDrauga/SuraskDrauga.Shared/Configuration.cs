using System;
using System.Collections.Generic;
using System.Text;

namespace SuraskDrauga
{
    class Configuration
    {
        public void SetGuid()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object value = localSettings.Values["GUID"];
            if (value == null)
            {
                localSettings.Values["GUID"] = Guid.NewGuid().ToString();
            }
            // Delete a simple setting
            //localSettings.Values.Remove("FirstTimeRun");
        }

        public void SetName(string name)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object value = localSettings.Values["NAME"];
            if ((string) value != name)
            {
                localSettings.Values["NAME"] = name;
            }
        }

        public string GetName()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object value = localSettings.Values["NAME"];
            if (value != null)
            {
                return (string)localSettings.Values["NAME"];
            }
            return "Igor_AK47";
        }

        public string GetGuid()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object value = localSettings.Values["GUID"];
            if (value != null)
            {
                return (string) localSettings.Values["GUID"];
            }
            return null;
        }
    }
}
