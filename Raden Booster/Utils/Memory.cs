using System;
using System.Runtime.InteropServices;

namespace Raden_Booster
{
    public static class PerformanceInfo
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        public static Int64 GetPhysicalAvailableMemoryInMiB()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }
        }

        public static Int64 GetTotalMemoryInMiB()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }
        }
        public static double GetCPUUsagePercent()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return (pi.PhysicalTotal.ToInt64() - pi.PhysicalAvailable.ToInt64()) / (double)pi.PhysicalTotal.ToInt64();
            }
            else
            {
                return -1;
            }
        }

        public static string GetFormFactor(int _formFactor)
        {
            string formFactor = string.Empty;

            switch (_formFactor)
            {
                case 1:
                    formFactor = "Other";
                    break;

                case 2:
                    formFactor = "SIP";
                    break;

                case 3:
                    formFactor = "DIP";
                    break;

                case 4:
                    formFactor = "ZIP";
                    break;

                case 5:
                    formFactor = "SOJ";
                    break;

                case 6:
                    formFactor = "Proprietary";
                    break;

                case 7:
                    formFactor = "SIMM";
                    break;

                case 8:
                    formFactor = "DIMM";
                    break;

                case 9:
                    formFactor = "TSOP";
                    break;

                case 10:
                    formFactor = "PGA";
                    break;

                case 11:
                    formFactor = "RIMM";
                    break;

                case 12:
                    formFactor = "SODIMM";
                    break;

                case 13:
                    formFactor = "SRIMM";
                    break;

                case 14:
                    formFactor = "SMD";
                    break;

                case 15:
                    formFactor = "SSMP";
                    break;

                case 16:
                    formFactor = "QFP";
                    break;

                case 17:
                    formFactor = "TQFP";
                    break;

                case 18:
                    formFactor = "SOIC";
                    break;

                case 19:
                    formFactor = "LCC";
                    break;

                case 20:
                    formFactor = "PLCC";
                    break;

                case 21:
                    formFactor = "BGA";
                    break;

                case 22:
                    formFactor = "FPBGA";
                    break;

                case 23:
                    formFactor = "LGA";
                    break;

                default:
                    formFactor = "Unknown";
                    break;
            }

            return formFactor;
        }
    }
}
