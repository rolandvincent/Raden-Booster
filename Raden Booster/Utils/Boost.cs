using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WinAPI;

namespace Raden_Booster.Utils
{
    internal class Boost
    {
        private struct MEMORY_COMBINE_INFORMATION_EX
        {
            public IntPtr Handle;
            public UIntPtr PagesCombined;
            public ulong Flags;
        };

        private struct SYSTEM_CACHE_INFORMATION
        {
            public uint CurrentSize;
            public uint PeakSize;
            public uint PageFaultCount;
            public uint MinimumWorkingSet;
            public uint MaximumWorkingSet;
            public uint Unused1;
            public uint Unused2;
            public uint Unused3;
            public uint Unused4;
        }

        private struct SYSTEM_CACHE_INFORMATION_64_BIT
        {
            public long CurrentSize;
            public long PeakSize;
            public long PageFaultCount;
            public long MinimumWorkingSet;
            public long MaximumWorkingSet;
            public long Unused1;
            public long Unused2;
            public long Unused3;
            public long Unused4;
        }

        private enum SYSTEM_MEMORY_LIST_COMMAND : int
        {
            MemoryCaptureAccessedBits,
            MemoryCaptureAndResetAccessedBits,
            MemoryEmptyWorkingSets,
            MemoryFlushModifiedList,
            MemoryPurgeStandbyList,
            MemoryPurgeLowPriorityStandbyList,
            MemoryCommandMax
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        public enum BoostOptions
        {
            ALL = 0,
            SAFE = 1,
            DEFAULT = 2
        }

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("NTDLL.dll", SetLastError = true)]
        internal static extern int NtSetSystemInformation(int SystemInformationClass, IntPtr SystemInfo, int SystemInfoLength);

        private const int SE_PRIVILEGE_ENABLED = 0x00000002;
        private const string SE_INCREASE_QUOTA_NAME = "SeIncreaseQuotaPrivilege";
        private const string SE_PROFILE_SINGLE_PROCESS_NAME = "SeProfileSingleProcessPrivilege";

        private static bool SetIncreasePrivilege(string privilegeName)
        {
            try
            {
                using (WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent(TokenAccessLevels.AdjustPrivileges | TokenAccessLevels.Query))
                {
                    bool retVal;
                    TokPriv1Luid tp;
                    tp.Count = 1;
                    tp.Luid = 0;
                    tp.Attr = SE_PRIVILEGE_ENABLED;
                    retVal = LookupPrivilegeValue(null, privilegeName, ref tp.Luid);
                    if (!retVal) throw new Exception("Error in LookupPrivilegeValue: ", new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()));
                    retVal = AdjustTokenPrivileges(currentIdentity.Token, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
                    if (!retVal) throw new Exception("Error in AdjustTokenPrivileges: ", new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()));
                    return retVal;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SetIncreasePrivilege(" + privilegeName + "):", ex);
            }
        }

        #region Combine Memory List
        public static void REDUCT_COMBINE_MEMORY_LISTS()
        {
            bool success = SetIncreasePrivilege(SE_PROFILE_SINGLE_PROCESS_NAME);
            if (!success)
                throw new Exception("Failed SE_INCREASE_QUOTA_NAME");

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 10)
            {
                MEMORY_COMBINE_INFORMATION_EX combine_info_ex = new MEMORY_COMBINE_INFORMATION_EX();

                var length = Marshal.SizeOf(combine_info_ex);
                GCHandle ghc = GCHandle.Alloc(combine_info_ex, GCHandleType.Pinned);

                Ntdll.NtStatus status = (Ntdll.NtStatus)NtSetSystemInformation((int)Ntdll.SYSTEM_INFORMATION_CLASS.SystemCombinePhysicalMemoryInformation, ghc.AddrOfPinnedObject(), length);
                ghc.Free();
                if (status != Ntdll.NtStatus.Success)
                {
                    throw new Exception("MEMORY_COMBINE_INFORMATION_EX : " + status.ToString());
                }
            }
        } 
        #endregion

        #region System Working Set
        public static void REDUCT_SYSTEM_WORKING_SET()
        {
            bool success = SetIncreasePrivilege(SE_INCREASE_QUOTA_NAME);
            if (!success)
                throw new Exception("Failed SE_INCREASE_QUOTA_NAME");

            if (Environment.Is64BitOperatingSystem)
            {
                SYSTEM_CACHE_INFORMATION_64_BIT sfci = new SYSTEM_CACHE_INFORMATION_64_BIT();

                sfci.MinimumWorkingSet = -1;
                sfci.MaximumWorkingSet = -1;

                var length = Marshal.SizeOf(sfci);
                GCHandle ghc = GCHandle.Alloc(sfci, GCHandleType.Pinned);

                Ntdll.NtStatus status = (Ntdll.NtStatus)NtSetSystemInformation((int)Ntdll.SYSTEM_INFORMATION_CLASS.SystemFileCacheInformation, ghc.AddrOfPinnedObject(), length);
                ghc.Free();
                if (status != Ntdll.NtStatus.Success)
                {
                    throw new Exception("REDUCT_SYSTEM_WORKING_SET : " + status.ToString());
                }
            }
            else
            {
                SYSTEM_CACHE_INFORMATION sfci = new SYSTEM_CACHE_INFORMATION();

                sfci.MinimumWorkingSet = uint.MaxValue;
                sfci.MaximumWorkingSet = uint.MaxValue;

                var length = Marshal.SizeOf(sfci);
                GCHandle ghc = GCHandle.Alloc(sfci, GCHandleType.Pinned);

                Ntdll.NtStatus status = (Ntdll.NtStatus)NtSetSystemInformation((int)Ntdll.SYSTEM_INFORMATION_CLASS.SystemFileCacheInformation, ghc.AddrOfPinnedObject(), length);
                ghc.Free();
                if (status != Ntdll.NtStatus.Success)
                {
                    throw new Exception("REDUCT_SYSTEM_WORKING_SET : " + status.ToString());
                }
            }

        }
        #endregion

