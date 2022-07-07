using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for Cleaner.xaml
    /// </summary>
    public partial class Cleaner : Window
    {
        public Cleaner()
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

        private long Recycle_Total, UserTemp_Total, WindowsTemp_Total, Browser_Total;

        private void updateTotal()
        {
            long total = 0;
            if (chkbox_recycle?.IsChecked == true)
                total += Recycle_Total;
            if (chkbox_browsercache?.IsChecked == true)
                total += Browser_Total;
            if (chkbox_usertemp?.IsChecked == true)
                total += UserTemp_Total;
            if (chkbox_wintemp?.IsChecked == true)
                total += WindowsTemp_Total;
            if (lbl_total != null)
                lbl_total.Content = ToStringSize(total);
        }

        private void resetLayout()
        {
            warn_browser.Visibility = Visibility.Collapsed;
            warn_recycle.Visibility = Visibility.Collapsed;
            warn_usertemp.Visibility = Visibility.Collapsed;
            warn_windowstemp.Visibility = Visibility.Collapsed;
            check_browser.Visibility = Visibility.Collapsed;
            check_recycle.Visibility = Visibility.Collapsed;
            check_usertemp.Visibility = Visibility.Collapsed;
            check_windowstemp.Visibility = Visibility.Collapsed;
            prog_browser.Visibility = Visibility.Collapsed;
            prog_recycle.Visibility = Visibility.Collapsed;
            prog_usertemp.Visibility = Visibility.Collapsed;
            prog_wintemp.Visibility = Visibility.Collapsed;
            btn_clean.IsEnabled = false;
            lbl_info.Content = "Press Scan to start scanning!";
            lbl_info.Foreground = (SolidColorBrush)Application.Current.FindResource("WindowForeground");
        }
        private void Checkbox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            updateTotal();
        }

        private string CurrentScan;
        private CancellationTokenSource CancelToken = new CancellationTokenSource();

        private string ToStringSize(long SizeInByte)
        {
            if ((SizeInByte / 1024d / 1024) > 1024)
                return Math.Round(SizeInByte / 1024d / 1024 / 1024, 2) + " GB";
            else if ((SizeInByte / 1024d) > 1024)
                return Math.Round(SizeInByte / 1024d / 1024, 2) + " MB";
            else if((SizeInByte) > 1024)
                return Math.Round(SizeInByte / 1024d, 2) + " KB";
            else
                return Math.Round(SizeInByte / 1d, 2) + " B";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btn_scan.IsEnabled = false;
            resetLayout();
            lbl_info.Foreground = new SolidColorBrush(Colors.Green);
            Recycle_Total = UserTemp_Total = WindowsTemp_Total = Browser_Total = 0;
            Task t = Task.Factory.StartNew(ScanCache, CancelToken.Token);
            while (t.Status == TaskStatus.Running || t.Status == TaskStatus.WaitingForChildrenToComplete || t.Status == TaskStatus.Created || t.Status == TaskStatus.WaitingToRun || t.Status == TaskStatus.WaitingForActivation)
            {
                recyclebin.Content = ToStringSize(Recycle_Total);
                usertemp.Content = ToStringSize(UserTemp_Total);
                windowstemp.Content = ToStringSize(WindowsTemp_Total);
                browser.Content = ToStringSize(Browser_Total);
                if (Browser_Total + UserTemp_Total + Recycle_Total + WindowsTemp_Total > 0)
                    lbl_info.Foreground = new SolidColorBrush(Colors.Red);
                lbl_info.Content = "There is " + ToStringSize(Browser_Total + UserTemp_Total + Recycle_Total + WindowsTemp_Total) + " of junk can be cleaned.";
                path.Text = CurrentScan;
                await Task.Delay(200);
            }
            path.Text = "";
            updateTotal();
            lbl_info.Content = "There is " + ToStringSize(Browser_Total + UserTemp_Total + Recycle_Total + WindowsTemp_Total) + " of junk can be cleaned.";
            btn_clean.IsEnabled = true;
            btn_scan.IsEnabled = true;
        }

        public void ScanCache()
        {
            Dispatcher.Invoke(() =>
            {
                prog_recycle.Visibility = Visibility.Visible;
            });
            Queue<string> Dir = new Queue<string>();
            Dir.Enqueue(Environment.GetFolderPath(Environment.SpecialFolder.Windows).Split('\\')[0] + "\\$Recycle.Bin");
            while (Dir.Count > 0)
            {
                string CurrentDir = Dir.Dequeue();
                try
                {
                    foreach (FileInfo FI in new DirectoryInfo(CurrentDir).GetFiles())
                    {
                        CurrentScan = FI.FullName;
                        Recycle_Total += FI.Length;
                    }
                }
                catch { }
                try
                {
                    foreach (DirectoryInfo DI in new DirectoryInfo(CurrentDir).GetDirectories())
                    {
                        Dir.Enqueue(DI.FullName);
                    }
                }
                catch { }
            }
            Dispatcher.Invoke(() =>
            {
                prog_recycle.Visibility = Visibility.Collapsed;
                recyclebin.Content = ToStringSize(Recycle_Total);
                if (Recycle_Total == 0)
                {
                    check_recycle.Visibility = Visibility.Visible;
                    warn_recycle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    check_recycle.Visibility = Visibility.Collapsed;
                    warn_recycle.Visibility = Visibility.Visible;
                }
                prog_usertemp.Visibility = Visibility.Visible;
            });
            Dir.Enqueue(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Temp");
            while (Dir.Count > 0)
            {
                string CurrentDir = Dir.Dequeue();
                try
                {
                    foreach (FileInfo FI in new DirectoryInfo(CurrentDir).GetFiles())
                    {
                        CurrentScan = FI.FullName;
                        UserTemp_Total += FI.Length;
                    }
                }
                catch { }
                try
                {
                    foreach (DirectoryInfo DI in new DirectoryInfo(CurrentDir).GetDirectories())
                    {
                        Dir.Enqueue(DI.FullName);
                    }
                }
                catch { }
            }
            Dispatcher.Invoke(() =>
            {
                prog_usertemp.Visibility = Visibility.Collapsed;
                usertemp.Content = ToStringSize(UserTemp_Total);
                if (UserTemp_Total == 0)
                {
                    check_usertemp.Visibility = Visibility.Visible;
                    warn_usertemp.Visibility = Visibility.Collapsed;
                }
                else
                {
                    check_usertemp.Visibility = Visibility.Collapsed;
                    warn_usertemp.Visibility = Visibility.Visible;
                }
                prog_wintemp.Visibility = Visibility.Visible;
            });
            Dir.Enqueue(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Temp");
            while (Dir.Count > 0)
            {
                string CurrentDir = Dir.Dequeue();
                try
                {
                    foreach (FileInfo FI in new DirectoryInfo(CurrentDir).GetFiles())
                    {
                        CurrentScan = FI.FullName;
                        WindowsTemp_Total += FI.Length;
                    }
                }
                catch { }
                try
                {
                    foreach (DirectoryInfo DI in new DirectoryInfo(CurrentDir).GetDirectories())
                    {
                        Dir.Enqueue(DI.FullName);
                    }
                }
                catch { }
            }
            Dispatcher.Invoke(() =>
            {
                prog_wintemp.Visibility = Visibility.Collapsed;
                windowstemp.Content = ToStringSize(WindowsTemp_Total);
                if (WindowsTemp_Total == 0)
                {
                    check_windowstemp.Visibility = Visibility.Visible;
                    warn_windowstemp.Visibility = Visibility.Collapsed;
                }
                else
                {
                    check_windowstemp.Visibility = Visibility.Collapsed;
                    warn_windowstemp.Visibility = Visibility.Visible;
                }
                prog_browser.Visibility = Visibility.Visible;
            });
            Dir.Enqueue(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Cache");
            Dir.Enqueue(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Code Cache");
            while (Dir.Count > 0)
            {
                string CurrentDir = Dir.Dequeue();
                try
                {
                    foreach (FileInfo FI in new DirectoryInfo(CurrentDir).GetFiles())
                    {
                        CurrentScan = FI.FullName;
                        Browser_Total += FI.Length;
                    }
                }
                catch { }
                try
                {
                    foreach (DirectoryInfo DI in new DirectoryInfo(CurrentDir).GetDirectories())
                    {
                        Dir.Enqueue(DI.FullName);
                    }
                }
                catch { }
            }
            Dispatcher.Invoke(() =>
            {
                prog_browser.Visibility = Visibility.Collapsed;
                browser.Content = ToStringSize(Browser_Total);
                if (Browser_Total == 0)
                {
                    check_browser.Visibility = Visibility.Visible;
                    warn_browser.Visibility = Visibility.Collapsed;
                }
                else
                {
                    check_browser.Visibility = Visibility.Collapsed;
                    warn_browser.Visibility = Visibility.Visible;
                }
            });
        }
    }
}
