using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raden_Booster.Utils.GameBooster
{
    internal class GameLaunchedEventArgs : EventArgs
    {
        public ProcessData Process;
        public DateTime StartDate;

        public GameLaunchedEventArgs(ProcessData process)
        {
            Process = process;
            StartDate = DateTime.Now;
        }
    }
}
