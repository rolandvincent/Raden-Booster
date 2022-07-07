using Microsoft.Win32;
using Raden_Booster.Utils.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WinAPI;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for Games.xaml
    /// </summary>
    public partial class Games : Page
    {

        private CancellationTokenSource _cancelationToken = new CancellationTokenSource();
        private Task task;
        const string COMPANY_PATTERN = @"CN=([^,]+)";
        public Games()
        {
            InitializeComponent();
            List<string> games = AppConfig.Current.Games.ToList();
            if (games.Count > 0)
            {
                foreach (string file in games)
                {
                    try
                    {
                        AddGame(file);
                    }
                    catch { }
                }
            }

            if (AppConfig.Current.GameBoosterEnabled)
            {
                BoosterEnabled.IsChecked = true;
                StartMonitoring();
            }
            else
                BoosterEnabled.IsChecked = false;

        }

        private void AddGame(string Path)
        {
            FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(Path);
            Grid grid = new Grid
            {
                Height = 50,
                Width = 50,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(3)
            };

            Button button = new Button();
            button.Style = (Style)Application.Current.Resources["closeBtn"];
            button.Visibility = Visibility.Hidden;

            button.Click += delegate
            {
                AppConfig.Current.RemoveGame(grid.Tag.ToString());
                GameList.Children.Remove(grid);
            };

            Image image = new Image();
            FileToImageIconConverter fileToImage = new FileToImageIconConverter(Path);

            image.Source = fileToImage.Icon;
            image.Width = 40;
            image.Margin = new Thickness(3);

            string[] FileName = new System.IO.FileInfo(fileInfo.FileName).Name.Split('.');
            if (string.IsNullOrEmpty(fileInfo.FileDescription))
            {
                if (string.IsNullOrEmpty(fileInfo.CompanyName))
                {
                    try
                    {
                        X509Certificate certificate = X509Certificate.CreateFromSignedFile(fileInfo.FileName);
                        if (certificate != null)
                        {
                            string rawPublisher = certificate.Subject;
                            Match cn = Regex.Match(rawPublisher, COMPANY_PATTERN, RegexOptions.IgnoreCase);
                            if (cn.Success)
                                image.ToolTip = cn.Groups[1].Value;
                            else
                                image.ToolTip = String.Join(".", FileName.Take(FileName.Length - 1));
                        }
                        else image.ToolTip = String.Join(".", FileName.Take(FileName.Length - 1));
                    }
                    catch
                    {
                        image.ToolTip = String.Join(".", FileName.Take(FileName.Length - 1));
                    }
                }
                else image.ToolTip = fileInfo.CompanyName;
            }
            else image.ToolTip = fileInfo.FileDescription;

            grid.Children.Add(image);
            grid.Children.Add(button);
            grid.MouseLeftButtonDown += ItemClick;
            grid.MouseLeftButtonUp += ItemClick;

            grid.MouseEnter += delegate
            {
                button.Visibility = Visibility.Visible;
                grid.Background = new SolidColorBrush(Color.FromRgb(60, 60, 60));
            };

            grid.MouseLeave += delegate
            {
                button.Visibility = Visibility.Hidden;
                grid.Background = new SolidColorBrush(Colors.Transparent);
            };

            grid.Tag = Path;

            GameList.Children.Insert(GameList.Children.Count - 1, grid);
        }

        public void StartMonitoring()
        {
            //List<int> GameProcess = new List<int>();
            //var GameListC = DatabaseController.GetGames();
            //GameListC.ForEach(_ => _ = _.ToLower());

            //foreach (Process process in Process.GetProcesses())
            //{
            //    try
            //    {
            //        if (GameListC.Contains(process.MainModule.FileName))
            //            GameProcess.Add(process.Id);
            //    }
            //    catch { }
            //}

            //task = Task.Factory.StartNew(() =>
            //{
            //    while (!_cancelationToken.Token.IsCancellationRequested)
            //    {
            //        List<ProcessW> Processes = new List<ProcessW>();
            //        foreach (Process p in Process.GetProcesses())
            //        {
            //            IntPtr procHandle = Kernel32.OpenProcess(Kernel32.ProcessAccessFlags.SetInformation | Kernel32.ProcessAccessFlags.QueryInformation, false, p.Id);
            //            if (procHandle != IntPtr.Zero)
            //            {
            //                if (!p.HasExited)
            //                {
            //                    try
            //                    {
            //                        ProcessW process = new ProcessW
            //                        {
            //                            ProcessName = p?.ProcessName,
            //                            Id = p.Id,
            //                            Handle = p.Handle,
            //                            FileName = p?.MainModule?.FileName
            //                        };
            //                        Processes.Add(process);
            //                    }
            //                    catch { }
            //                }
            //            }
            //        }
            //        foreach (ProcessW process in Processes)
            //        {
            //            try
            //            {
            //                if (GameListC.Contains(process.FileName) && !GameProcess.Contains(process.Id))
            //                {
            //                    GameProcess.Add(process.Id);
            //                    Dispatcher.Invoke(() =>
            //                    {
            //                        WinPopupMessage winPopupMessage = new WinPopupMessage();

            //                        winPopupMessage.Width = SystemParameters.WorkArea.Width - 200;
            //                        winPopupMessage.Left = 100;
            //                        winPopupMessage.Top = SystemParameters.WorkArea.Height - 100 - winPopupMessage.Height;
            //                        winPopupMessage.Show(1000, "Boosting...");
            //                    });
            //                    for (int i = 0; i < Processes.Count(); i++)
            //                        Kernel32.SetProcessWorkingSetSize(Processes[i].Handle, (IntPtr)(-1), (IntPtr)(-1));
            //                }
            //                else
            //                {
            //                    foreach (int id in GameProcess)
            //                    {
            //                        if (Process.GetProcessById(id).HasExited) GameProcess.Remove(id);
            //                        break;
            //                    }
            //                }
            //            }
            //            catch { }
            //        }
            //    }
            //}, _cancelationToken.Token, TaskCreationOptions.LongRunning, PriorityScheduler.Lowest);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oplg = new OpenFileDialog();
            oplg.Filter = "Application|*.exe";
            if (oplg.ShowDialog() == true)
            {
                AddGame(oplg.FileName);
                var games = AppConfig.Current.Games.ToList();
                games.Add(oplg.FileName);
                AppConfig.Current.Games = games.ToArray();
            }
        }

        private void ItemClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && e.ButtonState == MouseButtonState.Released)
            {
                WinPopupMessage winPopupMessage = new WinPopupMessage();
                WpfScreen screen = WpfScreen.GetScreenFrom(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive));
                winPopupMessage.Width = SystemParameters.WorkArea.Width - 200;
                winPopupMessage.Left = 100;
                winPopupMessage.Top = SystemParameters.WorkArea.Height - 100 - winPopupMessage.Height;
                winPopupMessage.Show();

                Process[] Processes = Process.GetProcesses();

                Task task = new Task(() =>
                {
                    for (int i = 0; i < Processes.Count(); i++)
                    {
                        try
                        {
                            Kernel32.SetProcessWorkingSetSize(Processes[i].Handle, (IntPtr)(-1), (IntPtr)(-1));
                        }
                        catch
                        { }
                    }
                });

                task.Start();

                task.ContinueWith(_task =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Process.Start(((Grid)sender).Tag.ToString());
                        winPopupMessage.Close();
                    });
                });


            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!((ToggleButton)sender).IsInitialized) return;
            AppConfig.Current.GameBoosterEnabled = true;
            _cancelationToken = new CancellationTokenSource();
            StartMonitoring();
        }

        private void BoosterEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!((ToggleButton)sender).IsInitialized) return;
            AppConfig.Current.GameBoosterEnabled = false;
            _cancelationToken.Cancel();
        }
    }
}
