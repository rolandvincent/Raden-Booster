using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Raden_Booster.Utils.Config
{
    internal class AppConfig
    {
        private static AppConfig instance = null;
        public static AppConfig Current
        {
            get
            {
                if (instance == null)
                    instance = new AppConfig();
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        public static void Reload()
        {
            instance.Configuration.Load();
        }

        public RegistryConfig Configuration;
        public AppConfig()
        {
            Configuration = new RegistryConfig("Raden Booster", Microsoft.Win32.RegistryHive.CurrentUser);
            Configuration.Load();
        }

        string MainWin = "MainWindow";
        string TaskMgr = "TaskManager";
        public Point? MainWindowPoint { 
            get
            {
                object X = Configuration.Get($"{MainWin}\\X");
                object Y = Configuration.Get($"{MainWin}\\Y");
                if (X != null && Y != null)
                    return new Point(double.Parse(X.ToString()), double.Parse(Y.ToString()));
                else
                    return null;
            }
            set
            {
                Configuration.Set($"{MainWin}\\X", value?.X);
                Configuration.Set($"{MainWin}\\Y", value?.Y);
                Configuration.Save();
            }
        }

        public Size? MainWindowSize
        {
            get
            {
                object W = Configuration.Get($"{MainWin}\\W");
                object H = Configuration.Get($"{MainWin}\\H");
                if (W != null && H != null)
                    return new Size(double.Parse(W.ToString()), double.Parse(H.ToString()));
                else
                    return null;
            }
            set
            {
                Configuration.Set($"{MainWin}\\W", value?.Width);
                Configuration.Set($"{MainWin}\\H", value?.Height);
                Configuration.Save();
            }
        }

        public Point? TaskManagerPoint
        {
            get
            {
                object X = Configuration.Get($"{TaskMgr}\\X");
                object Y = Configuration.Get($"{TaskMgr}\\Y");
                if (X != null && Y != null)
                    return new Point(double.Parse(X.ToString()), double.Parse(Y.ToString()));
                else
                    return null;
            }
            set
            {
                Configuration.Set($"{TaskMgr}\\X", value?.X);
                Configuration.Set($"{TaskMgr}\\Y", value?.Y);
                Configuration.Save();
            }
        }

        public Size? TaskManagerSize
        {
            get
            {
                object W = Configuration.Get($"{TaskMgr}\\W");
                object H = Configuration.Get($"{TaskMgr}\\H");
                if (W != null && H != null)
                    return new Size(double.Parse(W.ToString()), double.Parse(H.ToString()));
                else
                    return null;
            }
            set
            {
                Configuration.Set($"{TaskMgr}\\W", value?.Width);
                Configuration.Set($"{TaskMgr}\\H", value?.Height);
                Configuration.Save();
            }
        }

        public bool TaskManagerTopMost
        {
            get
            {
                return Configuration.Get($"{TaskMgr}\\AlwaysOnTop", false);
            }
            set
            {
                Configuration.Set($"{TaskMgr}\\AlwaysOnTop", value);
                Configuration.Save();
            }
        }

        public bool TempMonitor
        {
            get
            {
                return Configuration.Get("TempMonitor", true);
            }
            set
            {
                Configuration.Set($"TempMonitor", value);
                Configuration.Save();
            }
        }

        public string[] Games
        {
            get
            {
                return Configuration.Get("GameList", Array.Empty<string>());
            }
            set
            {
                Configuration.Set("GameList", value);
                Configuration.Save();
            }
        }

        public bool GameBoosterEnabled
        {
            get
            {
                return Configuration.Get("GameBoosterEnabled", false);
            }
            set
            {
                Configuration.Set("GameBoosterEnabled", value);
                Configuration.Save();
            }
        }

        public void RemoveGame(string FileName)
        {
            var GameList = Games.ToList();
            GameList.Remove(FileName);
            Games = GameList.ToArray();
        }

    }
}
