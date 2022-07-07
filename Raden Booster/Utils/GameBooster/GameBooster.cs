using Raden_Booster.Utils.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raden_Booster.Utils.GameBooster
{
    internal class GameBooster : IDisposable
    {
        private static GameBooster _current = null;
        public static GameBooster Current {
            get
            {
                if (_current == null)
                    _current = new GameBooster();
                    return _current;
            }
            private set
            {
                _current = value;
            }
        }

        private List<string> _games;
        private List<string> _exception;

        public delegate void OnGameLaunched(GameBooster sender, GameLaunchedEventArgs e);

        public event OnGameLaunched GameLaunched;

        private CancellationTokenSource _cancelToken;
        ManagementEventWatcher startWatch;

        public GameBooster()
        {
            _cancelToken = new CancellationTokenSource();
            UpdateDatabase();
        }

        public void Start()
        {
            if (_cancelToken.IsCancellationRequested)
            {
                _cancelToken = new CancellationTokenSource();
                StartMonitoring();
                startWatch?.Start();
            }
        }

        public void Stop()
        {
            _cancelToken.Cancel();
            startWatch?.Stop();
        }

        public void UpdateDatabase()
        {
            _games = AppConfig.Current.Games.ToList();
            Debug.WriteLine(String.Join("\n", _games));
        }

        private void StartMonitoring()
        {
            Task.Factory.StartNew(async() =>
            {
                while (!_cancelToken.Token.IsCancellationRequested)
                {

                    await Task.Delay(1000);
                }
            }, _cancelToken.Token, TaskCreationOptions.LongRunning, PriorityScheduler.BelowNormal);
        }

        void WatchProcess()
        {
            Task.Run(() =>
            {
                startWatch = new ManagementEventWatcher(
                        new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
                startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
                startWatch.Start();
            });
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

                    pData = new ProcessData
                    {
                        State = ProcessData.ProcessState.Running,
                        Name = obj["Name"]?.ToString(),
                        PID = int.Parse(obj["ProcessId"]?.ToString()),
                        Location = obj["ExecutablePath"]?.ToString(),
                        Memory = int.Parse(obj["WorkingSetSize"]?.ToString()),
                    };
                    obj.Dispose();
                }
                catch { return null; }
                objects.Dispose();
            }
            searcher.Dispose();
            return pData;
        }

        private void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ProcessData pGame = GetProcessDataById(int.Parse(e.NewEvent.Properties["ProcessID"].Value.ToString()));
            GameLaunched?.Invoke(this, new GameLaunchedEventArgs(pGame));
            e.NewEvent.Dispose();
        }

        public void Dispose()
        {
            Stop();
            _games = null;
            _exception = null;
            startWatch.Dispose();
        }
    }
}
