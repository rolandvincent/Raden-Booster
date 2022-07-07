using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Raden_Booster
{
    class DatabaseController
    {
        public static string AppName = @"SOFTWARE\Raden Booster";

        public static List<String> games = new List<string>();
        public static void AddGame(string URL)
        {
            games.Add(URL);
            UpdateDatabase();
        }

        public static void RemoveGame(string URL)
        {
            games.Remove(games.Find(game => game == URL));
            UpdateDatabase();
        }

        private static bool IsDatabaseExist()
        {
            Registry.CurrentUser.CreateSubKey(AppName);
            return true;
        }

        public static List<String> GetGames()
        {
            IsDatabaseExist();
            string[] games = (string[])Registry.GetValue($@"{Registry.CurrentUser.Name}\{AppName}", "GameList", new string[] { });
            return games.ToList();
        }

        public static void UpdateDatabase()
        {
            Registry.CurrentUser.CreateSubKey(AppName);
            Registry.SetValue($@"{Registry.CurrentUser.Name}\{AppName}", "GameList", games.ToArray(), RegistryValueKind.MultiString);
        }

        public static bool GetBoolValue(string KeyName)
        {
            IsDatabaseExist();
            int enabled = (int)Registry.GetValue($@"{Registry.CurrentUser.Name}\{AppName}", KeyName, 0);
            return enabled == 0 ? false : true;
        }

        public static void SetBoolValue(string KeyName, bool Value)
        {
            Registry.CurrentUser.CreateSubKey(AppName);
            Registry.SetValue($@"{Registry.CurrentUser.Name}\{AppName}", KeyName, Value ? 1 : 0, RegistryValueKind.DWord);
        }
    }
}
