using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using Forms = System.Windows.Forms;
using WinAPI;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Forms.NotifyIcon NotifyIcon { get; set; } = null;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0)
            {
                LoadMainWindow();
            }
            else
            {
                if (e.Args[0] == "--taskmgr")
                {
                    Task_Manager task_Manager = new Task_Manager();
                    task_Manager.Show();
                }
                else if (e.Args[0] == "--cleaner")
                {
                    Cleaner cleaner = new Cleaner();
                    cleaner.Show();
                }
                else if (e.Args[0] == "--boost")
                {
                    Process[] Processes = Process.GetProcesses();
                    for (int i = 0; i < Processes.Count(); i++)
                    {
                        try
                        {
                            Kernel32.SetProcessWorkingSetSize(Processes[i].Handle, (IntPtr)(-1), (IntPtr)(-1));
                        }
                        catch { }
                    }
                    Application.Current.Shutdown();
                }
                else if(e.Args[0] == "--hide")
                {
                    LoadNotifyIcon();
                }
                else
                {
                    LoadMainWindow();
                }
            }
        }

        private Window LoadMainWindow()
        {

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    if (window.Visibility != Visibility.Visible)
                        window.Show();
                    return window;
                }
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            LoadNotifyIcon();
            return MainWindow;
        }

        private void LoadNotifyIcon()
        {
            if (NotifyIcon == null)
            {
                NotifyIcon = new Forms.NotifyIcon()
                {
                    Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                    Visible = true,
                    ContextMenuStrip = CreateContextMenu()
                };
                NotifyIcon.MouseClick += NotifyIcon_MouseClick;
            }
        }

        private void NotifyIcon_MouseClick(object sender, Forms.MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                LoadMainWindow().Show();
            }
        }

        private Forms.ContextMenuStrip CreateContextMenu()
        {
            var openItem = new Forms.ToolStripMenuItem("Open");
            openItem.Click += delegate { LoadMainWindow(); };
            var exitItem = new Forms.ToolStripMenuItem("Exit");
            exitItem.Click += delegate
            {
                Application.Current.Shutdown();
            };
            var contextMenu = new Forms.ContextMenuStrip { Items = { openItem, exitItem } };
            return contextMenu;
        }
    }
}
