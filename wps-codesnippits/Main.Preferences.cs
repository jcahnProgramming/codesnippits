using System;
using UnityEngine;

public static partial class Main
{
    public static class Preferences
    {

        private static readonly string DateFormat = "dd-MM-yyyy HH:mm:ss";

        public static string CDNData
        {
            get { return GetString("CDNData"); }
            set { SetString("CDNData", value); }
        }

        public static DateTime CDNCacheDate
        {
            get { return GetDate("CDNCacheDate"); }
            set { SetDate("CDNCacheDate", value); }
        }

        public static string YouTubeData
        {
            get { return GetString("YouTubeData"); }
            set { SetString("YouTubeData", value); }
        }

        public static DateTime YouTubeCacheDate
        {
            get { return GetDate("YouTubeCacheDate"); }
            set { SetDate("YouTubeCacheDate", value); }
        }

        public static string GameDriveData
        {
            get { return GetString("GameDriveData"); }
            set { SetString("GameDriveData", value); }
        }

        public static DateTime GameDriveCacheDate
        {
            get { return GetDate("GameDriveCacheDate"); }
            set { SetDate("GameDriveCacheDate", value); }
        }

        public static bool TutorialShown
        {
            get { return GetString("TutorialShown", "False").Equals("True"); }
            set { SetString("TutorialShown", value.ToString()); }
        }

        public static string TutorialText
        {
            get { return GetString("TutorialText"); }
            set { SetString("TutorialText", value); }
        }

        public static string GetString(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }
            return null;
        }

        public static string GetString(string key, string def)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }
            else
            {
                return def;
            }
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static DateTime GetDate(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return DateTime.ParseExact(PlayerPrefs.GetString(key), DateFormat, null);
            }
            return DateTime.MinValue;
        }

        public static void SetDate(string key, DateTime value)
        {
            PlayerPrefs.SetString(key, value.ToString(DateFormat, null));
            PlayerPrefs.Save();
        }
    }
}