using IWshRuntimeLibrary;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Raden_Booster.PublicMethod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for StartupManagement.xaml
    /// </summary>
    public partial class StartupManagement : Window
    {
        public StartupManagement()
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        // State change
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
            }
        }

        [Obsolete]
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FormattedText formattedText = new FormattedText(Title, Thread.CurrentThread.CurrentCulture, FlowDirection.LeftToRight, new Typeface(txtTitle.FontFamily, txtTitle.FontStyle, txtTitle.FontWeight, txtTitle.FontStretch), txtTitle.FontSize, txtTitle.Foreground);
            if ((this.Width / 2d - TitleButtons.ActualWidth) - (formattedText.Width + txtTitle.Margin.Left * 2d) / 2d < 0)
                txtTitle.Margin = new Thickness(3, 4, TitleButtons.ActualWidth, 0);
            else
                txtTitle.Margin = new Thickness(3, 4, 0, 0);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WqlEventQuery hklm64 = new WqlEventQuery(
                 "SELECT * FROM RegistryKeyChangeEvent WHERE " +
                 "Hive = 'HKEY_LOCAL_MACHINE'" +
                 @"AND KeyPath = 'SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run'");
            ManagementEventWatcher hklmwatch64 = new ManagementEventWatcher(hklm64);

            WqlEventQuery hklm32 = new WqlEventQuery(
                 "SELECT * FROM RegistryKeyChangeEvent WHERE " +
                 "Hive = 'HKEY_LOCAL_MACHINE'" +
                 @"AND KeyPath = 'SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run'");
            ManagementEventWatcher hklmwatch32 = new ManagementEventWatcher(hklm32);

            WqlEventQuery approvedrun = new WqlEventQuery(
                 "SELECT * FROM RegistryTreeChangeEvent WHERE " +
                 "Hive = 'HKEY_LOCAL_MACHINE'" +
                 @"AND RootPath = 'SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved'");
            ManagementEventWatcher approvedrunwatcher = new ManagementEventWatcher(approvedrun);

            hklmwatch64.EventArrived += new EventArrivedEventHandler(HandleEvent);
            hklmwatch32.EventArrived += new EventArrivedEventHandler(HandleEvent);
            approvedrunwatcher.EventArrived += new EventArrivedEventHandler(HandleEvent);

            try
            {
                approvedrunwatcher.Start();
                hklmwatch64.Start();
                hklmwatch32.Start();
            }
            catch { }

            Refresh();
        }

        private void HandleEvent(object sender, EventArrivedEventArgs e)
        {
            Task.Delay(500).Wait();
            Dispatcher.Invoke(() =>
            {
                Refresh();
            });
        }

        private RegistryKey GetRegistry(RegistryHive Hive, Boolean Use64 = false)
        {
            if (Hive == RegistryHive.CurrentUser)
                return Registry.CurrentUser;
            else if (Hive == RegistryHive.LocalMachine)
            {
                if (Use64 && Environment.Is64BitOperatingSystem)
                    return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                else
                    return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            }
            else
            {
                return Registry.LocalMachine;
            }
        }
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                startuplist.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            startuplist.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        const string STARTUP_APPROVED = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved";
        const string PATH_PATTERN = @"^\s*(?<path3>(?:[^\/\-""><\?`=|]+\.exe|[^\/\-""><\?`=|]+))|^\""(?<path>[^""]+)|^\s*(?<path2>[^ ]+)";
        const string COMPANY_PATTERN = @"CN=([^,]+)";

        private void Refresh()
        {
            WshShell shell = new WshShell();
            List<StartupData> StartupList = new List<StartupData>();
            StartupList.Clear();

            foreach (string RegName in Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run").GetValueNames())
            {
                try
                {
                    StartupData startup = new StartupData();
                    startup.SourceName = RegName;
                    startup.Source = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                    startup.RegDestination = SourceDestination.HKCU;
                    startup.CommandLine = (String)Registry.CurrentUser.OpenSubKey(startup.Source).GetValue(RegName);
                    startup.Type = StarupType.Registry;
                    startup.StartupEnablePath = $@"{STARTUP_APPROVED}\Run";
                    Match m = Regex.Match(startup.CommandLine, PATH_PATTERN, RegexOptions.IgnoreCase);
                    string fileName = "";
                    if (m.Groups["path"].Success)
                        fileName = m.Groups["path"].Value.Trim();
                    if (m.Groups["path2"].Success)
                        fileName = m.Groups["path2"].Value.Trim();
                    if (m.Groups["path3"].Success)
                        fileName = m.Groups["path3"].Value.Trim();
                    startup.Location = Environment.ExpandEnvironmentVariables(fileName);
                    if (System.IO.File.Exists(startup.Location))
                    {
                        FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(startup.Location);
                        startup.Publisher = fileInfo.CompanyName.Trim();
                        if (String.IsNullOrWhiteSpace(startup.Publisher))
                        {
                            try
                            {
                                X509Certificate certificate = X509Certificate.CreateFromSignedFile(startup.Location);
                                if (certificate != null)
                                {
                                    string rawPublisher = certificate.Subject;
                                    Match cn = Regex.Match(rawPublisher, COMPANY_PATTERN, RegexOptions.IgnoreCase);
                                    if (cn.Success)
                                        startup.Publisher = cn.Groups[1].Value;
                                }
                            }
                            catch { }
                        }
                        startup.Name = String.IsNullOrEmpty(fileInfo.FileDescription) ? new FileInfo(fileName).Name : fileInfo.FileDescription;
                        StartupList.Add(startup);
                    }
                }
                catch (Exception ex) { 
                    Debug.WriteLine("Error:" + ex.StackTrace.ToString()); 
                }
            }
            if (Environment.Is64BitOperatingSystem)
            {
                foreach (string RegName in GetRegistry(RegistryHive.LocalMachine, true).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run").GetValueNames())
                {
                    try
                    {
                        StartupData startup = new StartupData();
                        startup.SourceName = RegName;
                        startup.Source = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                        startup.RegDestination = SourceDestination.HKLM;
                        startup.CommandLine = (String)GetRegistry(RegistryHive.LocalMachine, true).OpenSubKey(startup.Source).GetValue(RegName);
                        startup.StartupEnablePath = $@"{STARTUP_APPROVED}\Run";
                        startup.Type = StarupType.Registry;
                        Match m = Regex.Match(startup.CommandLine, PATH_PATTERN, RegexOptions.IgnoreCase);
                        string fileName = "";
                        if (m.Groups["path"].Success)
                            fileName = m.Groups["path"].Value.Trim();
                        if (m.Groups["path2"].Success)
                            fileName = m.Groups["path2"].Value.Trim();
                        if (m.Groups["path3"].Success)
                            fileName = m.Groups["path3"].Value.Trim();
                        startup.Location = Environment.ExpandEnvironmentVariables(fileName);
                        if (System.IO.File.Exists(startup.Location))
                        {
                            FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(startup.Location);
                            startup.Publisher = fileInfo.CompanyName.Trim();
                            if (String.IsNullOrWhiteSpace(startup.Publisher))
                            {
                                try
                                {
                                    X509Certificate certificate = X509Certificate.CreateFromSignedFile(startup.Location);
                                    if (certificate != null)
                                    {
                                        string rawPublisher = certificate.Subject;
                                        Match cn = Regex.Match(rawPublisher, COMPANY_PATTERN, RegexOptions.IgnoreCase);
                                        if (cn.Success)
                                            startup.Publisher = cn.Groups[1].Value;
                                    }
                                }
                                catch { }
                            }
                            startup.Name = String.IsNullOrEmpty(fileInfo.FileDescription) ? new FileInfo(fileName).Name : fileInfo.FileDescription;
                            StartupList.Add(startup);
                        }
                    }
                    catch (Exception ex) { 
                        Debug.WriteLine("Error:" + ex.StackTrace.ToString()); 
                    }
                }
            }
            foreach (string RegName in GetRegistry(RegistryHive.LocalMachine).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run").GetValueNames())
            {
                try
                {
                    StartupData startup = new StartupData();
                    startup.SourceName = RegName;
                    startup.Source = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
                    startup.RegDestination = SourceDestination.HKLM32;
                    startup.CommandLine = (String)GetRegistry(RegistryHive.LocalMachine).OpenSubKey(startup.Source).GetValue(RegName);
                    startup.StartupEnablePath = $@"{STARTUP_APPROVED}\Run32";
                    startup.Type = StarupType.Registry;
                    Match m = Regex.Match(startup.CommandLine, PATH_PATTERN, RegexOptions.IgnoreCase);
                    string fileName = "";
                    if (m.Groups["path"].Success)
                        fileName = m.Groups["path"].Value.Trim();
                    if (m.Groups["path2"].Success)
                        fileName = m.Groups["path2"].Value.Trim();
                    if (m.Groups["path3"].Success)
                        fileName = m.Groups["path3"].Value.Trim();
                    startup.Location = Environment.ExpandEnvironmentVariables(fileName);
                    if (System.IO.File.Exists(startup.Location))
                    {
                        FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(startup.Location);
                        startup.Publisher = fileInfo.CompanyName.Trim();
                        if (String.IsNullOrWhiteSpace(startup.Publisher))
                        {
                            try
                            {
                                X509Certificate certificate = X509Certificate.CreateFromSignedFile(startup.Location);
                                if (certificate != null)
                                {
                                    string rawPublisher = certificate.Subject;
                                    Match cn = Regex.Match(rawPublisher, COMPANY_PATTERN, RegexOptions.IgnoreCase);
                                    if (cn.Success)
                                        startup.Publisher = cn.Groups[1].Value;
                                }
                            }
                            catch { }
                        }
                        startup.Name = String.IsNullOrEmpty(fileInfo.FileDescription) ? new FileInfo(fileName).Name : fileInfo.FileDescription;
                        StartupList.Add(startup);
                    }
                }
                catch (Exception ex) { 
                    Debug.WriteLine("Error:" + ex.StackTrace.ToString()); 
                }
            }

            foreach (FileInfo shortcut in new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Startup)).GetFiles())
            {
                try
                {
                    if (shortcut.Name == "desktop.ini") continue;
                    if (shortcut.Extension.ToLower() == ".lnk")
                    {
                        IWshShortcut link = (IWshShortcut)shell.CreateShortcut(shortcut.FullName);
                        StartupData startup = new StartupData();
                        startup.SourceName = shortcut.Name;
                        startup.Source = shortcut.FullName;
                        startup.RegDestination = SourceDestination.HKCU;
                        startup.CommandLine = link.TargetPath;
                        startup.StartupEnablePath = $@"{STARTUP_APPROVED}\StartupFolder";
                        startup.Type = StarupType.Shortcut;
                        Match m = Regex.Match(startup.CommandLine, PATH_PATTERN, RegexOptions.IgnoreCase);
                        string fileName = "";
                        if (m.Groups["path"].Success)
                            fileName = m.Groups["path"].Value.Trim();
                        if (m.Groups["path2"].Success)
                            fileName = m.Groups["path2"].Value.Trim();
                        if (m.Groups["path3"].Success)
                            fileName = m.Groups["path3"].Value.Trim();
                        startup.Location = Environment.ExpandEnvironmentVariables(fileName);
                        if (System.IO.File.Exists(startup.Location))
                        {
                            FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(startup.Location);
                            startup.Publisher = fileInfo.CompanyName.Trim();
                            if (String.IsNullOrWhiteSpace(startup.Publisher))
                            {
                                try
                                {
                                    X509Certificate certificate = X509Certificate.CreateFromSignedFile(startup.Location);
                                    if (certificate != null)
                                    {
                                        string rawPublisher = certificate.Subject;
                                        Match cn = Regex.Match(rawPublisher, COMPANY_PATTERN, RegexOptions.IgnoreCase);
                                        if (cn.Success)
                                            startup.Publisher = cn.Groups[1].Value;
                                    }
                                }
                                catch { }
                            }
                            startup.Name = String.IsNullOrEmpty(fileInfo.FileDescription) ? new FileInfo(fileName).Name : fileInfo.FileDescription;
                            StartupList.Add(startup);
                        }
                    }
                }
                catch (Exception ex) { 
                    Debug.WriteLine("Error:" + ex.StackTrace.ToString()); 
                }
            }


            foreach (FileInfo shortcut in new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Microsoft\Windows\Start Menu\Programs\StartUp").GetFiles())
            {
                try
                {
                    if (shortcut.Name == "desktop.ini") continue;
                    if (shortcut.Extension.ToLower() == ".lnk")
                    {
                        IWshShortcut link = (IWshShortcut)shell.CreateShortcut(shortcut.FullName);
                        StartupData startup = new StartupData();
                        startup.SourceName = shortcut.Name;
                        startup.Source = shortcut.FullName;
                        startup.RegDestination = SourceDestination.HKLM;
                        startup.CommandLine = link.TargetPath;
                        startup.StartupEnablePath = $@"{STARTUP_APPROVED}\StartupFolder";
                        startup.Type = StarupType.Shortcut;
                        Match m = Regex.Match(startup.CommandLine, PATH_PATTERN, RegexOptions.IgnoreCase);
                        string fileName = "";
                        if (m.Groups["path"].Success)
                            fileName = m.Groups["path"].Value.Trim();
                        if (m.Groups["path2"].Success)
                            fileName = m.Groups["path2"].Value.Trim();
                        if (m.Groups["path3"].Success)
                            fileName = m.Groups["path3"].Value.Trim();
                        startup.Location = Environment.ExpandEnvironmentVariables(fileName);
                        if (System.IO.File.Exists(startup.Location))
                        {
                            FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(startup.Location);
                            startup.Publisher = fileInfo.CompanyName.Trim();
                            if (String.IsNullOrWhiteSpace(startup.Publisher))
                            {
                                try
                                {
                                    X509Certificate certificate = X509Certificate.CreateFromSignedFile(startup.Location);
                                    if (certificate != null)
                                    {
                                        string rawPublisher = certificate.Subject;
                                        Match cn = Regex.Match(rawPublisher, COMPANY_PATTERN, RegexOptions.IgnoreCase);
                                        if (cn.Success)
                                            startup.Publisher = cn.Groups[1].Value;
                                    }
                                }
                                catch { }
                            }
                            startup.Name = String.IsNullOrEmpty(fileInfo.FileDescription) ? new FileInfo(fileName).Name : fileInfo.FileDescription;
                            StartupList.Add(startup);
                        }
                    }
                }
                catch (Exception ex) { 
                    Debug.WriteLine("Error:" + ex.StackTrace.ToString()); }
            }

            startuplist.ItemsSource = null;
            StartupList.Sort((a, b) => a.Name.CompareTo(b.Name));
            startuplist.ItemsSource = StartupList;
        }

        private void startuplist_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            open.IsEnabled = startuplist.SelectedItems.Count > 0;
            remove.IsEnabled = startuplist.SelectedItems.Count > 0;
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\explorer.exe",
                Arguments = " /select,\"" + ((StartupData)startuplist.SelectedItem).Location + "\""
            });
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            StartupData startupData = (StartupData)startuplist.SelectedItem;
            if (startupData.Type == StarupType.Shortcut)
            {
                try
                {
                    System.IO.File.Delete(startupData.Source);
                }
                catch (Exception ex)
                {
                    message.Text = ex.Message;
                    ErrorDlg.ShowDialog(ErrorDlg.Content);
                }
            }
            else
            {
                try
                {
                    if (startupData.RegDestination == SourceDestination.HKCU)
                        Registry.CurrentUser.OpenSubKey(startupData.Source, true).DeleteValue(startupData.SourceName);
                    else if (startupData.RegDestination == SourceDestination.HKLM)
                        GetRegistry(RegistryHive.LocalMachine, true).OpenSubKey(startupData.Source, true).DeleteValue(startupData.SourceName);
                    else
                        GetRegistry(RegistryHive.LocalMachine).OpenSubKey(startupData.Source, true).DeleteValue(startupData.SourceName);
                }
                catch (Exception ex)
                {
                    message.Text = ex.Message;
                    ErrorDlg.ShowDialog(ErrorDlg.Content);
                }
            }
            Refresh();
        }

        private void HideStatusMessage()
        {
            if (statusmessage?.Height == 25)
            {
                DoubleAnimation timeline = new DoubleAnimation(25, 0, Duration.Forever);
                timeline.FillBehavior = FillBehavior.HoldEnd;
                CubicEase b = new CubicEase();
                b.EasingMode = EasingMode.EaseOut;
                timeline.EasingFunction = b;
                timeline.Duration = TimeSpan.FromMilliseconds(400);
                Storyboard.SetTargetName(timeline, "statusmessage");
                Storyboard.SetTargetProperty(
                    timeline, new PropertyPath(Card.HeightProperty));

                Storyboard status = new Storyboard();
                status.Children.Add(timeline);
                status.Begin(statusmessage);
            }
        }

        private void ShowStatusMessage()
        {
            if (statusmessage?.Height != 25)
            {
                DoubleAnimation timeline = new DoubleAnimation(0, 25, Duration.Forever);
                timeline.FillBehavior = FillBehavior.HoldEnd;
                CubicEase b = new CubicEase();
                timeline.EasingFunction = b;
                b.EasingMode = EasingMode.EaseIn;
                timeline.Duration = TimeSpan.FromMilliseconds(400);
                Storyboard.SetTargetName(timeline, "statusmessage");
                Storyboard.SetTargetProperty(
                    timeline, new PropertyPath(Card.HeightProperty));

                Storyboard status = new Storyboard();
                status.Children.Add(timeline);
                status.Begin(statusmessage);
            }
        }

        private void MessageDismiss(object sender, RoutedEventArgs e)
        {
            HideStatusMessage();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major <= 6 && Environment.OSVersion.Version.Minor <= 1)
                ShowStatusMessage();
        }
    }
}
