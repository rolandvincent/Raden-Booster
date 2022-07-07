using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;

namespace Raden_Booster
{
    public class TempratureInfo
    {
        public struct TempratureData
        {
            public TempratureData(string Name, float? value)
            {
                HardwareName = Name;
                Value = value;
            }
            public string HardwareName;
            public float? Value;
        }
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        private static Computer computer = null;
        public static void Close()
        {
            if (computer != null)
                computer.Close();
        }
        public static List<TempratureData> GetSystemInfo()
        {
            List<TempratureData> tempratures = new List<TempratureData>();

            UpdateVisitor updateVisitor = new UpdateVisitor();
            if (computer == null)
            {
                computer = new Computer();
                computer.Open();
                computer.CPUEnabled = true;
                computer.GPUEnabled = true;
                computer.HDDEnabled = true;
            }
            computer.Accept(updateVisitor);
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                {
                    if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        tempratures.Add(new TempratureData(computer.Hardware[i].Sensors[j].Name, computer.Hardware[i].Sensors[j].Value));
                }
            }
            tempratures.TrimExcess();
            return tempratures;
        }
    }

}
