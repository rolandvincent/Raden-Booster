using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WinAPI;
using Raden_Booster.Utils;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for Booster.xaml
    /// </summary>
    public partial class Booster : Page
    {
        bool boosting = false;

        internal CancellationTokenSource _cancelToken = new CancellationTokenSource();
        public Booster()
        {
            InitializeComponent();

            Proggress1.Value = 100;
            BoostBtn.Content = Proggress1.Value.ToString("0.##") + "%";

            StartMonitoring(_cancelToken.Token);
        }

        public void StartMonitoring(CancellationToken _cancelationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (!_cancelationToken.IsCancellationRequested)
                {
                    double memoryPercentage = PerformanceInfo.GetCPUUsagePercent() * 100d;

                    this.Dispatcher.Invoke(() =>
                    {
                        if (Application.Current.MainWindow?.WindowState != WindowState.Minimized)
                        {
                            BoostBtn.Content = memoryPercentage.ToString("0.##") + "%";
                            if (!boosting)
                            {
                                Proggress1.Value = memoryPercentage;
                                BoostBtn.Tag = "Click to boost!";
                            }
                        }
                    });

                    await Task.Delay(1000);
                }
            }, _cancelationToken, TaskCreationOptions.LongRunning, PriorityScheduler.Lowest);
        }

        private void BoostBtn_Click(object sender, RoutedEventArgs e)
        {
            boosting = true;
            Proggress1.IsIndeterminate = true;
            Proggress1.Value = 0;
            BoostBtn.IsHitTestVisible = false;
            BoostBtn.Tag = "Boosting...";
            Process[] Processes = Process.GetProcesses();
            int i = 0;
            int mode = boostmode.SelectedIndex;
            long MemoryBefore = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();

            Task.Run(async () =>
            {

                try
                {
                    if (mode == 0)
                    {
                        foreach (Process p in Processes)
                        {
                            try
                            {
                                Kernel32.SetProcessWorkingSetSize(p.Handle, (IntPtr)(-1), (IntPtr)(-1));
                            }
                            catch { }
                        }
                        await Task.Delay(5000);
                    }
                    else if (mode == 1)
                    {
                        Boost.REDUCT_WORKING_SET();
                        Boost.REDUCT_STANDBY_PRIORITY0_LIST();
                        await Task.Delay(5000);
                    }
                    else if (mode == 2)
                    {
                        Boost.REDUCT_COMBINE_MEMORY_LISTS();
                        Boost.REDUCT_SYSTEM_WORKING_SET();
                        Boost.REDUCT_WORKING_SET();
                        Boost.REDUCT_STANDBY_PRIORITY0_LIST();
                        Boost.REDUCT_STANDBY_LIST();
                        Boost.REDUCT_MODIFIED_LIST();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            MessageBox.MessageQueue?.Enqueue(
                                "Error Occured:" + ex.Message, "OK", (_) => MessageBox.MessageQueue?.Clear(), null, true, true, TimeSpan.FromSeconds(10));
                        });
                    }
                    catch (Exception ex2)
                    {
                        Debug.WriteLine(ex2.StackTrace);
                        Debug.WriteLine(ex2.Message);
                    }
                }

                this.Dispatcher.Invoke(() =>
                {
                    Proggress1.IsIndeterminate = false;
                    BoostBtn.IsHitTestVisible = true;
                    boosting = false;
                    long memoryReleased = PerformanceInfo.GetPhysicalAvailableMemoryInMiB() - MemoryBefore;
                    memoryReleased = memoryReleased < 0 ? 0 : memoryReleased;
                    MessageBox.MessageQueue?.Enqueue(
                        $"Boost completed. {memoryReleased} MB Memory Released.");
                });
            });
        }

        private void boostmode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (boostmode.SelectedIndex == 2)
            {
                ShowStatusMessage();
            }
            else
            {
                HideStatusMessage();
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

        private void MessageDismiss(object sender, RoutedEventArgs e)
        {
            HideStatusMessage();
        }
    }
}
