using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Raden_Booster.Utils
{
    public class ProcessData
    {
        public enum ProcessState : int
        {
            Running,
            Create,
            Shutdown
        }
        public long _time { get; set; }
        public ProcessState State { get; set; }
        public string Title { get; set; }
        public ImageSource Icon { get; set; }
        public string Name { get; set; }
        public int PID { get; set; }
        public string Status { get; set; }
        public long Memory { get; set; }
        public string Location { get; set; }
        public string CommandLine { get; set; }
        public int ParentProcessId { get; set; }
    }

}
