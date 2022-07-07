using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Raden_Booster.Utils.Config;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for Temprature.xaml
    /// </summary>
    public partial class Temprature : Page
    {
        internal CancellationTokenSource _cancelToken = new CancellationTokenSource();
        public Temprature()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void LoadSettings()
        {
            temp_monitor.IsChecked = AppConfig.Current.TempMonitor;
        }

        public void StartMonitoring()
        {
            if (TempraturePanel == null) return;
            if (TempraturePanel.Children.Count == 0) return;
            _cancelToken = new CancellationTokenSource();
            var _cancelationToken = _cancelToken.Token;
            Task.Run(async () =>
            {
                while (!_cancelationToken.IsCancellationRequested)
                {
                    double memoryPercentage = (double)(PerformanceInfo.GetTotalMemoryInMiB() - PerformanceInfo.GetPhysicalAvailableMemoryInMiB()) / PerformanceInfo.GetTotalMemoryInMiB() * 100;

                    updateTemprature();

                    await Task.Delay(1000);
                }
            }, _cancelationToken);
        }

        private int GraphMax = 100;

        public StackPanel GetTempraturePanel()
        {
            return TempraturePanel;
        }

        private void checkTheTemprature()
        {
            TempraturePanel.Children.Clear();
            FontFamily fontfamily = new FontFamily("Bahnschrift");

            var tempratureInfo = TempratureInfo.GetSystemInfo();
            Debug.WriteLine($"TOTAL TEMP {tempratureInfo.Count}");
            foreach (TempratureInfo.TempratureData data in tempratureInfo)
            {
                Grid HorizontalSP = new Grid()
                {
                    Margin = new Thickness(0, 0, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };

                Label nameLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = data.HardwareName == "Temperature" ? "Disk" : data.HardwareName,
                    Margin = new Thickness(3, 0, 0, 0),
                    Foreground = (SolidColorBrush)Application.Current.Resources["WindowForeground"],
                    FontFamily = fontfamily,
                    Width = 80
                };

                Label valueLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = (data.Value?.ToString() ?? "-") + " °C",
                    Margin = new Thickness(100, 0, 0, 0),
                    FontFamily = fontfamily,
                    Foreground = new SolidColorBrush(Colors.White),
                    Width = 60
                };

                var SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Temprature (°C)",
                        Values = new ChartValues<ObservableValue>(),
                        PointGeometrySize = 0,
                        StrokeThickness = 2,
                        Stroke = new SolidColorBrush(Colors.White),
                        Fill = new SolidColorBrush(Color.FromArgb(30,255,255,255))
                    }
                };

                for (int i = 0; i < GraphMax; i++)
                {
                    if (i == GraphMax - 1)
                        SeriesCollection[0].Values.Add(new ObservableValue(data.Value ?? double.NaN));
                    else
                        SeriesCollection[0].Values.Add(new ObservableValue(double.NaN));
                };

                CartesianChart cartesianChart = new CartesianChart
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    DisableAnimations = true,
                    Height = 140,
                    Margin = new Thickness(150, 0, 0, 0),
                    AxisX = new AxesCollection
                    {
                        new Axis()
                        {
                            Title = "Time",
                        }
                    },
                    AxisY = new AxesCollection
                    {
                        new Axis()
                        {
                            Name = "Axis",
                            Title = "Temperature (°C)",
                            Sections = new SectionsCollection
                            {
                                new AxisSection()
                                {
                                    Value = 30,
                                    Opacity = 0.4,
                                    Fill = new SolidColorBrush(Colors.Green)
                                },
                                new AxisSection()
                                {
                                    Value = 80,
                                    Opacity = 0.4,
                                    Fill = new SolidColorBrush(Colors.Red)
                                }
                            }
                        }
                    },
                    Series = SeriesCollection
                };

                HorizontalSP.Children.Add(nameLabel);
                HorizontalSP.Children.Add(valueLabel);
                HorizontalSP.Children.Add(cartesianChart);

                TempraturePanel.Children.Add(HorizontalSP);
            }
        }

        private void updateTemprature()
        {
            try
            {
                var tempratures = TempratureInfo.GetSystemInfo();
                this.Dispatcher.Invoke(() =>
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                    foreach (Grid panel in TempraturePanel.Children)
                    {
                        var tmp = tempratures.Find(elm => (elm.HardwareName == "Temperature" ? "Disk" : elm.HardwareName) == ((Label)panel.Children[0]).Content.ToString());

                        ((Label)panel.Children[1]).Content = tmp.Value + " °C";
                        ((CartesianChart)panel.Children[2]).Series[0].Values.Add(new ObservableValue(tmp.Value ?? double.NaN));
                        if (((CartesianChart)panel.Children[2]).Series[0].Values.Count > GraphMax) ((CartesianChart)panel.Children[2]).Series[0].Values.RemoveAt(0);
                    }
                });
            }
            catch
            {}

        }

        private void temp_monitor_Checked_Changed(object sender, RoutedEventArgs e)
        {
            if (temp_monitor.IsChecked == true)
                StartMonitoring();
            else
                _cancelToken.Cancel();
            AppConfig.Current.TempMonitor = temp_monitor.IsChecked ?? true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (TempraturePanel.Children.Count == 0)
            {
                checkTheTemprature();
                if (temp_monitor.IsChecked == true)
                    StartMonitoring();
                else
                    _cancelToken.Cancel();
            }
        }
    }
}
