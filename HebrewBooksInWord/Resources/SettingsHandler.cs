using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HebrewBooksInWord.Resources
{
    public static class SettingsHandler
    {
        private static string settingsFilePath
        {
            get
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(appPath, "hebrewBooksSettings.dat");
            }
        }

        private static Dictionary<string, object> settingsCache = new Dictionary<string, object>();

        // Save a specific setting
        public static void SaveSetting<T>(string key, T value)
        {
            LoadAllSettings();

            settingsCache[key] = value;

            using (FileStream fs = new FileStream(settingsFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, settingsCache);
            }
        }

        // Load a specific setting
        public static T LoadSetting<T>(string key, T defaultValue = default(T))
        {
            LoadAllSettings();

            if (settingsCache.TryGetValue(key, out object value))
            {
                return (T)value;
            }

            return defaultValue;
        }

        // Load settings from disk only when necessary
        private static void LoadAllSettings()
        {
            if (settingsCache.Count == 0 && File.Exists(settingsFilePath))
            {
                using (FileStream fs = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    settingsCache = (Dictionary<string, object>)formatter.Deserialize(fs);
                }
            }
        }
    }
}