        #region Working Set
        public static void REDUCT_WORKING_SET()
        {
            bool success = SetIncreasePrivilege(SE_PROFILE_SINGLE_PROCESS_NAME);
            if (!success)
                throw new Exception("Failed SE_PROFILE_SINGLE_PROCESS_NAME");

            int standby = (int)SYSTEM_MEMORY_LIST_COMMAND.MemoryEmptyWorkingSets;
            int iSize = Marshal.SizeOf(standby);
            GCHandle gch = GCHandle.Alloc(standby, GCHandleType.Pinned);

            Ntdll.NtStatus status = (Ntdll.NtStatus)NtSetSystemInformation((int)Ntdll.SYSTEM_INFORMATION_CLASS.SystemMemoryListInformation, gch.AddrOfPinnedObject(), iSize);
            gch.Free();

            if (status != Ntdll.NtStatus.Success)
            {
                throw new Exception("REDUCT_STANDBY_LIST" + status.ToString());
            }
        }
        #endregion

        #region StandBy Priority List
        public static void REDUCT_STANDBY_PRIORITY0_LIST()
        {
            bool success = SetIncreasePrivilege(SE_PROFILE_SINGLE_PROCESS_NAME);
            if (!success)
                throw new Exception("Failed SE_PROFILE_SINGLE_PROCESS_NAME");

            int standby = (int)SYSTEM_MEMORY_LIST_COMMAND.MemoryPurgeLowPriorityStandbyList;
            int iSize = Marshal.SizeOf(standby);
            GCHandle gch = GCHandle.Alloc(standby, GCHandleType.Pinned);

            Ntdll.NtStatus status = (Ntdll.NtStatus)NtSetSystemInformation((int)Ntdll.SYSTEM_INFORMATION_CLASS.SystemMemoryListInformation, gch.AddrOfPinnedObject(), iSize);
            gch.Free();

            if (status != Ntdll.NtStatus.Success)
            {
                throw new Exception("REDUCT_STANDBY_LIST" + status.ToString());
            }
        }

        #endregion

        #region StandBy List
        public static void REDUCT_STANDBY_LIST()
        {
            bool success = SetIncreasePrivilege(SE_PROFILE_SINGLE_PROCESS_NAME);
            if (!success)
                throw new Exception("Failed SE_PROFILE_SINGLE_PROCESS_NAME");

            int standby = (int)SYSTEM_MEMORY_LIST_COMMAND.MemoryPurgeStandbyList;
            int iSize = Marshal.SizeOf(standby);
            GCHandle gch = GCHandle.Alloc(standby, GCHandleType.Pinned);

            Ntdll.NtStatus status = (Ntdll.NtStatus)NtSetSystemInformation((int)Ntdll.SYSTEM_INFORMATION_CLASS.SystemMemoryListInformation, gch.AddrOfPinnedObject(), iSize);
            gch.Free();

            if (status != Ntdll.NtStatus.Success)
            {
                throw new Exception("REDUCT_STANDBY_LIST" + status.ToString());
            }
        }
        #endregion

        #region Modified List
        public static void REDUCT_MODIFIED_LIST()
        {
            bool success = SetIncreasePrivilege(SE_PROFILE_SINGLE_PROCESS_NAME);
            if (!success)
                throw new Exception("Failed SE_PROFILE_SINGLE_PROCESS_NAME");

            int standby = (int)SYSTEM_MEMORY_LIST_COMMAND.MemoryFlushModifiedList;
            int iSize = Marshal.SizeOf(standby);
            GCHandle gch = GCHandle.Alloc(standby, GCHandleType.Pinned);

            Ntdll.NtStatus status = (Ntdll.NtStatus)NtSetSystemInformation((int)Ntdll.SYSTEM_INFORMATION_CLASS.SystemMemoryListInformation, gch.AddrOfPinnedObject(), iSize);
            gch.Free();

            if (status != Ntdll.NtStatus.Success)
            {
                throw new Exception("REDUCT_STANDBY_LIST" + status.ToString());
            }
        } 
        #endregion
    }
}
