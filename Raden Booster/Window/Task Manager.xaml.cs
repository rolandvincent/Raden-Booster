using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using Raden_Booster.PublicMethod;
using Raden_Booster.Utils;
using Raden_Booster.Utils.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for Task_Manager.xaml
    /// </summary>
    public partial class Task_Manager : Window
    {
        public Task_Manager()
        {
            InitializeComponent();
            LoadSettings();
            StateChanged += MainWindowStateChangeRaised;
            CancellationToken cancel = source.Token;

            new Thread(() =>
            {
                List<ProcessData> process = GetAllProcessData();
                this.Dispatcher.Invoke(() =>
                {
                    xlist.BeginInit();
                    foreach (ProcessData data in process)
                    {
                        xlist.Items.Add(data);
                    }
                    xlist.EndInit();
                    process_load_prog.Visibility = Visibility.Collapsed;
                });
            }).Start();

            WatchProcess();

            Task.Factory.StartNew(async () =>
            {
                while (!cancel.IsCancellationRequested)
                {
                    await UpdateProcessOriginal();
                    await Task.Delay(1000);
                }
            }, cancel, TaskCreationOptions.LongRunning, PriorityScheduler.BelowNormal);

            ModifyTheme(true);
        }

        private void LoadSettings()
        {
            if (AppConfig.Current.TaskManagerPoint != null)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = AppConfig.Current.TaskManagerPoint?.X ?? 0;
                this.Top = AppConfig.Current.TaskManagerPoint?.Y ?? 0;
            }

            if (AppConfig.Current.TaskManagerSize != null)
            {
                this.Width = AppConfig.Current.TaskManagerSize?.Width ?? 0;
                this.Height = AppConfig.Current.TaskManagerSize?.Height ?? 0;
            }

            topmost.IsChecked = AppConfig.Current.TaskManagerTopMost;
        }

        private List<ProcessData> GetAllProcessData()
        {
            List<ProcessData> processData = new List<ProcessData>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Process");
            Process[] processes = Process.GetProcesses();

            var sii = new WinAPI.Shell32.SHSTOCKICONINFO();
            sii.cbSize = (UInt32)Marshal.SizeOf(typeof(WinAPI.Shell32.SHSTOCKICONINFO));
            WinAPI.Shell32.SHGetStockIconInfo(WinAPI.Shell32.SHSTOCKICONID.SIID_APPLICATION,
                WinAPI.Shell32.SHGSI.SHGSI_ICON,
                ref sii);

            foreach (ManagementObject obj in searcher.Get())
            {
                try
                {
                    Process p = processes.First(x => x.Id == int.Parse(obj["ProcessId"]?.ToString()));

                    ProcessData pData = new ProcessData
                    {
                        State = ProcessData.ProcessState.Running,
                        Name = obj["Name"]?.ToString(),
                        CommandLine = obj["CommandLine"]?.ToString(),
                        PID = int.Parse(obj["ProcessId"]?.ToString()),
                        Location = obj["ExecutablePath"]?.ToString(),
                        Memory = int.Parse(obj["WorkingSetSize"]?.ToString()),
                        ParentProcessId = int.Parse(obj["ParentProcessId"]?.ToString()),
                        Title = p?.MainWindowTitle,
                    };
                    try
                    {
                        IntPtr pHandle = WinAPI.Kernel32.OpenProcess(p, WinAPI.Kernel32.ProcessAccessFlags.QueryInformation);
                        if (pHandle != IntPtr.Zero)
                        {
                            if (p.Threads[0].WaitReason == ThreadWaitReason.Suspended)
                                pData.Status = "Suspended";
                            else
                                pData.Status = "Running";
                        }
                        else 
                            pData.Status = "Running";
                        WinAPI.Kernel32.CloseHandle(pHandle);
                    }
                    catch { pData.Status = "Running"; }

                    Dispatcher.Invoke(() =>
                    {
                        if (String.IsNullOrEmpty(pData.Location))
                        {
                            pData.Icon = Imaging.CreateBitmapSourceFromHIcon(sii.hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        }
                        else
                        {
                            FileToImageIconConverter fileToImage = new FileToImageIconConverter(pData.Location);
                            pData.Icon = fileToImage.Icon;
                        }
                    });
                    processData.Add(pData);
                }
                catch { }
                obj.Dispose();
            }
            processes = null;
            searcher.Dispose();
            processData.TrimExcess();
            return processData;
        }

        private List<ProcessData> GetAllProcessDataLite()
        {
            List<ProcessData> processData = new List<ProcessData>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT WorkingSetSize,ProcessId FROM Win32_Process");
            Process[] processes = Process.GetProcesses();

            foreach (ManagementObject obj in searcher.Get())
            {
                try
                {
                    ProcessData pData = new ProcessData
                    {
                        PID = int.Parse(obj["ProcessId"]?.ToString()),
                        Memory = int.Parse(obj["WorkingSetSize"]?.ToString()),

                    };
                    Process p = processes.First(x => x.Id == pData.PID);
                    try
                    {
                        IntPtr pHandle = WinAPI.Kernel32.OpenProcess(p, WinAPI.Kernel32.ProcessAccessFlags.QueryInformation);
                        if (pHandle != IntPtr.Zero)
                        {
                            if (p.Threads[0].WaitReason == ThreadWaitReason.Suspended)
                                pData.Status = "Suspended";
                            else
                                pData.Status = "Running";
                        }
                        else
                            pData.Status = "Running";
                        WinAPI.Kernel32.CloseHandle(pHandle);
                    }
                    catch { pData.Status = "Running"; }
                    processData.Add(pData);
                    obj.Dispose();
                }
                catch {
                    obj.Dispose();
                }
            }
            processes = null;
            searcher.Dispose();
            return processData;
        }

        private ProcessData GetProcessDataById(int pid)
        {
            ProcessData pData = null;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId = " + pid);
            using (ManagementObjectCollection objects = searcher.Get())
            {
                try
                {
                    ManagementBaseObject obj = objects.Cast<ManagementBaseObject>().SingleOrDefault();
                    if (obj == null) return null;
                    Process p = Process.GetProcessById(int.Parse(obj["ProcessId"]?.ToString()));
                    string status = "Running";

                    pData = new ProcessData
                    {
                        State = ProcessData.ProcessState.Running,
                        Name = obj["Name"]?.ToString(),
                        CommandLine = obj["CommandLine"]?.ToString(),
                        PID = int.Parse(obj["ProcessId"]?.ToString()),
                        Location = obj["ExecutablePath"]?.ToString(),
                        Memory = int.Parse(obj["WorkingSetSize"]?.ToString()),
                        ParentProcessId = int.Parse(obj["ParentProcessId"]?.ToString()),
                        Status = status,
                        Title = p.MainWindowTitle,
                    };
                    try
                    {
                        IntPtr pHandle = WinAPI.Kernel32.OpenProcess(p, WinAPI.Kernel32.ProcessAccessFlags.QueryInformation);
                        if (pHandle != IntPtr.Zero)
                        {
                            if (p.Threads[0].WaitReason == ThreadWaitReason.Suspended)
                                pData.Status = "Suspended";
                            else
                                pData.Status = "Running";
                        }
                        else
                            pData.Status = "Running";
                        WinAPI.Kernel32.CloseHandle(pHandle);
                    }
                    catch { pData.Status = "Running"; }
                    obj.Dispose();
                }
                catch { return null; }
                objects.Dispose();
            }
            searcher.Dispose();
            return pData;
        }

        private CancellationTokenSource source = new CancellationTokenSource();
        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }

        private Task UpdateProcess()
        {
            List<Process> proc = Process.GetProcesses().ToList();
            Dispatcher.Invoke(() =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                xlist.BeginInit();
                for (int i = 0; i < xlist.Items.Count; i++)
                {
                    var pData = (ProcessData)xlist.Items[i];
                    try
                    {
                        if (pData.State == ProcessData.ProcessState.Running)
                        {
                            var p = proc.First(x => x.Id == pData.PID);
                            if (p != null)
                            {
                                pData.Memory = p.PrivateMemorySize64;
                                IntPtr pHandle = WinAPI.Kernel32.OpenProcess(p, WinAPI.Kernel32.ProcessAccessFlags.QueryInformation);
                                if (pHandle != IntPtr.Zero)
                                {
                                    if (p.Threads[0].WaitReason == ThreadWaitReason.Suspended)
                                        pData.Status = "Suspended";
                                    else
                                        pData.Status = "Running";
                                }
                                xlist.Items[i] = pData;
                                WinAPI.Kernel32.CloseHandle(pHandle);
                            }
                        }
                        else if (pData.State == ProcessData.ProcessState.Create)
                        {
                            var p = proc.First(x => x.Id == pData.PID);
                            if (p != null)
                            {
                                pData.Memory = p.PrivateMemorySize64;
                                var elapsedSpan = new TimeSpan(DateTime.Now.Ticks - pData._time);
                                if (elapsedSpan.TotalSeconds > 1)
                                    pData.State = ProcessData.ProcessState.Running;
                                xlist.Items[i] = pData;
                            }
                        }
                        else if (pData.State == ProcessData.ProcessState.Shutdown)
                        {
                            var elapsedSpan = new TimeSpan(DateTime.Now.Ticks - pData._time);
                            if (elapsedSpan.TotalSeconds > 1)
                            {
                                xlist.Items.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    catch { }
                }
                xlist.EndInit();
            });
            proc = null;
            return Task.CompletedTask;
        }

        private Task UpdateProcessOriginal()
        {
            List<ProcessData> proc = GetAllProcessDataLite();
            Dispatcher.Invoke(() =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                xlist.BeginInit();
                for (int i = 0; i < xlist.Items.Count; i++)
                {
                    var pData = (ProcessData)xlist.Items[i];
                    try
                    {
                        if (pData.State == ProcessData.ProcessState.Running)
                        {
                            var p = proc.First(x => x.PID == pData.PID);
                            if (p != null)
                            {
                                pData.Memory = p.Memory;
                                pData.Status = p.Status;
                                xlist.Items[i] = pData;
                            }
                        }
                        else if (pData.State == ProcessData.ProcessState.Create)
                        {
                            var p = proc.First(x => x.PID == pData.PID);
                            if (p != null)
                            {
                                pData.Memory = p.Memory;
                                var elapsedSpan = new TimeSpan(DateTime.Now.Ticks - pData._time);
                                if (elapsedSpan.TotalSeconds > 1)
                                    pData.State = ProcessData.ProcessState.Running;
                                xlist.Items[i] = pData;
                            }
                        }
                        else if (pData.State == ProcessData.ProcessState.Shutdown)
                        {
                            var elapsedSpan = new TimeSpan(DateTime.Now.Ticks - pData._time);
                            if (elapsedSpan.TotalSeconds > 1)
                            {
                                xlist.Items.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    catch { }
                }
                xlist.EndInit();
            });
            proc = null;
            return Task.CompletedTask;
        }

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public int Page { get; set; } = 0;

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

        void WatchProcess()
        {
            Task.Run(async () =>
            {
                await Task.Delay(500);
                ManagementEventWatcher startWatch = new ManagementEventWatcher(
                        new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
                startWatch.EventArrived
                                    += new EventArrivedEventHandler(startWatch_EventArrived);
                startWatch.Start();
                ManagementEventWatcher endWatch = new ManagementEventWatcher(
                 new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
                endWatch.EventArrived
                                    += new EventArrivedEventHandler(endWatch_EventArrived);
                endWatch.Start();
            });
        }

        private void endWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                int pid = int.Parse(e.NewEvent.Properties["ProcessID"].Value.ToString());
                for (int i = 0; i < xlist.Items.Count; i++)
                {
                    ProcessData pData = (ProcessData)xlist.Items[i];
                    if (pData.PID == pid)
                    {
                        pData.State = ProcessData.ProcessState.Shutdown;
                        pData._time = DateTime.Now.Ticks;
                        xlist.BeginInit();
                        xlist.Items[i] = pData;
                        xlist.EndInit();
                        break;
                    }
                }
            });
            e.NewEvent.Dispose();
        }

        private void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ProcessData p = GetProcessDataById(int.Parse(e.NewEvent.Properties["ProcessID"].Value.ToString()));
            if (p == null) return;
            Dispatcher.Invoke(() =>
            {
                try
                {
                    if (String.IsNullOrEmpty(p.Location))
                    {
                        var sii = new WinAPI.Shell32.SHSTOCKICONINFO();
                        sii.cbSize = (UInt32)Marshal.SizeOf(typeof(WinAPI.Shell32.SHSTOCKICONINFO));
                        WinAPI.Shell32.SHGetStockIconInfo(WinAPI.Shell32.SHSTOCKICONID.SIID_APPLICATION,
                            WinAPI.Shell32.SHGSI.SHGSI_ICON,
                            ref sii);
                        p.Icon = Imaging.CreateBitmapSourceFromHIcon(sii.hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                    else
                    {
                        FileToImageIconConverter fileToImage = new FileToImageIconConverter(p.Location);
                        p.Icon = fileToImage.Icon;
                    }
                    p.State = ProcessData.ProcessState.Create;
                    p._time = DateTime.Now.Ticks;
                    xlist.Items.Add(p);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            });
            e.NewEvent.Dispose();
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = topmost.IsChecked;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                xlist.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            xlist.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (xlist.SelectedItem != null)
            {
                ProcessData p = xlist.SelectedItem as ProcessData;
                try
                {
                    Process.GetProcessById(p.PID).Kill();
                }
                catch (Exception ex)
                {
                    MessageText.Text = ex.Message;
                    EndProcessDlg.ShowDialog(EndProcessDlg.Content);
                }
            }
        }

        private int GraphMax = 200;

        private void queryProcessInfo(Action<List<ManagementObject>, List<ManagementObject>> onSuccess)
        {
            Task.Run(() =>
            {
                long time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                ManagementObjectSearcher cpumos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                Debug.WriteLine("Query Processor Loaded:" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - time));
                time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                ManagementObjectSearcher memorymos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
                Debug.WriteLine("Query Memory Loaded:" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - time));
                time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                List<ManagementObject> cpuQuery = new List<ManagementObject>();
                List<ManagementObject> memoryQuery = new List<ManagementObject>();
                foreach (ManagementObject mo in cpumos.Get())
                {
                    cpuQuery.Add(mo);
                }
                foreach (ManagementObject mo in memorymos.Get())
                {
                    memoryQuery.Add(mo);
                }
                memoryQuery.TrimExcess();
                cpuQuery.TrimExcess();
                Dispatcher.Invoke(() => onSuccess(cpuQuery, memoryQuery));
                cpumos.Dispose();
                memorymos.Dispose();
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double MaxSpeed = 0;
            long time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            queryProcessInfo((cpuQuery, memoryQuery) =>
            {
                foreach (ManagementObject mo in cpuQuery)
                {
                    MaxSpeed = double.Parse(mo["CurrentClockSpeed"].ToString()) / 1000;
                    cputype.Content = mo["Name"];
                    CurrentClockSpeed.Content = MaxSpeed + " GHz";
                    NumberOfCores.Content = mo["NumberOfCores"];
                    NumberOfLogicalProcessors.Content = mo["NumberOfLogicalProcessors"];
                    mo.Dispose();
                }
                Debug.WriteLine("MOS Processor Sucessful read:" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - time));
                time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                foreach (ManagementObject mo in memoryQuery)
                {
                    ramlabel.Content = Math.Round(double.Parse(mo["Capacity"].ToString()) / 1024d / 1024d / 1024d) + " GB";
                    RAMSpeed.Content = mo["Speed"] + " MHz";
                    FormFactor.Content = PerformanceInfo.GetFormFactor(int.Parse(mo["FormFactor"].ToString()));
                    mo.Dispose();
                }

                cpuQuery = null;
                memoryQuery = null;

                var cancel = source.Token;

                this.Closing += Task_Manager_Closing;

                var CpuSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "CPU (%)",
                        Values = new ChartValues<ObservableValue>(),
                        PointGeometrySize = 0,
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Colors.White),
                        Fill = new SolidColorBrush(Color.FromArgb(30,255,255,255))
                    }
                };

                var RamSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "RAM (MB)",
                        Values = new ChartValues<ObservableValue>(),
                        PointGeometrySize = 0,
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Colors.White),
                        Fill = new SolidColorBrush(Color.FromArgb(30,255,255,255))
                    }
                };
                try
                {
                    PerformanceCounter cpuTotalTime = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    PerformanceCounter cpuPerformance = null;
                    try
                    {
                        cpuPerformance = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
                    }
                    catch { }
                    PerformanceCounter memoryCached = new PerformanceCounter("Memory", "Cache Bytes", "");
                    PerformanceCounter memoryCachedPeak = new PerformanceCounter("Memory", "Cache Bytes Peak", "");
                    PerformanceCounter CachedCore = new PerformanceCounter("Memory", "Standby Cache Core Bytes", "");
                    PerformanceCounter CachedReserve = new PerformanceCounter("Memory", "Standby Cache Reserve Bytes", "");
                    PerformanceCounter CachedResident = new PerformanceCounter("Memory", "System Cache Resident Bytes", "");
                    PerformanceCounter memoryCommited = new PerformanceCounter("Memory", "Committed Bytes", "");

                    ThreadStart observeFunc = delegate
                    {
                        for (int i = 0; i < GraphMax; i++)
                        {
                            if (i == GraphMax - 1)
                            {
                                CpuSeries[0].Values.Add(new ObservableValue(cpuTotalTime.NextValue()));
                                RamSeries[0].Values.Add(new ObservableValue(cpuTotalTime.NextValue()));
                            }
                            else
                            {
                                CpuSeries[0].Values.Add(new ObservableValue(double.NaN));
                                RamSeries[0].Values.Add(new ObservableValue(double.NaN));
                            }
                        };
                    };

                    observeFunc += delegate
                    {
                        Dispatcher.Invoke(() =>
                        {
                            CpuConterGraph.Series = CpuSeries;
                            RamConterGraph.Series = RamSeries;

                            RamConterGraph.AxisY[0].MaxValue = PerformanceInfo.GetTotalMemoryInMiB();
                        });


                        Task.Factory.StartNew(async () =>
                        {
                            while (!cancel.IsCancellationRequested)
                            {
                                float cpuTotal = cpuTotalTime.NextValue();
                                long ramAvailable = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                                long totalMemory = PerformanceInfo.GetTotalMemoryInMiB();
                                long ramUsage = totalMemory - ramAvailable;
                                double cached = Math.Round((memoryCached.NextValue() + memoryCachedPeak.NextValue() + CachedCore.NextValue() + CachedReserve.NextValue() + CachedResident.NextValue()) / 1024 / 1024);

                                Dispatcher.Invoke(() =>
                                {
                                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                                    CpuConterGraph.Series[0].Values.Add(new ObservableValue(cpuTotal));
                                    if (CpuConterGraph.Series[0].Values.Count > GraphMax) CpuConterGraph.Series[0].Values.RemoveAt(0);

                                    RamConterGraph.Series[0].Values.Add(new ObservableValue(ramUsage));
                                    if (RamConterGraph.Series[0].Values.Count > GraphMax) RamConterGraph.Series[0].Values.RemoveAt(0);

                                    cpulabel.Content = Math.Round(cpuTotal) + "%";
                                    cpuheader.Content = Math.Round(cpuTotal) + "%";
                                    if (cpuPerformance != null)
                                        cpuspeed.Content = Math.Round((cpuPerformance.NextValue() / 100d) * MaxSpeed, 2) + " GHz";
                                    ramusage.Content = $"{Math.Round((double)ramUsage)}MB/{totalMemory} MB";
                                    if (ramAvailable > 1024)
                                        Available.Content = $"{Math.Round(ramAvailable / 1024d, 2)} GB";
                                    else
                                        Available.Content = $"{ramAvailable} MB";
                                    Commited.Content = $"{Math.Round(memoryCommited.NextValue() / 1024 / 1024 / 1024, 1)} GB";
                                    if (cached > 1024)
                                        Cached.Content = $"{Math.Round(cached / 1024d, 2)} GB";
                                    else
                                        Cached.Content = $"{cached}MB";

                                    ramheader.Content = Math.Round((double)ramUsage / (double)totalMemory * 100d) + "%";
                                });
                                Cleaning();
                                await Task.Delay(1000);
                            }
                        }, cancel, TaskCreationOptions.LongRunning, PriorityScheduler.BelowNormal);
                    };

                    new Thread(observeFunc) { IsBackground = true }.Start();

                    maxram.Value = PerformanceInfo.GetTotalMemoryInMiB();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
        }

        private void Task_Manager_Closing(object sender, CancelEventArgs e)
        {
            source.Cancel();
            AppConfig.Current.TaskManagerSize = new Size(this.Width, this.Height);
            AppConfig.Current.TaskManagerPoint = new Point(this.Left, this.Top);
            AppConfig.Current.TaskManagerTopMost = topmost.IsChecked;
        }

        private void ShowMessageDialog(string Text)
        {
            MessageText.Text = Text;
            EndProcessDlg.ShowDialog(EndProcessDlg.Content);
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessData pData = (ProcessData)xlist.SelectedItem;
                if (String.IsNullOrWhiteSpace(pData.Location))
                {
                    ShowMessageDialog("Unable to locate file");
                    return;
                }
                Process.Start(new ProcessStartInfo()
                {
                    FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\explorer.exe",
                    Arguments = " /select,\"" + pData.Location + "\""
                });
            }
            catch (Exception ex)
            {
                ShowMessageDialog(ex.Message);
            }
        }

        private void Cleaning()
        {
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
        }

        private void endprocess_Click(object sender, RoutedEventArgs e)
        {
            if (xlist.SelectedItem != null)
            {
                ProcessData p = xlist.SelectedItem as ProcessData;
                try
                {
                    Process.GetProcessById(p.PID).Kill();
                }
                catch (Exception ex)
                {
                    ShowMessageDialog(ex.Message);
                }
            }
        }

        private void xlist_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            lv_action.IsEnabled = xlist.SelectedItems.Count > 0;
            lv_open.IsEnabled = xlist.SelectedItems.Count > 0;
            lv_properties.IsEnabled = xlist.SelectedItems.Count > 0;
        }

        private void properties_Click(object sender, RoutedEventArgs e)
        {
            if (xlist.SelectedItems.Count > 0)
            {
                try
                {
                    ProcessData pData = (ProcessData)xlist.SelectedItems[0];
                    if (String.IsNullOrWhiteSpace(pData.Location))
                    {
                        ShowMessageDialog("Unable to locate file");
                        return;
                    }

                    WinAPI.Shell32.SHELLEXECUTEINFO sei = new WinAPI.Shell32.SHELLEXECUTEINFO();
                    sei.cbSize = Marshal.SizeOf(sei);
                    sei.lpVerb = "properties";
                    sei.lpFile = pData.Location;
                    sei.nShow = WinAPI.Shell32.ShowCommands.SW_SHOW;
                    sei.fMask = WinAPI.Shell32.ShellExecuteMaskFlags.SEE_MASK_INVOKEIDLIST;
                    WinAPI.Shell32.ShellExecuteEx(ref sei);
                }
                catch (Exception ex)
                {
                    ShowMessageDialog(ex.Message);
                }
            }

        }

        private void exitMenuClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RunMenuClick(object sender, RoutedEventArgs e)
        {
            IntPtr windowHandle = IntPtr.Zero;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].GetType() == this.GetType())
                {
                    windowHandle = new WindowInteropHelper(Application.Current.Windows[i]).Handle;
                }
            }

            WinAPI.Shell32.SHRunFileDialog(windowHandle, IntPtr.Zero, null, "Raden Booster Run", null, 0);
        }

        private void Status_Checked(object sender, RoutedEventArgs e)
        {
            lh_status.Visibility = Visibility.Visible;
        }

        private void Status_Unchecked(object sender, RoutedEventArgs e)
        {
            lh_status.Visibility = Visibility.Collapsed;
        }

        private void lv_resume_Click(object sender, RoutedEventArgs e)
        {
            if (xlist.SelectedItems.Count > 0)
            {
                ProcessData pData = xlist.SelectedItem as ProcessData;
                try
                {
                    var process = Process.GetProcessById(pData.PID);
                    for (int i=0; i < process.Threads.Count; i++)
                    {
                        ProcessThread thread = process.Threads[i];
                        IntPtr TH = WinAPI.Kernel32.OpenThread(WinAPI.Kernel32.ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                        if (TH != IntPtr.Zero)
                        {
                            WinAPI.Kernel32.ResumeThread(TH);
                            WinAPI.Kernel32.CloseHandle(TH);
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowMessageDialog(ex.Message);
                }
            }
        }

        private void lv_suspend_Click(object sender, RoutedEventArgs e)
        {
            if (xlist.SelectedItems.Count > 0)
            {
                ProcessData pData = xlist.SelectedItem as ProcessData;
                try
                {
                    var process = Process.GetProcessById(pData.PID);
                    for (int i = 0; i < process.Threads.Count; i++)
                    {
                        ProcessThread thread = process.Threads[i];
                        IntPtr TH = WinAPI.Kernel32.OpenThread(WinAPI.Kernel32.ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                        if (TH != IntPtr.Zero)
                        {
                            WinAPI.Kernel32.SuspendThread(TH);
                            WinAPI.Kernel32.CloseHandle(TH);
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowMessageDialog(ex.Message);
                }
            }
        }

        private void lv_restart_Click(object sender, RoutedEventArgs e)
        {
            if (xlist.SelectedItems.Count > 0)
            {
                ProcessData pData = xlist.SelectedItem as ProcessData;
                try
                {
                    var process = Process.GetProcessById(pData.PID);
                    string szExePath = process.MainModule.FileName;

                    process.Kill();
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = szExePath
                    });
                }
                catch (Exception ex)
                {
                    ShowMessageDialog(ex.Message);
                }
            }
        }
    }
}
