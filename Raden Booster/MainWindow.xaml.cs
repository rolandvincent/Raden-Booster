using MaterialDesignThemes.Wpf;
using Raden_Booster.Utils.Config;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;

            LoadSettings();

            booster =  new Booster();
            games = new Games();
            temprature = new Temprature();
            tweak = new Tweak();

            PageView.Content = booster;
            ModifyTheme(true);

            Closing += MainWindow_Closing;
        }

        private void LoadSettings()
        {
            if (AppConfig.Current.MainWindowPoint != null)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = AppConfig.Current.MainWindowPoint?.X ?? 0;
                this.Top = AppConfig.Current.MainWindowPoint?.Y ?? 0;
            }

            if (AppConfig.Current.MainWindowSize != null)
            {
                this.Width = AppConfig.Current.MainWindowSize?.Width ?? 0;
                this.Height = AppConfig.Current.MainWindowSize?.Height ?? 0;
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AppConfig.Current.MainWindowSize = new Size(this.Width, this.Height);
            AppConfig.Current.MainWindowPoint = new Point(this.Left, this.Top);
            TempratureInfo.Close();
            ((Temprature)temprature)._cancelToken.Cancel();
        }

        private Page booster;
        private Page games;
        private Page temprature;
        private Page tweak;

        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }

        // Can execute
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
            Hide();
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
                txtTitle.Margin = new Thickness(3 + MenuBtn.ActualWidth, 4, TitleButtons.ActualWidth, 0);
            else
                txtTitle.Margin = new Thickness(3, 4, 0, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuContextMenu.PlacementTarget = MenuBtn;
            MenuContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            MenuContextMenu.IsOpen = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (((RadioButton)sender).IsChecked == true)
                PageView.Content = booster;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked == true)
                PageView.Content = games;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked == true)
                PageView.Content = tweak;
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked == true)
                PageView.Content = temprature;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutDlg.ShowDialog(AboutDlg.Content);
            Booster window = (Booster)booster;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Booster page1 = (Booster)booster;
            Temprature page4 = (Temprature)temprature;
            if (Visibility == Visibility.Visible)
            {
                if (page1._cancelToken.IsCancellationRequested)
                {
                    page1._cancelToken = new CancellationTokenSource();
                    page1.StartMonitoring(page1._cancelToken.Token);
                }
            }
            else
            {
                page1._cancelToken.Cancel();
            }

        }
    }
}
