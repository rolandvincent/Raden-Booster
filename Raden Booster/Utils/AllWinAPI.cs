using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace WinAPI
{
    class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);
        public static IntPtr OpenProcess(Process proc, ProcessAccessFlags flags) => OpenProcess(flags, false, proc.Id);
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);
        [DllImport("kernel32.dll")]
        public static extern int GetProcessId(IntPtr handle);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Beep(uint dwFreq, uint dwDuration);
        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref Int32 pbCancel, CopyFileFlags dwCopyFlags);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot(SnapshotFlags dwFlags, uint th32ProcessID);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr CreateConsoleScreenBuffer(UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr secutiryAttributes, UInt32 flags, IntPtr screenBufferData);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false)]
        internal static extern bool CreateDirectory(String path, SECURITY_ATTRIBUTES lpSecurityAttributes);
        [DllImport("kernel32.dll")]
        public static extern bool CreateDirectoryEx(string lpTemplateDirectory, string lpNewDirectory, IntPtr? lpSecurityAttributes = null);
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);
        //[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern IntPtr CreateFile(
        //[MarshalAs(UnmanagedType.LPTStr)] string filename,
        //[MarshalAs(UnmanagedType.U4)] FileAccess access,
        //[MarshalAs(UnmanagedType.U4)] FileShare share,
        //IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
        //[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        //[MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
        //IntPtr templateFile);
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            int flags,
            IntPtr template);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr CreateFileA(
             [MarshalAs(UnmanagedType.LPStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] FileAccess access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes,
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             IntPtr templateFile);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateFileW(
             [MarshalAs(UnmanagedType.LPWStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] FileAccess access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes,
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             IntPtr templateFile);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CreateProcess(
           string lpApplicationName,
           string lpCommandLine,
           ref SECURITY_ATTRIBUTES lpProcessAttributes,
           ref SECURITY_ATTRIBUTES lpThreadAttributes,
           bool bInheritHandles,
           uint dwCreationFlags,
           IntPtr lpEnvironment,
           string lpCurrentDirectory,
           [In] ref STARTUPINFO lpStartupInfo,
           out PROCESS_INFORMATION lpProcessInformation);
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateThread([In] ref SECURITY_ATTRIBUTES
           SecurityAttributes, uint StackSize, System.Threading.ThreadStart StartFunction,
           IntPtr ThreadParameter, uint CreationFlags, out uint ThreadId);
        [DllImport("Advapi32.dll", SetLastError = true)]
        public static extern bool DecryptFile(string lpFileName, uint dwReserved);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string lpFileName);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFileA([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFileW([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
        [DllImport("kernel32.dll")]
        public static extern void DeleteCriticalSection(ref CRITICAL_SECTION lpCriticalSection);
        [DllImport("kernel32.dll")]
        public static extern bool DeleteVolumeMountPoint(string lpszVolumeMountPoint);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle,
           IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
           uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);
        [DllImport("kernel32.dll")]
        public static extern void EnterCriticalSection(ref CRITICAL_SECTION lpCriticalSection);
        [DllImport("kernel32.dll")]
        public static extern void ExitProcess(uint uExitCode);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindFirstFileEx(
            string lpFileName,
            FINDEX_INFO_LEVELS fInfoLevelId,
            out WIN32_FIND_DATA lpFindFileData,
            FINDEX_SEARCH_OPS fSearchOp,
            IntPtr lpSearchFilter,
            int dwAdditionalFlags);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern FindVolumeSafeHandle FindFirstVolume([Out] StringBuilder lpszVolumeName, uint cchBufferLength); [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FindNextVolume(FindVolumeSafeHandle hFindVolume, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FindVolumeClose(IntPtr hFindVolume);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResourceEx(IntPtr hModule, IntPtr lpType, IntPtr lpName, ushort wLanguage);
        [DllImport("kernel32.dll")]
        public static extern bool FlushConsoleInputBuffer(IntPtr hConsoleInput);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool FreeConsole();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetCommandLine();
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleProcessList(uint[] ProcessList, uint ProcessCount);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleHistoryInfo(out CONSOLE_HISTORY_INFO ConsoleHistoryInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetFileAttributesEx(string lpFileName, GET_FILEEX_INFO_LEVELS fInfoLevelId, out WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);
        [DllImport("kernel32.dll")]
        public static extern DriveType GetDriveType([MarshalAs(UnmanagedType.LPStr)] string lpRootPathName);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandleEx(IntPtr hFile, FILE_INFO_BY_HANDLE_CLASS infoClass, out FILE_ID_BOTH_DIR_INFO dirInfo, uint dwBufferSize);
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
        [DllImport("kernel32.dll", SetLastError = true)]
        [PreserveSig]
        public static extern uint GetModuleFileName([In] IntPtr hModule, [Out] StringBuilder lpFilename, [In][MarshalAs(UnmanagedType.U4)] int nSize);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32", SetLastError = true, EntryPoint = "GetProcAddress")]
        public static extern IntPtr GetProcAddressOrdinal(IntPtr hModule, IntPtr procName);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetProcessIdOfThread(IntPtr handle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
#pragma warning disable CS0618 // 'FILETIME' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202'
        public static extern bool GetProcessTimes(IntPtr hProcess, out FILETIME
#pragma warning restore CS0618 // 'FILETIME' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202'
#pragma warning disable CS0618 // 'FILETIME' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202'
#pragma warning disable CS0618 // 'FILETIME' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202'
           lpCreationTime, out FILETIME lpExitTime, out FILETIME lpKernelTime,
#pragma warning restore CS0618 // 'FILETIME' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202'
#pragma warning restore CS0618 // 'FILETIME' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202'
#pragma warning disable CS0618 // 'FILETIME' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202'
           out FILETIME lpUserTime);
#pragma warning restore CS0618 // 'FILETIME' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202'
        [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Auto)]
        public static extern void GetStartupInfo(out STARTUPINFO lpStartupInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern void GetSystemInfo(ref SYSTEM_INFO Info);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool GetVolumeInformation(
           string rootPathName,
           StringBuilder volumeNameBuffer,
           int volumeNameSize,
           out uint volumeSerialNumber,
           out uint maximumComponentLength,
           out FileSystemFeature fileSystemFlags,
           StringBuilder fileSystemNameBuffer,
           int nFileSystemNameSize);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalFree(IntPtr hMem);
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid hProcess, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);
        [DllImport("kernel32.dll")]
        public static extern bool LockFile(IntPtr hFile, uint dwFileOffsetLow, uint
           dwFileOffsetHigh, uint nNumberOfBytesToLockLow, uint
           nNumberOfBytesToLockHigh);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, MoveFileFlags dwFlags);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool MoveFile(string lpExistingFileName, string lpNewFileName, MoveFileFlags dwFlags);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool MoveFileWithProgress(string lpExistingFileName,
           string lpNewFileName, CopyProgressRoutine lpProgressRoutine,
           IntPtr lpData, MoveFileFlags dwFlags);
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void MoveMemory(IntPtr dest, IntPtr src, int size);
        [DllImport("kernel32.dll")]
        public static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);
        [DllImport("kernel32.dll")]
        public static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);
        [DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool Process32First([In] IntPtr hSnapshot, ref PROCESSENTRY32 lppe);
        [DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool Process32Next([In] IntPtr hSnapshot, ref PROCESSENTRY32 lppe);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle,
           uint dwThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadConsoleOutputCharacter(
            IntPtr hConsoleOutput,
            StringBuilder lpCharacter,
            uint nLength,
            COORD dwReadCoord,
            out uint lpNumberOfCharsRead);
        [DllImport("kernel32.dll")]
        public static extern bool ReadFileScatter(IntPtr hFile, FILE_SEGMENT_ELEMENT[]
           aSegementArray, uint nNumberOfBytesToRead, IntPtr lpReserved,
           [In] ref System.Threading.NativeOverlapped lpOverlapped);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool RemoveDirectory(string lpPathName);
        [DllImport("Kernel32.dll", EntryPoint = "RtlSecureZeroMemory", SetLastError = false)]
        public static extern void SecureZeroMemory(IntPtr dest, IntPtr size);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SearchPath(string lpPath,
            string lpFileName,
            string lpExtension,
            int nBufferLength,
            [MarshalAs ( UnmanagedType.LPTStr )]
            StringBuilder lpBuffer,
            out IntPtr lpFilePart);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleHistoryInfo(CONSOLE_HISTORY_INFO ConsoleHistoryInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleIcon(IntPtr hIcon);
        [DllImport("kernel32.dll")]
        public static extern bool SetCurrentDirectory(string lpPathName);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(string lpPathName);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetEndOfFile(IntPtr hFile);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void SetLastError(uint dwErrorCode);
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool SetFileInformationByHandle(SafeHandle hFile, FileInformationClass FileInformationClass, ref FileInformation FileInformation, Int32 dwBufferSize);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetFileAttributesW(string lpFileName, FileAttributes dwFileAttributes);
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMinimumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);
        [DllImport("kernel32.dll")]
        public static extern bool SetVolumeMountPoint(string lpszVolumeMountPoint, string lpszVolumeName);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetVolumeNameForVolumeMountPoint(
            string volumeName,
            StringBuilder uniqueVolumeName,
            int uniqueNameBufferCapacity);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
        [DllImport("kernel32.dll")]
        public static extern bool TerminateThread(IntPtr hThread, uint dwExitCode);
        [DllImport("kernel32.dll")]
        public static extern bool Thread32First(IntPtr hSnapshot, ref THREADENTRY32 lpte);
        [DllImport("kernel32.dll")]
        public static extern bool Toolhelp32ReadProcessMemory(uint th32ProcessID,
           IntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr cbRead,
           IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern bool TryEnterCriticalSection(ref CRITICAL_SECTION lpCriticalSection);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteConsoleOutputCharacter(
           IntPtr hConsoleOutput,
           StringBuilder lpCharacter,
           uint nLength,
           COORD dwWriteCoord,
           out uint lpNumberOfCharsWritten);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
          IntPtr hProcess,
          IntPtr lpBaseAddress,
          byte[] lpBuffer,
          Int32 nSize,
          out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
          IntPtr hProcess,
          IntPtr lpBaseAddress,
          [MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
          int dwSize,
          out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CreateFileW")]
        public static extern IntPtr GetInputBuffer(
            [MarshalAs(UnmanagedType.LPWStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] UInt32 access,
            [MarshalAs(UnmanagedType.U4)] UInt32 share,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)]
                UInt32 creationDisposition,
            [MarshalAs(UnmanagedType.U4)] UInt32 flagsAndAttributes,
            IntPtr templateFile);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetCurrentProcessorNumber();
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LocalAlloc([In] uint uFlags, [In] uint uBytes);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll")]
        public static extern bool Heap32ListFirst(IntPtr hSnapshot, ref HEAPLIST32 lphl);

        [DllImport("kernel32.dll")]
        public static extern bool Heap32ListNext(IntPtr hSnapshot, ref HEAPLIST32 lphl);

        [DllImport("kernel32.dll")]
        public static extern bool Heap32First(IntPtr hSnapshot, ref HEAPENTRY32 lphe,
            uint th32ProcessID, uint th32HeapID);

        [DllImport("kernel32.dll")]
        public static extern bool Heap32Next(IntPtr hSnapshot, ref HEAPENTRY32 lphe);

        [DllImport("kernel32.dll")]
        public static extern bool Toolhelp32ReadProcessMemory(uint th32ProcessID,
            IntPtr lpBaseAddress, IntPtr lpBuffer, uint cbRead, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool Process32FirstW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("kernel32.dll")]
        public static extern bool Process32NextW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("kernel32.dll")]
        public static extern bool Thread32Next(IntPtr hSnapshot, ref THREADENTRY32 lpte);

        [DllImport("kernel32.dll")]
        public static extern bool Module32FirstW(IntPtr hSnapshot, ref MODULEENTRY32W lpme);

        [DllImport("kernel32.dll")]
        public static extern bool Module32NextW(IntPtr hSnapshot, ref MODULEENTRY32W lpme);

        [DllImport("kernel32.dll")]
        public static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32W lpme);

        [DllImport("kernel32.dll")]
        public static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32W lpme);
        [DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
        public static extern void ZeroMemory(ref IntPtr dest, int size);
        #region Struct
        public struct HEAPLIST32
        {
            public uint dwSize;
            public uint th32ProcessID;
            public uint th32HeapID;
            public uint dwFlags;
        }

        public struct HEAPENTRY32
        {
            public uint dwSize;
            public IntPtr hHandle;
            public uint dwAddress;
            public uint dwBlockSize;
            public uint dwFlags;
            public uint dwLockCount;
            public uint dwResvd;
            public uint th32ProcessID;
            public uint th32HeapID;
        }

        public struct PROCESSENTRY32W
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public UIntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            public string szExeFile;
        }
        public struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public uint th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            public string szExeFile;
        }
        public struct MODULEENTRY32W
        {
            public uint dwSize;
            public uint th32ModuleID;
            public uint th32ProcessID;
            public uint GlblcntUsage;
            public uint ProccntUsage;
            public IntPtr modBaseAddr;
            public uint modBaseSize;
            public IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExePath;
        }

        public struct MODULEENTRY32
        {
            public uint dwSize;
            public uint th32ModuleID;
            public uint th32ProcessID;
            public uint GlblcntUsage;
            public uint ProccntUsage;
            public IntPtr modBaseAddr;
            public uint modBaseSize;
            public IntPtr hModule;
            public string szModule;
            public string szExePath;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FILE_BASIC_INFO
        {
            public Int64 CreationTime;
            public Int64 LastAccessTime;
            public Int64 LastWriteTime;
            public Int64 ChangeTime;
            public UInt32 FileAttributes;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FILE_DISPOSITION_INFO
        {
            public bool DeleteFile;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct FileInformation
        {
            [FieldOffset(0)]
            public FILE_BASIC_INFO FILE_BASIC_INFO;
            [FieldOffset(0)]
            public FILE_DISPOSITION_INFO FILE_DISPOSITION_INFO;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct THREADENTRY32
        {
            internal UInt32 dwSize;
            internal UInt32 cntUsage;
            internal UInt32 th32ThreadID;
            internal UInt32 th32OwnerProcessID;
            internal UInt32 tpBasePri;
            internal UInt32 tpDeltaPri;
            internal UInt32 dwFlags;
        }
        [StructLayout(LayoutKind.Explicit, Size = 8)]
        internal struct FILE_SEGMENT_ELEMENT
        {
            [FieldOffset(0)]
            public IntPtr Buffer;
            [FieldOffset(0)]
            public UInt64 Alignment;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            internal ushort wProcessorArchitecture;
            internal ushort wReserved;
            internal uint dwPageSize;
            internal IntPtr lpMinimumApplicationAddress;
            internal IntPtr lpMaximumApplicationAddress;
            internal IntPtr dwActiveProcessorMask;
            internal uint dwNumberOfProcessors;
            internal uint dwProcessorType;
            internal uint dwAllocationGranularity;
            internal ushort wProcessorLevel;
            internal ushort wProcessorRevision;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct LargeInteger
        {
            [FieldOffset(0)]
            public int Low;
            [FieldOffset(4)]
            public int High;
            [FieldOffset(0)]
            public long QuadPart;

            // use only when QuadPart canot be passed
            public long ToInt64()
            {
                return ((long)this.High << 32) | (uint)this.Low;
            }
            public static LargeInteger FromInt64(long value)
            {
                return new LargeInteger
                {
                    Low = (int)(value),
                    High = (int)((value >> 32))
                };
            }

        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FILE_ID_BOTH_DIR_INFO
        {
            public uint NextEntryOffset;
            public uint FileIndex;
            public LargeInteger CreationTime;
            public LargeInteger LastAccessTime;
            public LargeInteger LastWriteTime;
            public LargeInteger ChangeTime;
            public LargeInteger EndOfFile;
            public LargeInteger AllocationSize;
            public uint FileAttributes;
            public uint FileNameLength;
            public uint EaSize;
            public char ShortNameLength;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string ShortName;
            public LargeInteger FileId;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string FileName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct WIN32_FILE_ATTRIBUTE_DATA
        {
            public FileAttributes dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_HISTORY_INFO
        {
            ushort cbSize;
            ushort HistoryBufferSize;
            ushort NumberOfHistoryBuffers;
            uint dwFlags;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct CRITICAL_SECTION
        {
            public IntPtr DebugInfo;
            public int LockCount;
            public int RecursionCount;
            public IntPtr OwningThread;
            public IntPtr LockSemaphore;
            public UIntPtr SpinCount;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFOEX
        {
            public STARTUPINFO StartupInfo;
            public IntPtr lpAttributeList;
        }

        // This also works with CharSet.Ansi as long as the calling function uses the same character set.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SECURITY_ATTRIBUTES
        {
            internal int nLength = 0;
            // don't remove null, or it will fail to set the default ACL, making the folder inaccessible and non-removeable
            // unsafe is available if compile with /unsafe flag, https://docs.microsoft.com/en-us/dotnet/csharp/misc/cs0227
            public IntPtr? pSecurityDescriptor = null;
            //or
            //internal unsafe byte* pSecurityDescriptor = null;
            internal int bInheritHandle = 0;
            public IntPtr lpSecurityDescriptor;
        }
        #endregion

        #region Var
        public const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;
        public const int ERROR_ACCESS_DENIED = 5;
        public const int FIND_FIRST_EX_CASE_SENSITIVE = 1;
        public const int FIND_FIRST_EX_LARGE_FETCH = 2;
        public const int STD_OUTPUT_HANDLE = -11;
        public const int STD_INPUT_HANDLE = -10;
        public const int STD_ERROR_HANDLE = -12;
        public const int THREADACCESS_SUSPEND_RESUME = 0x0002;

        public const uint TH32CS_SNAPHEAPLIST = 0x00000001;
        public const uint TH32CS_SNAPPROCESS = 0x00000002;
        public const uint TH32CS_SNAPTHREAD = 0x00000004;
        public const uint TH32CS_SNAPMODULE = 0x00000008;
        public const uint TH32CS_SNAPMODULE32 = 0x00000010;
        public const uint TH32CS_SNAPALL = (TH32CS_SNAPHEAPLIST |
                                                 TH32CS_SNAPPROCESS |
                                                 TH32CS_SNAPTHREAD |
                                                 TH32CS_SNAPMODULE);
        public const uint TH32CS_INHERIT = 0x80000000;

        public const uint HF32_DEFAULT = 1;
        public const uint HF32_SHARED = 2;

        public const uint LF32_FIXED = 0x00000001;
        public const uint LF32_FREE = 0x00000002;
        public const uint LF32_MOVEABLE = 0x00000004;

        public static int pbCancel;
        #endregion

        public delegate CopyProgressResult CopyProgressRoutine(long TotalFileSize,
            long TotalBytesTransferred,
            long StreamSize,
            long StreamBytesTransferred,
            uint dwStreamNumber,
            CopyProgressCallbackReason dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData);

        #region Enum
        public enum FileInformationClass : int
        {
            FileBasicInfo = 0,
            FileStandardInfo = 1,
            FileNameInfo = 2,
            FileRenameInfo = 3,
            FileDispositionInfo = 4,
            FileAllocationInfo = 5,
            FileEndOfFileInfo = 6,
            FileStreamInfo = 7,
            FileCompressionInfo = 8,
            FileAttributeTagInfo = 9,
            FileIdBothDirectoryInfo = 10, // 0xA
            FileIdBothDirectoryRestartInfo = 11, // 0xB
            FileIoPriorityHintInfo = 12, // 0xC
            FileRemoteProtocolInfo = 13, // 0xD
            FileFullDirectoryInfo = 14, // 0xE
            FileFullDirectoryRestartInfo = 15, // 0xF
            FileStorageInfo = 16, // 0x10
            FileAlignmentInfo = 17, // 0x11
            FileIdInfo = 18, // 0x12
            FileIdExtdDirectoryInfo = 19, // 0x13
            FileIdExtdDirectoryRestartInfo = 20, // 0x14
        }
        [Flags]
        public enum FileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }
        [Flags]
        public enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            All = (HeapList | Process | Thread | Module),
            Inherit = 0x80000000,
            NoHeaps = 0x40000000

        }
        [Flags]
        public enum MoveFileFlags : uint
        {
            MOVE_FILE_REPLACE_EXISTSING = 0x00000001,
            MOVE_FILE_COPY_ALLOWED = 0x00000002,
            MOVE_FILE_DELAY_UNTIL_REBOOT = 0x00000004,
            MOVE_FILE_WRITE_THROUGH = 0x00000008,
            MOVE_FILE_CREATE_HARDLINK = 0x00000010,
            MOVE_FILE_FAIL_IF_NOT_TRACKABLE = 0x00000020
        }
        public enum CopyProgressCallbackReason : uint
        {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }
        public enum CopyProgressResult : uint
        {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }
        [Flags]
        public enum FileSystemFeature : uint
        {
            /// <summary>
            /// The file system preserves the case of file names when it places a name on disk.
            /// </summary>
            CasePreservedNames = 2,

            /// <summary>
            /// The file system supports case-sensitive file names.
            /// </summary>
            CaseSensitiveSearch = 1,

            /// <summary>
            /// The specified volume is a direct access (DAX) volume. This flag was introduced in Windows 10, version 1607.
            /// </summary>
            DaxVolume = 0x20000000,

            /// <summary>
            /// The file system supports file-based compression.
            /// </summary>
            FileCompression = 0x10,

            /// <summary>
            /// The file system supports named streams.
            /// </summary>
            NamedStreams = 0x40000,

            /// <summary>
            /// The file system preserves and enforces access control lists (ACL).
            /// </summary>
            PersistentACLS = 8,

            /// <summary>
            /// The specified volume is read-only.
            /// </summary>
            ReadOnlyVolume = 0x80000,

            /// <summary>
            /// The volume supports a single sequential write.
            /// </summary>
            SequentialWriteOnce = 0x100000,

            /// <summary>
            /// The file system supports the Encrypted File System (EFS).
            /// </summary>
            SupportsEncryption = 0x20000,

            /// <summary>
            /// The specified volume supports extended attributes. An extended attribute is a piece of
            /// application-specific metadata that an application can associate with a file and is not part
            /// of the file's data.
            /// </summary>
            SupportsExtendedAttributes = 0x00800000,

            /// <summary>
            /// The specified volume supports hard links. For more information, see Hard Links and Junctions.
            /// </summary>
            SupportsHardLinks = 0x00400000,

            /// <summary>
            /// The file system supports object identifiers.
            /// </summary>
            SupportsObjectIDs = 0x10000,

            /// <summary>
            /// The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
            /// </summary>
            SupportsOpenByFileId = 0x01000000,

            /// <summary>
            /// The file system supports re-parse points.
            /// </summary>
            SupportsReparsePoints = 0x80,

            /// <summary>
            /// The file system supports sparse files.
            /// </summary>
            SupportsSparseFiles = 0x40,

            /// <summary>
            /// The volume supports transactions.
            /// </summary>
            SupportsTransactions = 0x200000,

            /// <summary>
            /// The specified volume supports update sequence number (USN) journals. For more information,
            /// see Change Journal Records.
            /// </summary>
            SupportsUsnJournal = 0x02000000,

            /// <summary>
            /// The file system supports Unicode in file names as they appear on disk.
            /// </summary>
            UnicodeOnDisk = 4,

            /// <summary>
            /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
            /// </summary>
            VolumeIsCompressed = 0x8000,

            /// <summary>
            /// The file system supports disk quotas.
            /// </summary>
            VolumeQuotas = 0x20
        }
        public enum FILE_INFO_BY_HANDLE_CLASS
        {
            FileBasicInfo = 0,
            FileStandardInfo = 1,
            FileNameInfo = 2,
            FileRenameInfo = 3,
            FileDispositionInfo = 4,
            FileAllocationInfo = 5,
            FileEndOfFileInfo = 6,
            FileStreamInfo = 7,
            FileCompressionInfo = 8,
            FileAttributeTagInfo = 9,
            FileIdBothDirectoryInfo = 10,// 0x0A
            FileIdBothDirectoryRestartInfo = 11, // 0xB
            FileIoPriorityHintInfo = 12, // 0xC
            FileRemoteProtocolInfo = 13, // 0xD
            FileFullDirectoryInfo = 14, // 0xE
            FileFullDirectoryRestartInfo = 15, // 0xF
            FileStorageInfo = 16, // 0x10
            FileAlignmentInfo = 17, // 0x11
            FileIdInfo = 18, // 0x12
            FileIdExtdDirectoryInfo = 19, // 0x13
            FileIdExtdDirectoryRestartInfo = 20, // 0x14
            MaximumFileInfoByHandlesClass
        }
        public enum GET_FILEEX_INFO_LEVELS
        {
            GetFileExInfoStandard,
            GetFileExMaxInfoLevel
        }
        public enum FINDEX_SEARCH_OPS
        {
            FindExSearchNameMatch = 0,
            FindExSearchLimitToDirectories = 1,
            FindExSearchLimitToDevices = 2
        }
        public enum FINDEX_INFO_LEVELS
        {
            FindExInfoStandard = 0,
            FindExInfoBasic = 1
        }
        [Flags]
        public enum DuplicateOptions : uint
        {
            DUPLICATE_CLOSE_SOURCE = (0x00000001),// Closes the source handle. This occurs regardless of any error status returned.
            DUPLICATE_SAME_ACCESS = (0x00000002), //Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the source handle.
        }
        [Flags]
        public enum CopyFileFlags : uint
        {
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
        [Flags]
        enum EFileAccess : uint
        {
            //
            // Standart Section
            //

            AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
            MaximumAllowed = 0x2000000,     // MaximumAllowed access type

            Delete = 0x10000,
            ReadControl = 0x20000,
            WriteDAC = 0x40000,
            WriteOwner = 0x80000,
            Synchronize = 0x100000,

            StandardRightsRequired = 0xF0000,
            StandardRightsRead = ReadControl,
            StandardRightsWrite = ReadControl,
            StandardRightsExecute = ReadControl,
            StandardRightsAll = 0x1F0000,
            SpecificRightsAll = 0xFFFF,

            FILE_READ_DATA = 0x0001,        // file & pipe
            FILE_LIST_DIRECTORY = 0x0001,       // directory
            FILE_WRITE_DATA = 0x0002,       // file & pipe
            FILE_ADD_FILE = 0x0002,         // directory
            FILE_APPEND_DATA = 0x0004,      // file
            FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
            FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
            FILE_READ_EA = 0x0008,          // file & directory
            FILE_WRITE_EA = 0x0010,         // file & directory
            FILE_EXECUTE = 0x0020,          // file
            FILE_TRAVERSE = 0x0020,         // directory
            FILE_DELETE_CHILD = 0x0040,     // directory
            FILE_READ_ATTRIBUTES = 0x0080,      // all
            FILE_WRITE_ATTRIBUTES = 0x0100,     // all

            //
            // Generic Section
            //

            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000,

            SPECIFIC_RIGHTS_ALL = 0x00FFFF,
            FILE_ALL_ACCESS =
            StandardRightsRequired |
            Synchronize |
            0x1FF,

            FILE_GENERIC_READ =
            StandardRightsRead |
            FILE_READ_DATA |
            FILE_READ_ATTRIBUTES |
            FILE_READ_EA |
            Synchronize,

            FILE_GENERIC_WRITE =
            StandardRightsWrite |
            FILE_WRITE_DATA |
            FILE_WRITE_ATTRIBUTES |
            FILE_WRITE_EA |
            FILE_APPEND_DATA |
            Synchronize,

            FILE_GENERIC_EXECUTE =
            StandardRightsExecute |
              FILE_READ_ATTRIBUTES |
              FILE_EXECUTE |
              Synchronize
        }

        [Flags]
        public enum EFileShare : uint
        {
            /// <summary>
            ///
            /// </summary>
            None = 0x00000000,
            /// <summary>
            /// Enables subsequent open operations on an object to request read access.
            /// Otherwise, other processes cannot open the object if they request read access.
            /// If this flag is not specified, but the object has been opened for read access, the function fails.
            /// </summary>
            Read = 0x00000001,
            /// <summary>
            /// Enables subsequent open operations on an object to request write access.
            /// Otherwise, other processes cannot open the object if they request write access.
            /// If this flag is not specified, but the object has been opened for write access, the function fails.
            /// </summary>
            Write = 0x00000002,
            /// <summary>
            /// Enables subsequent open operations on an object to request delete access.
            /// Otherwise, other processes cannot open the object if they request delete access.
            /// If this flag is not specified, but the object has been opened for delete access, the function fails.
            /// </summary>
            Delete = 0x00000004
        }

        public enum ECreationDisposition : uint
        {
            /// <summary>
            /// Creates a new file. The function fails if a specified file exists.
            /// </summary>
            New = 1,
            /// <summary>
            /// Creates a new file, always.
            /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
            /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
            /// </summary>
            CreateAlways = 2,
            /// <summary>
            /// Opens a file. The function fails if the file does not exist.
            /// </summary>
            OpenExisting = 3,
            /// <summary>
            /// Opens a file, always.
            /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
            /// </summary>
            OpenAlways = 4,
            /// <summary>
            /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
            /// The calling process must open the file with the GENERIC_WRITE access right.
            /// </summary>
            TruncateExisting = 5
        }

        [Flags]
        public enum EFileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }
        #endregion


        public static void XCopy(string oldFile, string newFile)
        {
            CopyFileEx(oldFile, newFile, new CopyProgressRoutine(CopyProgressHandler), IntPtr.Zero, ref pbCancel, CopyFileFlags.COPY_FILE_RESTARTABLE);
        }

        public static CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize, long StreamByteTrans, uint dwStreamNumber, CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            return CopyProgressResult.PROGRESS_CONTINUE;
        }

        public static bool CreateFolder(string path)
        {
            var lpSecurityAttributes = new SECURITY_ATTRIBUTES();
            var security = new System.Security.AccessControl.DirectorySecurity();
            lpSecurityAttributes.nLength = Marshal.SizeOf(lpSecurityAttributes);
            byte[] src = security.GetSecurityDescriptorBinaryForm();
            IntPtr dest = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, dest, src.Length);
            lpSecurityAttributes.lpSecurityDescriptor = dest;
            return CreateDirectory(path, lpSecurityAttributes);
        }

        public static bool CreateFolder(string path, System.Security.AccessControl.DirectorySecurity security)
        {
            var lpSecurityAttributes = new SECURITY_ATTRIBUTES();
            lpSecurityAttributes.nLength = Marshal.SizeOf(lpSecurityAttributes);
            byte[] src = security.GetSecurityDescriptorBinaryForm();
            IntPtr dest = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, dest, src.Length);
            lpSecurityAttributes.lpSecurityDescriptor = dest;
            return CreateDirectory(path, lpSecurityAttributes);
        }

        public void writeToDisk(string lpFileName, byte[] dataToWrite)
        {
            if (dataToWrite == null) throw new System.ArgumentException("dataToWrite parameter cannot be null!");

            SafeFileHandle drive = CreateFile(fileName: lpFileName,
                             fileAccess: FileAccess.Write,
                             fileShare: FileShare.Write | FileShare.Read | FileShare.Delete,
                             securityAttributes: IntPtr.Zero,
                             creationDisposition: FileMode.Open,
                             flags: 4, //with this also an enum can be used. (as described above as EFileAttributes)
                             template: IntPtr.Zero);

            FileStream diskStreamToWrite = new FileStream(drive, FileAccess.Write);
            diskStreamToWrite.Write(dataToWrite, 0, dataToWrite.Length);
            try { diskStreamToWrite.Close(); } catch { }
            try { drive.Close(); } catch { }
        }

        public static byte[] ReadDrive(string FileName, int sizeToRead)
        {

            if ((sizeToRead < 1) || (sizeToRead == null)) throw new System.ArgumentException("Size parameter cannot be null or 0 or less than 0!");
            SafeFileHandle drive = CreateFile(fileName: FileName,
                         fileAccess: FileAccess.Read,
                         fileShare: FileShare.Write | FileShare.Read | FileShare.Delete,
                         securityAttributes: IntPtr.Zero,
                         creationDisposition: FileMode.Open,
                         flags: 4, //with this also an enum can be used. (as described above as EFileAttributes)
                         template: IntPtr.Zero);
            if (drive.IsInvalid)
            {
                throw new IOException("Unable to access drive. Win32 Error Code " + Marshal.GetLastWin32Error());
                //if get windows error code 5 this means access denied. You must try to run the program as admin privileges.
            }

            FileStream diskStreamToRead = new FileStream(drive, FileAccess.Read);
            byte[] buf = new byte[512];
            diskStreamToRead.Read(buf, 0, 512);
            try { diskStreamToRead.Close(); } catch { }
            try { drive.Close(); } catch { }
            return buf;
        }

        public static StringCollection GetVolumes()
        {
            const uint bufferLength = 1024;
            StringBuilder volume = new StringBuilder((int)bufferLength, (int)bufferLength);
            StringCollection ret = new StringCollection();

            using (FindVolumeSafeHandle volumeHandle = FindFirstVolume(volume, bufferLength))
            {
                if (volumeHandle.IsInvalid)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }

                do
                {
                    ret.Add(volume.ToString());
                } while (FindNextVolume(volumeHandle, volume, bufferLength));

                return ret;
            }
        }

        public class FindVolumeSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public FindVolumeSafeHandle()
            : base(true)
            {
            }

            public FindVolumeSafeHandle(IntPtr preexistingHandle, bool ownsHandle)
            : base(ownsHandle)
            {
                SetHandle(preexistingHandle);
            }

            protected override bool ReleaseHandle()
            {
                return FindVolumeClose(handle);
            }
        }

        public static void Flush()
        {
            //"CONIN$" will allow you to grab the input buffer regardless if it is being redirected.
            //0x40000000 | 0x80000000 - corresponds to GENERIC_READ | GENERIC_WRITE
            //0x00000001 - FILE_SHARE_READ - seems to be required to get the buffer correctly.
            //IntPtr.Zero - According to MSDN - "bInheritHandle member of the SECURITY_ATTRIBUTES structure must
            //be TRUE if the console is inherited"
            //if the console is not inherited, this can be null (IntPtr.Zero seen here)
            //3 - OPEN_EXISTING - recommended by MSDN to get the buffer correctly
            //The last two parameters are ignored in this case according to MSDN

            IntPtr inBuffer = GetInputBuffer("CONIN$", 0x40000000 | 0x80000000,
            0x00000001, IntPtr.Zero, 3, 0x0, IntPtr.Zero);

            //throw an error if the input buffer is not obtained
            Int32 error = Marshal.GetLastWin32Error();
            if (error != 0) throw new System.ComponentModel.Win32Exception(error);

            FlushConsoleInputBuffer(inBuffer);
        }

        public static bool GetFileAttributesEx(string lpFileName, out WIN32_FILE_ATTRIBUTE_DATA fileData)
        {
            return GetFileAttributesEx(lpFileName, GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, out fileData);
        }

        internal static Int64 GetDirectoryId(string dir)
        {
            var handle = CreateFile(dir, FileAccess.Read, FileShare.Read, IntPtr.Zero, FileMode.Open, 0x02000000 | 0x00000080, IntPtr.Zero);
            var fileStruct = new FILE_ID_BOTH_DIR_INFO();
            GetFileInformationByHandleEx((IntPtr)Convert.ToInt64(handle), FILE_INFO_BY_HANDLE_CLASS.FileIdBothDirectoryInfo, out fileStruct, (uint)Marshal.SizeOf(fileStruct));
            CloseHandle((IntPtr)Convert.ToInt64(handle));
            var win32Error = Marshal.GetLastWin32Error();
            if (win32Error != 0)
                throw new Win32Exception();

            return fileStruct.FileId.QuadPart;
        }

        public static FileVersionInfo GetFileVersionInfo(string FileName)
        {
            return System.Diagnostics.FileVersionInfo.GetVersionInfo(FileName);
        }

        //public void XCopy(string oldFile, string newFile)
        //{
        //    CopyFileEx(oldFile, newFile, new CopyProgressRoutine(this.CopyProgressHandler), IntPtr.Zero, MoveFileFlags.MOVE_FILE_REPLACE_EXISTSING | MoveFileFlags.MOVE_FILE_WRITE_THROUGH | MoveFileFlags.MOVE_FILE_COPY_ALLOWED);
        //}

        //public CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize, long StreamByteTrans, uint dwStreamNumber, CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        //{
        //    return CopyProgressResult.PROGRESS_CONTINUE;
        //}

        public static void EnumProcessModules(int procID)
        {
            var snapshot = CreateToolhelp32Snapshot(SnapshotFlags.Module | SnapshotFlags.Module32, (uint)procID);
            MODULEENTRY32 mod = new MODULEENTRY32() { dwSize = (uint)Marshal.SizeOf(typeof(MODULEENTRY32)) };
            if (!Module32First(snapshot, ref mod))
                return;

            List<string> modules = new List<string>();
            do
            {
                modules.Add(mod.szModule);
            }
            while (Module32Next(snapshot, ref mod));
            Console.WriteLine("OK");
        }

        public void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, (IntPtr)Convert.ToInt64(-1), (IntPtr)Convert.ToInt64(-1));
            }
        }



        //  StringBuilder sb = new StringBuilder(1024);
        //  VolumeFunctions.GetVolumeNameForVolumeMountPoint("C:\\", sb, sb.Capacity);
        //  richTextBox1.Text = sb.ToString() + "Test";
        //  VolumeFunctions.SetVolumeMountPoint(@"W:\", @"\C:\Test");
        //  richTextBox1.AppendText(Win32Exception(Marshal.GetLastWin32Error()).ToString());

    }

    public class CreateToolhelp32SnapshotFlags
    {
        public const uint TH32CS_SNAPHEAPLIST = 0x00000001;
        public const uint TH32CS_SNAPPROCESS = 0x00000002;
        public const uint TH32CS_SNAPTHREAD = 0x00000004;
        public const uint TH32CS_SNAPMODULE = 0x00000008;
        public const uint TH32CS_SNAPMODULE32 = 0x00000010;
        public const uint TH32CS_SNAPALL = (TH32CS_SNAPHEAPLIST | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD | TH32CS_SNAPMODULE);
        public const uint TH32CS_INHERIT = 0x80000000;
    }

    class Shell32
    {
        [DllImport("shell32.dll")]
        public static extern IntPtr ExtractAssociatedIcon(IntPtr hInst, StringBuilder lpIconPath, out ushort lpiIcon);
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        [DllImport("shell32.dll")]
        public static extern IntPtr FindExecutable(string lpFile, string lpDirectory, [Out] StringBuilder lpResult);
        [DllImport("shell32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsUserAnAdmin();
        [DllImport("shell32.dll", EntryPoint = "PathIsExeW", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PathIsExe([MarshalAs(UnmanagedType.LPWStr)] string szfile);
        [DllImport("shell32.dll", EntryPoint = "PathMakeUniqueName", CharSet = CharSet.Unicode)]
        public static extern bool PathMakeUniqueName(
            StringBuilder pszUniqueName,
            int cchMax,
            string pszTemplate,
            string pszLongPlate,
            string pszDir);
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(HChangeNotifyEventID wEventId,
            HChangeNotifyFlags uFlags,
            IntPtr dwItem1,
            IntPtr dwItem2);
        [DllImport("shell32.dll")]
        public static extern int SHCreateDirectoryEx(IntPtr hwnd, string pszPath, IntPtr psa);
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);
        [DllImport("shell32.dll")]
        public static extern int SHEmptyRecycleBin(IntPtr hWnd, string pszRootPath, uint dwFlags);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);
        [DllImport("Shell32.dll", SetLastError = false)]
        public static extern Int32 SHGetStockIconInfo(SHSTOCKICONID siid, SHGSI uFlags, ref SHSTOCKICONINFO psii);
        [DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "#61", SetLastError = true)]
        public static extern bool SHRunFileDialog(IntPtr hwndOwner, IntPtr hIcon, string lpszPath, string lpszDialogTitle, string lpszDialogTextBody, RunFileDialogFlags uflags);
        [DllImport("shell32.dll")]
        public static extern uint SHFormatDrive(IntPtr hwnd, uint drive, uint fmtID, uint options);

        #region Const
        const int MAX_PATH = 260;
        const int SHERB_NOCONFIRMATION = 0x00000001;
        const int SHERB_NOPROGRESSUI = 0x00000002;
        const int SHERB_NOSOUND = 0x00000004;
        public const uint SHFMT_ERROR = 0xFFFFFFFF;
        public const uint SHFMT_CANCEL = 0xFFFFFFFE;
        public const uint SHFMT_NOFORMAT = 0xFFFFFFD;
        #endregion

        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public ShellExecuteMaskFlags fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public ShowCommands nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHSTOCKICONINFO
        {
            public UInt32 cbSize;
            public IntPtr hIcon;
            public Int32 iSysIconIndex;
            public Int32 iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szPath;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public FileFuncFlags wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            public FILEOP_FLAGS fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszProgressTitle;
        }
        #endregion

        #region Enum
        [Flags]
        public enum SHFormatOptions : uint
        {
            SHFMT_OPT_FULL = 0x1,
            SHFMT_OPT_SYSONLY = 0x2,
        }
        public enum SHFormatFlags : uint
        {
            SHFMT_ID_DEFAULT = 0xFFFF,
            SHFMT_OPT_FULL = 0x1,
            SHFMT_OPT_SYSONLY = 0x2,
            SHFMT_ERROR = 0xFFFFFFFF,
            SHFMT_CANCEL = 0xFFFFFFFE,
            SHFMT_NOFORMAT = 0xFFFFFFD,
        }
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [Flags]
        public enum ShellExecuteMaskFlags : uint
        {
            SEE_MASK_DEFAULT = 0x00000000,
            SEE_MASK_CLASSNAME = 0x00000001,
            SEE_MASK_CLASSKEY = 0x00000003,
            SEE_MASK_IDLIST = 0x00000004,
            SEE_MASK_INVOKEIDLIST = 0x0000000c,   // Note SEE_MASK_INVOKEIDLIST(0xC) implies SEE_MASK_IDLIST(0x04)
            SEE_MASK_HOTKEY = 0x00000020,
            SEE_MASK_NOCLOSEPROCESS = 0x00000040,
            SEE_MASK_CONNECTNETDRV = 0x00000080,
            SEE_MASK_NOASYNC = 0x00000100,
            SEE_MASK_FLAG_DDEWAIT = SEE_MASK_NOASYNC,
            SEE_MASK_DOENVSUBST = 0x00000200,
            SEE_MASK_FLAG_NO_UI = 0x00000400,
            SEE_MASK_UNICODE = 0x00004000,
            SEE_MASK_NO_CONSOLE = 0x00008000,
            SEE_MASK_ASYNCOK = 0x00100000,
            SEE_MASK_HMONITOR = 0x00200000,
            SEE_MASK_NOZONECHECKS = 0x00800000,
            SEE_MASK_NOQUERYCLASSSTORE = 0x01000000,
            SEE_MASK_WAITFORINPUTIDLE = 0x02000000,
            SEE_MASK_FLAG_LOG_USAGE = 0x04000000,
        }
        [Flags()]
        public enum RunFileDialogFlags : uint
        {
            /// <summary>
            /// Don't use any of the flags (only works alone)
            /// </summary>
            None = 0x0000,
            /// <summary>
            /// Removes the browse button
            /// </summary>
            NoBrowse = 0x0001,
            /// <summary>
            /// No default item selected
            /// </summary>
            NoDefault = 0x0002,
            /// <summary>
            /// Calculates the working directory from the file name
            /// </summary>
            CalcDirectory = 0x0004,
            /// <summary>
            /// Removes the edit box label
            /// </summary>
            NoLabel = 0x0008,
            /// <summary>
            /// Removes the seperate memory space checkbox (Windows NT only)
            /// </summary>
            NoSeperateMemory = 0x0020
        }
        [Flags]
        public enum SHGSI : uint
        {
            SHGSI_ICONLOCATION = 0,
            SHGSI_ICON = 0x000000100,
            SHGSI_SYSICONINDEX = 0x000004000,
            SHGSI_LINKOVERLAY = 0x000008000,
            SHGSI_SELECTED = 0x000010000,
            SHGSI_LARGEICON = 0x000000000,
            SHGSI_SMALLICON = 0x000000001,
            SHGSI_SHELLICONSIZE = 0x000000004
        }
        public enum SHSTOCKICONID : uint
        {
            SIID_DOCNOASSOC = 0,
            SIID_DOCASSOC = 1,
            SIID_APPLICATION = 2,
            SIID_FOLDER = 3,
            SIID_FOLDEROPEN = 4,
            SIID_DRIVE525 = 5,
            SIID_DRIVE35 = 6,
            SIID_DRIVEREMOVE = 7,
            SIID_DRIVEFIXED = 8,
            SIID_DRIVENET = 9,
            SIID_DRIVENETDISABLED = 10,
            SIID_DRIVECD = 11,
            SIID_DRIVERAM = 12,
            SIID_WORLD = 13,
            SIID_SERVER = 15,
            SIID_PRINTER = 16,
            SIID_MYNETWORK = 17,
            SIID_FIND = 22,
            SIID_HELP = 23,
            SIID_SHARE = 28,
            SIID_LINK = 29,
            SIID_SLOWFILE = 30,
            SIID_RECYCLER = 31,
            SIID_RECYCLERFULL = 32,
            SIID_MEDIACDAUDIO = 40,
            SIID_LOCK = 47,
            SIID_AUTOLIST = 49,
            SIID_PRINTERNET = 50,
            SIID_SERVERSHARE = 51,
            SIID_PRINTERFAX = 52,
            SIID_PRINTERFAXNET = 53,
            SIID_PRINTERFILE = 54,
            SIID_STACK = 55,
            SIID_MEDIASVCD = 56,
            SIID_STUFFEDFOLDER = 57,
            SIID_DRIVEUNKNOWN = 58,
            SIID_DRIVEDVD = 59,
            SIID_MEDIADVD = 60,
            SIID_MEDIADVDRAM = 61,
            SIID_MEDIADVDRW = 62,
            SIID_MEDIADVDR = 63,
            SIID_MEDIADVDROM = 64,
            SIID_MEDIACDAUDIOPLUS = 65,
            SIID_MEDIACDRW = 66,
            SIID_MEDIACDR = 67,
            SIID_MEDIACDBURN = 68,
            SIID_MEDIABLANKCD = 69,
            SIID_MEDIACDROM = 70,
            SIID_AUDIOFILES = 71,
            SIID_IMAGEFILES = 72,
            SIID_VIDEOFILES = 73,
            SIID_MIXEDFILES = 74,
            SIID_FOLDERBACK = 75,
            SIID_FOLDERFRONT = 76,
            SIID_SHIELD = 77,
            SIID_WARNING = 78,
            SIID_INFO = 79,
            SIID_ERROR = 80,
            SIID_KEY = 81,
            SIID_SOFTWARE = 82,
            SIID_RENAME = 83,
            SIID_DELETE = 84,
            SIID_MEDIAAUDIODVD = 85,
            SIID_MEDIAMOVIEDVD = 86,
            SIID_MEDIAENHANCEDCD = 87,
            SIID_MEDIAENHANCEDDVD = 88,
            SIID_MEDIAHDDVD = 89,
            SIID_MEDIABLURAY = 90,
            SIID_MEDIAVCD = 91,
            SIID_MEDIADVDPLUSR = 92,
            SIID_MEDIADVDPLUSRW = 93,
            SIID_DESKTOPPC = 94,
            SIID_MOBILEPC = 95,
            SIID_USERS = 96,
            SIID_MEDIASMARTMEDIA = 97,
            SIID_MEDIACOMPACTFLASH = 98,
            SIID_DEVICECELLPHONE = 99,
            SIID_DEVICECAMERA = 100,
            SIID_DEVICEVIDEOCAMERA = 101,
            SIID_DEVICEAUDIOPLAYER = 102,
            SIID_NETWORKCONNECT = 103,
            SIID_INTERNET = 104,
            SIID_ZIPFILE = 105,
            SIID_SETTINGS = 106,
            SIID_DRIVEHDDVD = 132,
            SIID_DRIVEBD = 133,
            SIID_MEDIAHDDVDROM = 134,
            SIID_MEDIAHDDVDR = 135,
            SIID_MEDIAHDDVDRAM = 136,
            SIID_MEDIABDROM = 137,
            SIID_MEDIABDR = 138,
            SIID_MEDIABDRE = 139,
            SIID_CLUSTEREDDRIVE = 140,
            SIID_MAX_ICONS = 175
        }
        public enum FileFuncFlags : uint
        {
            FO_MOVE = 0x1,
            FO_COPY = 0x2,
            FO_DELETE = 0x3,
            FO_RENAME = 0x4
        }
        [Flags]
        public enum FILEOP_FLAGS : ushort
        {
            FOF_MULTIDESTFILES = 0x1,
            FOF_CONFIRMMOUSE = 0x2,
            /// <summary>
            /// Don't create progress/report
            /// </summary>
            FOF_SILENT = 0x4,
            FOF_RENAMEONCOLLISION = 0x8,
            /// <summary>
            /// Don't prompt the user.
            /// </summary>
            FOF_NOCONFIRMATION = 0x10,
            /// <summary>
            /// Fill in SHFILEOPSTRUCT.hNameMappings.
            /// Must be freed using SHFreeNameMappings
            /// </summary>
            FOF_WANTMAPPINGHANDLE = 0x20,
            FOF_ALLOWUNDO = 0x40,
            /// <summary>
            /// On *.*, do only files
            /// </summary>
            FOF_FILESONLY = 0x80,
            /// <summary>
            /// Don't show names of files
            /// </summary>
            FOF_SIMPLEPROGRESS = 0x100,
            /// <summary>
            /// Don't confirm making any needed dirs
            /// </summary>
            FOF_NOCONFIRMMKDIR = 0x200,
            /// <summary>
            /// Don't put up error UI
            /// </summary>
            FOF_NOERRORUI = 0x400,
            /// <summary>
            /// Dont copy NT file Security Attributes
            /// </summary>
            FOF_NOCOPYSECURITYATTRIBS = 0x800,
            /// <summary>
            /// Don't recurse into directories.
            /// </summary>
            FOF_NORECURSION = 0x1000,
            /// <summary>
            /// Don't operate on connected elements.
            /// </summary>
            FOF_NO_CONNECTED_ELEMENTS = 0x2000,
            /// <summary>
            /// During delete operation,
            /// warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
            /// </summary>
            FOF_WANTNUKEWARNING = 0x4000,
            /// <summary>
            /// Treat reparse points as objects, not containers
            /// </summary>
            FOF_NORECURSEREPARSE = 0x8000
        }
        /// <summary>
        /// Flags that indicate the meaning of the <i>dwItem1</i> and <i>dwItem2</i> parameters.
        /// The uFlags parameter must be one of the following values.
        /// </summary>
        [Flags]
        public enum HChangeNotifyFlags
        {
            /// <summary>
            /// The <i>dwItem1</i> and <i>dwItem2</i> parameters are DWORD values.
            /// </summary>
            SHCNF_DWORD = 0x0003,
            /// <summary>
            /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of ITEMIDLIST structures that
            /// represent the item(s) affected by the change.
            /// Each ITEMIDLIST must be relative to the desktop folder.
            /// </summary>
            SHCNF_IDLIST = 0x0000,
            /// <summary>
            /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of
            /// maximum length MAX_PATH that contain the full path names
            /// of the items affected by the change.
            /// </summary>
            SHCNF_PATHA = 0x0001,
            /// <summary>
            /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of
            /// maximum length MAX_PATH that contain the full path names
            /// of the items affected by the change.
            /// </summary>
            SHCNF_PATHW = 0x0005,
            /// <summary>
            /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that
            /// represent the friendly names of the printer(s) affected by the change.
            /// </summary>
            SHCNF_PRINTERA = 0x0002,
            /// <summary>
            /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that
            /// represent the friendly names of the printer(s) affected by the change.
            /// </summary>
            SHCNF_PRINTERW = 0x0006,
            /// <summary>
            /// The function should not return until the notification
            /// has been delivered to all affected components.
            /// As this flag modifies other data-type flags, it cannot by used by itself.
            /// </summary>
            SHCNF_FLUSH = 0x1000,
            /// <summary>
            /// The function should begin delivering notifications to all affected components
            /// but should return as soon as the notification process has begun.
            /// As this flag modifies other data-type flags, it cannot by used by itself.
            /// </summary>
            SHCNF_FLUSHNOWAIT = 0x2000
        }
        public enum FindExecutableErrorCodes : int
        {
            SE_ERR_FNF = 2,
            SE_ERR_NOASSOC = 31,
            SE_ERR_OOM = 8
        }
        [Flags]
        public enum HChangeNotifyEventID
        {
            /// <summary>
            /// All events have occurred.
            /// </summary>
            SHCNE_ALLEVENTS = 0x7FFFFFFF,

            /// <summary>
            /// A file type association has changed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/>
            /// must be specified in the <i>uFlags</i> parameter.
            /// <i>dwItem1</i> and <i>dwItem2</i> are not used and must be <see langword="null"/>.
            /// </summary>
            SHCNE_ASSOCCHANGED = 0x08000000,

            /// <summary>
            /// The attributes of an item or folder have changed.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the item or folder that has changed.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_ATTRIBUTES = 0x00000800,

            /// <summary>
            /// A nonfolder item has been created.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the item that was created.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_CREATE = 0x00000002,

            /// <summary>
            /// A nonfolder item has been deleted.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the item that was deleted.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_DELETE = 0x00000004,

            /// <summary>
            /// A drive has been added.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the root of the drive that was added.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_DRIVEADD = 0x00000100,

            /// <summary>
            /// A drive has been added and the Shell should create a new window for the drive.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the root of the drive that was added.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_DRIVEADDGUI = 0x00010000,

            /// <summary>
            /// A drive has been removed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the root of the drive that was removed.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_DRIVEREMOVED = 0x00000080,

            /// <summary>
            /// Not currently used.
            /// </summary>
            SHCNE_EXTENDED_EVENT = 0x04000000,

            /// <summary>
            /// The amount of free space on a drive has changed.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the root of the drive on which the free space changed.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_FREESPACE = 0x00040000,

            /// <summary>
            /// Storage media has been inserted into a drive.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the root of the drive that contains the new media.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_MEDIAINSERTED = 0x00000020,

            /// <summary>
            /// Storage media has been removed from a drive.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the root of the drive from which the media was removed.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_MEDIAREMOVED = 0x00000040,

            /// <summary>
            /// A folder has been created. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/>
            /// or <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the folder that was created.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_MKDIR = 0x00000008,

            /// <summary>
            /// A folder on the local computer is being shared via the network.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the folder that is being shared.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_NETSHARE = 0x00000200,

            /// <summary>
            /// A folder on the local computer is no longer being shared via the network.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the folder that is no longer being shared.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_NETUNSHARE = 0x00000400,

            /// <summary>
            /// The name of a folder has changed.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the previous pointer to an item identifier list (PIDL) or name of the folder.
            /// <i>dwItem2</i> contains the new PIDL or name of the folder.
            /// </summary>
            SHCNE_RENAMEFOLDER = 0x00020000,

            /// <summary>
            /// The name of a nonfolder item has changed.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the previous PIDL or name of the item.
            /// <i>dwItem2</i> contains the new PIDL or name of the item.
            /// </summary>
            SHCNE_RENAMEITEM = 0x00000001,

            /// <summary>
            /// A folder has been removed.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the folder that was removed.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_RMDIR = 0x00000010,

            /// <summary>
            /// The computer has disconnected from a server.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the server from which the computer was disconnected.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// </summary>
            SHCNE_SERVERDISCONNECT = 0x00004000,

            /// <summary>
            /// The contents of an existing folder have changed,
            /// but the folder still exists and has not been renamed.
            /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or
            /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>.
            /// <i>dwItem1</i> contains the folder that has changed.
            /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
            /// If a folder has been created, deleted, or renamed, use SHCNE_MKDIR, SHCNE_RMDIR, or
            /// SHCNE_RENAMEFOLDER, respectively, instead.
            /// </summary>
            SHCNE_UPDATEDIR = 0x00001000,

            /// <summary>
            /// An image in the system image list has changed.
            /// <see cref="HChangeNotifyFlags.SHCNF_DWORD"/> must be specified in <i>uFlags</i>.
            /// </summary>
            SHCNE_UPDATEIMAGE = 0x00008000,

        }
        #endregion

        public static void EmptyRecycleBin(string rootPath)
        {
            int hresult = SHEmptyRecycleBin(IntPtr.Zero, rootPath,
                    SHERB_NOCONFIRMATION | SHERB_NOPROGRESSUI | SHERB_NOSOUND);
            System.Diagnostics.Debug.Write(hresult);
        }
    }

    class Advapi32
    {
        [DllImport("advapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern int GetNamedSecurityInfo([In] string pObjectName, [In] SE_OBJECT_TYPE ObjectType, [In] SECURITY_INFORMATION SecurityInfo, [Out] out IntPtr ppsidOwner, [Out] out IntPtr ppsidGroup, [Out] out IntPtr ppDacl, [Out] out IntPtr ppSacl, [Out] out IntPtr ppSecurityDescriptor);
        [DllImport("advapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern int SetEntriesInAcl([In] uint cCountOfExplicitEntries, [In] IntPtr pListOfExplicitEntries, [In] IntPtr OldAcl, [Out] out IntPtr NewAcl);
        [DllImport("advapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern int SetNamedSecurityInfo([In] string pObjectName, [In] SE_OBJECT_TYPE ObjectType, [In] SECURITY_INFORMATION SecurityInfo, [In] IntPtr psidOwner, [In] IntPtr psidGroup, [In] IntPtr pDacl, [In] IntPtr pSacl);
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        public static extern uint SetNamedSecurityInfoW(String pObjectName, SE_OBJECT_TYPE ObjectType, SECURITY_INFORMATION SecurityInfo, IntPtr psidOwner, IntPtr psidGroup, IntPtr pDacl, IntPtr pSacl);
        [DllImport("Advapi32.dll", EntryPoint = "GetUserName", ExactSpelling = false, SetLastError = true)]
        public static extern bool GetUserName([MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, [MarshalAs(UnmanagedType.LPArray)] Int32[] nSize);
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int LookupAccountName([In, MarshalAs(UnmanagedType.LPTStr)] string systemName, [In, MarshalAs(UnmanagedType.LPTStr)] string accountName, IntPtr Sid, ref int cbSid,
            StringBuilder domainName,
            ref int cbDomainName,
            ref int use);
        [DllImportAttribute("advapi32.dll", EntryPoint = "AdjustTokenPrivileges")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(
            [InAttribute()] IntPtr TokenHandle,
            [MarshalAsAttribute(UnmanagedType.Bool)] bool DisableAllPrivileges,
            [InAttribute()] ref TOKEN_PRIVILEGES NewState,
            uint BufferLength,
            IntPtr PreviousState,
            IntPtr ReturnLength);
        [DllImportAttribute("advapi32.dll", EntryPoint = "AllocateAndInitializeSid")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool AllocateAndInitializeSid(
            [InAttribute] ref SID_IDENTIFIER_AUTHORITY pIdentifierAuthority,
            byte nSubAuthorityCount,
            uint nSubAuthority0,
            uint nSubAuthority1,
            uint nSubAuthority2,
            uint nSubAuthority3,
            uint nSubAuthority4,
            uint nSubAuthority5,
            uint nSubAuthority6,
            uint nSubAuthority7,
            ref IntPtr pSid);
        [DllImport("advapi32.dll")]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);
        [DllImportAttribute("advapi32.dll", EntryPoint = "OpenProcessToken")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(
            [InAttribute] IntPtr ProcessHandle,
            uint DesiredAccess,
            out IntPtr TokenHandle);
        [DllImport("advapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern void BuildExplicitAccessWithName([In] IntPtr pExplicitAccess, [In] string pTrusteeName, [In] ACCESS_MASK AccessPermissions, [In] ACCESS_MODE AccessMode, [In] Inheritance Inheritance);
        [DllImport("advapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        public static extern bool DeleteAce(IntPtr pAcl, int dwAceIndex);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern void BuildTrusteeWithSid(ref TRUSTEE pTrustee, IntPtr sid);
        [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ConvertSidToStringSid([MarshalAs(UnmanagedType.LPArray)] byte[] pSID, out IntPtr ptrSid);
        [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ConvertSidToStringSid(IntPtr pSid, out string strSid);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(
            string StringSecurityDescriptor,
            uint StringSDRevision,
            out IntPtr SecurityDescriptor,
            out UIntPtr SecurityDescriptorSize);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool ConvertStringSidToSid(
            string StringSid,
            out IntPtr ptrSid);
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);
        [DllImport("advapi32.dll")]
        public static extern IntPtr FreeSid(IntPtr pSid);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int GetAce(IntPtr aclPtr, int aceIndex, out IntPtr acePtr);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int GetExplicitEntriesFromAcl(IntPtr pacl, ref int pcCountOfExplicitEntries, ref IntPtr pListOfExplicitEntries);
        [DllImport("advapi32.dll")]
        public static extern uint GetLengthSid(IntPtr pSid);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool IsValidSid([MarshalAs(UnmanagedType.LPArray)] byte[] pSid);
        [DllImport("advapi32.dll", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LogonUser(
            [MarshalAs(UnmanagedType.LPStr)] string pszUserName,
            [MarshalAs(UnmanagedType.LPStr)] string pszDomain,
            [MarshalAs(UnmanagedType.LPStr)] string pszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(
            IntPtr TokenHandle,
            TOKEN_INFORMATION_CLASS TokenInformationClass,
            IntPtr TokenInformation,
            uint TokenInformationLength,
            out uint ReturnLength);
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CreateProcessWithLogonW(
             String userName,
             String domain,
             String password,
             LogonFlags logonFlags,
             String applicationName,
             String commandLine,
             CreationFlags creationFlags,
             UInt32 environment,
             String currentDirectory,
             ref STARTUPINFO startupInfo,
             out PROCESS_INFORMATION processInformation);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool SetSecurityDescriptorDacl(ref SECURITY_DESCRIPTOR sd, bool daclPresent, IntPtr dacl, bool daclDefaulted);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern Boolean SetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass,
        ref UInt32 TokenInformation, UInt32 TokenInformationLength);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool AddAce(IntPtr pAcl, uint dwAceRevision, uint dwStartingAceIndex, IntPtr pAceList, uint nAceListLength);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool ConvertSecurityDescriptorToStringSecurityDescriptor(IntPtr SD, int SDRevision, SECURITY_INFORMATION SecurityInfo, ref IntPtr StringSD, IntPtr StringSDLength);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetAclInformation(IntPtr pAcl, IntPtr pAclInformation, uint nAclInformationLength, ACL_INFORMATION_CLASS dwAclInformationClass);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetAclInformation(IntPtr pAcl, ref ACL_REVISION_INFORMATION pAclInformation, uint nAclInformationLength, ACL_INFORMATION_CLASS dwAclInformationClass);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetAclInformation(IntPtr pAcl, ref ACL_SIZE_INFORMATION pAclInformation, uint nAclInformationLength, ACL_INFORMATION_CLASS dwAclInformationClass);

        #region Structure
        [StructLayout(LayoutKind.Sequential)]
        public struct ACL_SIZE_INFORMATION
        {
            public uint AceCount;
            public uint AclBytesInUse;
            public uint AclBytesFree;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ACL_REVISION_INFORMATION
        {
            public uint AclRevision;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        public struct STARTUPINFOEX
        {
            public STARTUPINFO StartupInfo;
            public IntPtr lpAttributeList;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SID_IDENTIFIER_AUTHORITY
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
            public byte[] Value;
        }
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
            public LUID_AND_ATTRIBUTES[] Privileges;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public uint Attributes;
        }
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SECURITY_DESCRIPTOR
        {
            public byte revision;
            public byte size;
            public short control;
            public IntPtr owner;
            public IntPtr group;
            public IntPtr sacl;
            public IntPtr dacl;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public struct EXPLICIT_ACCESS
        {
            public int grfAccessPermissions;
            public int grfAccessMode;
            public int grfInheritance;
            public TRUSTEE Trustee;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 0)]
        public struct TRUSTEE : IDisposable
        {
            public IntPtr pMultipleTrustee;
            public MULTIPLE_TRUSTEE_OPERATION MultipleTrusteeOperation;
            public TRUSTEE_FORM TrusteeForm;
            public TRUSTEE_TYPE TrusteeType;
            public IntPtr ptstrName;

            void IDisposable.Dispose()
            {
                if (ptstrName != IntPtr.Zero) Marshal.Release(ptstrName);
            }

            public string Name { get { return Marshal.PtrToStringAuto(ptstrName); } }
        }
        #endregion

        #region Enum
        public enum ACL_INFORMATION_CLASS
        {
            AclRevisionInformation = 1,
            AclSizeInformation
        }
        [Flags]
        public enum CreationFlags
        {
            CREATE_SUSPENDED = 0x00000004,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            CREATE_SEPARATE_WOW_VDM = 0x00000800,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
        }

        [Flags]
        public enum LogonFlags
        {
            LOGON_WITH_PROFILE = 0x00000001,
            LOGON_NETCREDENTIALS_ONLY = 0x00000002
        }
        public enum TOKEN_INFORMATION_CLASS
        {
            /// <summary>
            /// The buffer receives a TOKEN_USER structure that contains the user account of the token.
            /// </summary>
            TokenUser = 1,

            /// <summary>
            /// The buffer receives a TOKEN_GROUPS structure that contains the group accounts associated with the token.
            /// </summary>
            TokenGroups,

            /// <summary>
            /// The buffer receives a TOKEN_PRIVILEGES structure that contains the privileges of the token.
            /// </summary>
            TokenPrivileges,

            /// <summary>
            /// The buffer receives a TOKEN_OWNER structure that contains the default owner security identifier (SID) for newly created objects.
            /// </summary>
            TokenOwner,

            /// <summary>
            /// The buffer receives a TOKEN_PRIMARY_GROUP structure that contains the default primary group SID for newly created objects.
            /// </summary>
            TokenPrimaryGroup,

            /// <summary>
            /// The buffer receives a TOKEN_DEFAULT_DACL structure that contains the default DACL for newly created objects.
            /// </summary>
            TokenDefaultDacl,

            /// <summary>
            /// The buffer receives a TOKEN_SOURCE structure that contains the source of the token. TOKEN_QUERY_SOURCE access is needed to retrieve this information.
            /// </summary>
            TokenSource,

            /// <summary>
            /// The buffer receives a TOKEN_TYPE value that indicates whether the token is a primary or impersonation token.
            /// </summary>
            TokenType,

            /// <summary>
            /// The buffer receives a SECURITY_IMPERSONATION_LEVEL value that indicates the impersonation level of the token. If the access token is not an impersonation token, the function fails.
            /// </summary>
            TokenImpersonationLevel,

            /// <summary>
            /// The buffer receives a TOKEN_STATISTICS structure that contains various token statistics.
            /// </summary>
            TokenStatistics,

            /// <summary>
            /// The buffer receives a TOKEN_GROUPS structure that contains the list of restricting SIDs in a restricted token.
            /// </summary>
            TokenRestrictedSids,

            /// <summary>
            /// The buffer receives a DWORD value that indicates the Terminal Services session identifier that is associated with the token.
            /// </summary>
            TokenSessionId,

            /// <summary>
            /// The buffer receives a TOKEN_GROUPS_AND_PRIVILEGES structure that contains the user SID, the group accounts, the restricted SIDs, and the authentication ID associated with the token.
            /// </summary>
            TokenGroupsAndPrivileges,

            /// <summary>
            /// Reserved.
            /// </summary>
            TokenSessionReference,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if the token includes the SANDBOX_INERT flag.
            /// </summary>
            TokenSandBoxInert,

            /// <summary>
            /// Reserved.
            /// </summary>
            TokenAuditPolicy,

            /// <summary>
            /// The buffer receives a TOKEN_ORIGIN value.
            /// </summary>
            TokenOrigin,

            /// <summary>
            /// The buffer receives a TOKEN_ELEVATION_TYPE value that specifies the elevation level of the token.
            /// </summary>
            TokenElevationType,

            /// <summary>
            /// The buffer receives a TOKEN_LINKED_TOKEN structure that contains a handle to another token that is linked to this token.
            /// </summary>
            TokenLinkedToken,

            /// <summary>
            /// The buffer receives a TOKEN_ELEVATION structure that specifies whether the token is elevated.
            /// </summary>
            TokenElevation,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if the token has ever been filtered.
            /// </summary>
            TokenHasRestrictions,

            /// <summary>
            /// The buffer receives a TOKEN_ACCESS_INFORMATION structure that specifies security information contained in the token.
            /// </summary>
            TokenAccessInformation,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if virtualization is allowed for the token.
            /// </summary>
            TokenVirtualizationAllowed,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if virtualization is enabled for the token.
            /// </summary>
            TokenVirtualizationEnabled,

            /// <summary>
            /// The buffer receives a TOKEN_MANDATORY_LABEL structure that specifies the token's integrity level.
            /// </summary>
            TokenIntegrityLevel,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if the token has the UIAccess flag set.
            /// </summary>
            TokenUIAccess,

            /// <summary>
            /// The buffer receives a TOKEN_MANDATORY_POLICY structure that specifies the token's mandatory integrity policy.
            /// </summary>
            TokenMandatoryPolicy,

            /// <summary>
            /// The buffer receives the token's logon security identifier (SID).
            /// </summary>
            TokenLogonSid,

            /// <summary>
            /// The maximum value for this enumeration
            /// </summary>
            MaxTokenInfoClass
        }
        public enum LogonType
        {
            /// <summary>
            /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
            /// by a terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon information for disconnected operations;
            /// therefore, it is inappropriate for some client/server applications,
            /// such as a mail server.
            /// </summary>
            LOGON32_LOGON_INTERACTIVE = 2,

            /// <summary>
            /// This logon type is intended for high performance servers to authenticate plaintext passwords.

            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_NETWORK = 3,

            /// <summary>
            /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without
            /// their direct intervention. This type is also for higher performance servers that process many plaintext
            /// authentication attempts at a time, such as mail or Web servers.
            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_BATCH = 4,

            /// <summary>
            /// Indicates a service-type logon. The account provided must have the service privilege enabled.
            /// </summary>
            LOGON32_LOGON_SERVICE = 5,

            /// <summary>
            /// This logon type is for GINA DLLs that log on users who will be interactively using the computer.
            /// This logon type can generate a unique audit record that shows when the workstation was unlocked.
            /// </summary>
            LOGON32_LOGON_UNLOCK = 7,

            /// <summary>
            /// This logon type preserves the name and password in the authentication package, which allows the server to make
            /// connections to other network servers while impersonating the client. A server can accept plaintext credentials
            /// from a client, call LogonUser, verify that the user can access the system across the network, and still
            /// communicate with other servers.
            /// NOTE: Windows NT:  This value is not supported.
            /// </summary>
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

            /// <summary>
            /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
            /// The new logon session has the same local identifier but uses different credentials for other network connections.
            /// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
            /// NOTE: Windows NT:  This value is not supported.
            /// </summary>
            LOGON32_LOGON_NEW_CREDENTIALS = 9,
        }
        public enum LogonProvider
        {
            /// <summary>
            /// Use the standard logon provider for the system.
            /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name
            /// is not in UPN format. In this case, the default provider is NTLM.
            /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
            /// </summary>
            LOGON32_PROVIDER_DEFAULT = 0,
            LOGON32_PROVIDER_WINNT35 = 1,
            LOGON32_PROVIDER_WINNT40 = 2,
            LOGON32_PROVIDER_WINNT50 = 3
        }
        [Flags]
        public enum CreateProcessFlags
        {
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_NO_WINDOW = 0x08000000,
            CREATE_PROTECTED_PROCESS = 0x00040000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_SEPARATE_WOW_VDM = 0x00000800,
            CREATE_SHARED_WOW_VDM = 0x00001000,
            CREATE_SUSPENDED = 0x00000004,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,
            DEBUG_PROCESS = 0x00000001,
            DETACHED_PROCESS = 0x00000008,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
            INHERIT_PARENT_AFFINITY = 0x00010000
        }
        public enum LOGON_PROVIDER
        {
            LOGON32_PROVIDER_DEFAULT,
            LOGON32_PROVIDER_WINNT35,
            LOGON32_PROVIDER_WINNT40,
            LOGON32_PROVIDER_WINNT50
        }
        public enum LOGON_TYPE
        {
            LOGON32_LOGON_INTERACTIVE = 2,
            LOGON32_LOGON_NETWORK = 3,
            LOGON32_LOGON_BATCH = 4,
            LOGON32_LOGON_SERVICE = 5,
            LOGON32_LOGON_UNLOCK = 7,
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
            LOGON32_LOGON_NEW_CREDENTIALS = 9
        }
        [Flags]
        public enum ACCESS_MASK : uint
        {
            DELETE = 0x00010000,
            READ_CONTROL = 0x00020000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            SYNCHRONIZE = 0x00100000,

            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            STANDARD_RIGHTS_READ = 0x00020000,
            STANDARD_RIGHTS_WRITE = 0x00020000,
            STANDARD_RIGHTS_EXECUTE = 0x00020000,
            STANDARD_RIGHTS_ALL = 0x001F0000,
            SPECIFIC_RIGHTS_ALL = 0x0000FFFF,

            ACCESS_SYSTEM_SECURITY = 0x01000000,

            MAXIMUM_ALLOWED = 0x02000000,

            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000,

            DESKTOP_READOBJECTS = 0x00000001,
            DESKTOP_CREATEWINDOW = 0x00000002,
            DESKTOP_CREATEMENU = 0x00000004,
            DESKTOP_HOOKCONTROL = 0x00000008,
            DESKTOP_JOURNALRECORD = 0x00000010,
            DESKTOP_JOURNALPLAYBACK = 0x00000020,
            DESKTOP_ENUMERATE = 0x00000040,
            DESKTOP_WRITEOBJECTS = 0x00000080,
            DESKTOP_SWITCHDESKTOP = 0x00000100,

            WINSTA_ENUMDESKTOPS = 0x00000001,
            WINSTA_READATTRIBUTES = 0x00000002,
            WINSTA_ACCESSCLIPBOARD = 0x00000004,
            WINSTA_CREATEDESKTOP = 0x00000008,
            WINSTA_WRITEATTRIBUTES = 0x00000010,
            WINSTA_ACCESSGLOBALATOMS = 0x00000020,
            WINSTA_EXITWINDOWS = 0x00000040,
            WINSTA_ENUMERATE = 0x00000100,
            WINSTA_READSCREEN = 0x00000200,
            WINSTA_ALL_ACCESS = 0x0000037F
        }
        public enum ACCESS_MODE : uint
        {
            NOT_USED_ACCESS = 0,
            GRANT_ACCESS = 1,
            SET_ACCESS,
            DENY_ACCESS,
            REVOKE_ACCESS,
            SET_AUDIT_SUCCESS,
            SET_AUDIT_FAILURE
        }
        public enum Inheritance : uint
        {
            CONTAINER_INHERIT_ACE = 0,
            INHERIT_ONLY_ACE,
            NO_PROPAGATE_INHERIT_ACE,
            OBJECT_INHERIT_ACE,
            SUB_CONTAINERS_AND_OBJECTS_INHERIT,
            SUB_CONTAINERS_ONLY_INHERIT,
            SUB_OBJECTS_ONLY_INHERIT
        }
        public enum MULTIPLE_TRUSTEE_OPERATION
        {
            NO_MULTIPLE_TRUSTEE,
            TRUSTEE_IS_IMPERSONATE
        }
        public enum TRUSTEE_FORM
        {
            TRUSTEE_IS_SID,
            TRUSTEE_IS_NAME,
            TRUSTEE_BAD_FORM,
            TRUSTEE_IS_OBJECTS_AND_SID,
            TRUSTEE_IS_OBJECTS_AND_NAME
        }
        public enum TRUSTEE_TYPE
        {
            TRUSTEE_IS_UNKNOWN,
            TRUSTEE_IS_USER,
            TRUSTEE_IS_GROUP,
            TRUSTEE_IS_DOMAIN,
            TRUSTEE_IS_ALIAS,
            TRUSTEE_IS_WELL_KNOWN_GROUP,
            TRUSTEE_IS_DELETED,
            TRUSTEE_IS_INVALID,
            TRUSTEE_IS_COMPUTER
        }
        [Flags]
        public enum SECURITY_INFORMATION : uint
        {
            OWNER_SECURITY_INFORMATION = 0x00000001,
            GROUP_SECURITY_INFORMATION = 0x00000002,
            DACL_SECURITY_INFORMATION = 0x00000004,
            SACL_SECURITY_INFORMATION = 0x00000008,
            UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000,
            UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000,
            PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000,
            PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000
        }
        public enum SE_OBJECT_TYPE
        {
            SE_UNKNOWN_OBJECT_TYPE = 0,
            SE_FILE_OBJECT,
            SE_SERVICE,
            SE_PRINTER,
            SE_REGISTRY_KEY,
            SE_LMSHARE,
            SE_KERNEL_OBJECT,
            SE_WINDOW_OBJECT,
            SE_DS_OBJECT,
            SE_DS_OBJECT_ALL,
            SE_PROVIDER_DEFINED_OBJECT,
            SE_WMIGUID_OBJECT,
            SE_REGISTRY_WOW64_32KEY
        }
        #endregion

        public const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const int TOKEN_QUERY = 8;
        public const int SE_PRIVILEGE_ENABLED = 2;
        public static bool AdjustPrivileges(string lpName)
        {
            IntPtr token = IntPtr.Zero;
            if (!OpenProcessToken(Kernel32.GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out token))
            {
                if (Kernel32.GetLastError() == 120)
                    return true;
                else
                    return false;
            }
            LUID luid = new LUID();
            if (!LookupPrivilegeValue(null, lpName, ref luid))
            {
                Kernel32.CloseHandle(token);
                return false;
            }
            TOKEN_PRIVILEGES tokenPrivileges = new TOKEN_PRIVILEGES();
            tokenPrivileges.PrivilegeCount = 1;
            tokenPrivileges.Privileges = new LUID_AND_ATTRIBUTES[1];
            tokenPrivileges.Privileges[0].Luid = luid;
            tokenPrivileges.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
            if (!AdjustTokenPrivileges(token, false, ref tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero))
            {
                Kernel32.CloseHandle(token);
                return false;
            }

            Kernel32.CloseHandle(token);
            return true;


            //IntPtr pSidOwner, pSidGroup, pDacl, pSacl, pSecurityDescriptor;
            //ACCESS_MASK mask = new ACCESS_MASK();
            //uint ret = GetNamedSecurityInfo(objName,
            //                                seObjType,
            //                                SECURITY_INFORMATION.DACL_SECURITY_INFORMATION | SECURITY_INFORMATION.OWNER_SECURITY_INFORMATION | SECURITY_INFORMATION.GROUP_SECURITY_INFORMATION,
            //                                out pSidOwner,
            //                                out pSidGroup,
            //                                out pDacl,
            //                                out pSacl,
            //                                out pSecurityDescriptor);
            //return false;
        }
        public const string SE_TAKE_OWNERSHIP_NAME = "SeTakeOwnershipPrivilege";
        public const string SE_RESTORE_PRIVILEGE = "SeRestorePrivilege";
        public static void AddDACL(string pObjName, string User, ACCESS_MASK AccessMask, ACCESS_MODE AccessMode, Inheritance inheritance)
        {
            if (AdjustPrivileges(SE_TAKE_OWNERSHIP_NAME))
            {
                byte[] str = new byte[256];
                Int32[] len = new Int32[1];
                len[0] = 256;
                GetUserName(str, len);
                string UserName = System.Text.Encoding.ASCII.GetString(str);
                UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                int err = 0, cbSid = 0, use = 0, cbDomainName = 0;
                IntPtr ea = IntPtr.Zero;
                if ((ea = Kernel32.LocalAlloc(0, 64)) == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                if ((err = LookupAccountName(null, UserName, IntPtr.Zero, ref cbSid, null, ref cbDomainName, ref use)) == 0)
                {
                    if ((err = GetNamedSecurityInfo(pObjName, SE_OBJECT_TYPE.SE_FILE_OBJECT, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION, out IntPtr sidOwn, out IntPtr sidGroup, out IntPtr oldDacl, out IntPtr psacl, out IntPtr psd)) != 0)
                        throw new Win32Exception((int)err);
                    BuildExplicitAccessWithName(ea, User, AccessMask, AccessMode, inheritance);
                    if ((err = SetEntriesInAcl(1, ea, oldDacl, out IntPtr newDacl)) == 0)
                    {
                        if ((err = SetNamedSecurityInfo(pObjName, SE_OBJECT_TYPE.SE_FILE_OBJECT, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION, IntPtr.Zero, IntPtr.Zero, newDacl, IntPtr.Zero)) != 0)
                            throw new Win32Exception((int)err);
                    }
                    else
                        throw new Win32Exception((int)err);
                }
                else throw new Win32Exception(err);
            }
        }

        public static void RemoveDACL(string FileName, Inheritance inheritance)
        {
            IntPtr sd = IntPtr.Zero;
            IntPtr daclNew = IntPtr.Zero;
            int err;

            IntPtr daclOld;
            IntPtr unused1, unused2, unused3;
            if ((err = GetNamedSecurityInfo(FileName, SE_OBJECT_TYPE.SE_FILE_OBJECT, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION, out unused1, out unused2, out daclOld, out unused3, out sd)) != 0)
                throw new Win32Exception(err);
            DeleteAce(daclOld, 0);
            if ((err = SetNamedSecurityInfo(FileName, SE_OBJECT_TYPE.SE_FILE_OBJECT, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION, IntPtr.Zero, IntPtr.Zero, daclOld, IntPtr.Zero)) != 0)
                throw new Win32Exception(err);
        }
    }

    class Ntdll
    {
        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, IntPtr SystemInformation, uint SystemInformationLength, out uint ReturnLength);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern NtStatus NtOpenProcess(ref IntPtr ProcessHandle, UInt32 AccessMask, ref OBJECT_ATTRIBUTES ObjectAttributes, ref CLIENT_ID ClientId);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, IntPtr processInformation, uint processInformationLength, IntPtr returnLength);
        [DllImport("ntdll.dll")]
        public static extern UInt32 NtSetSystemInformation(SYSTEM_INFORMATION_CLASS InfoClass, IntPtr Info, UInt32 Length);

        #region struct
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct OBJECT_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr RootDirectory;
            public IntPtr ObjectName;
            public uint Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;
        }

        public struct CLIENT_ID
        {
            IntPtr UniqueProcess;
            IntPtr UniqueThread;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_BASIC_INFORMATION
        {
            IntPtr Reserved1;
            IntPtr PebAddress;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            IntPtr Reserved2;
            IntPtr UniquePid;
            IntPtr Reserved3;
        }

        public struct PROCESS_EXTENDED_BASIC_INFORMATION
        {
            public UIntPtr Size;
            public PROCESS_BASIC_INFORMATION BasicInfo;
            public uint Flags;
        }
        #endregion

        #region enum

        public enum PROCESSINFOCLASS : int
        {
            ProcessBasicInformation = 0x00,
             ProcessDebugPort = 0x07,
             ProcessExceptionPort = 0x08,
             ProcessAccessToken = 0x09,
             ProcessWow64Information = 0x1A,
             ProcessImageFileName = 0x1B,
             ProcessDebugObjectHandle = 0x1E,
             ProcessDebugFlags = 0x1F,
             ProcessExecuteFlags = 0x22,
             ProcessInstrumentationCallback = 0x28,
             MaxProcessInfoClass = 0x64
        }

        public enum SYSTEM_INFORMATION_CLASS
        {
            SystemBasicInformation = 0x00,
            SystemProcessorInformation = 0x01,
            SystemPerformanceInformation = 0x02,
            SystemTimeOfDayInformation = 0x03,
            SystemPathInformation = 0x04,
            SystemProcessInformation = 0x05,
            SystemCallCountInformation = 0x06,
            SystemDeviceInformation = 0x07,
            SystemProcessorPerformanceInformation = 0x08,
            SystemFlagsInformation = 0x09,
            SystemCallTimeInformation = 0x0A,
            SystemModuleInformation = 0x0B,
            SystemLocksInformation = 0x0C,
            SystemStackTraceInformation = 0x0D,
            SystemPagedPoolInformation = 0x0E,
            SystemNonPagedPoolInformation = 0x0F,
            SystemHandleInformation = 0x10,
            SystemObjectInformation = 0x11,
            SystemPageFileInformation = 0x12,
            SystemVdmInstemulInformation = 0x13,
            SystemVdmBopInformation = 0x14,
            SystemFileCacheInformation = 0x15,
            SystemPoolTagInformation = 0x16,
            SystemInterruptInformation = 0x17,
            SystemDpcBehaviorInformation = 0x18,
            SystemFullMemoryInformation = 0x19,
            SystemLoadGdiDriverInformation = 0x1A,
            SystemUnloadGdiDriverInformation = 0x1B,
            SystemTimeAdjustmentInformation = 0x1C,
            SystemSummaryMemoryInformation = 0x1D,
            SystemMirrorMemoryInformation = 0x1E,
            SystemPerformanceTraceInformation = 0x1F,
            SystemObsolete0 = 0x20,
            SystemExceptionInformation = 0x21,
            SystemCrashDumpStateInformation = 0x22,
            SystemKernelDebuggerInformation = 0x23,
            SystemContextSwitchInformation = 0x24,
            SystemRegistryQuotaInformation = 0x25,
            SystemExtendServiceTableInformation = 0x26,
            SystemPrioritySeperation = 0x27,
            SystemVerifierAddDriverInformation = 0x28,
            SystemVerifierRemoveDriverInformation = 0x29,
            SystemProcessorIdleInformation = 0x2A,
            SystemLegacyDriverInformation = 0x2B,
            SystemCurrentTimeZoneInformation = 0x2C,
            SystemLookasideInformation = 0x2D,
            SystemTimeSlipNotification = 0x2E,
            SystemSessionCreate = 0x2F,
            SystemSessionDetach = 0x30,
            SystemSessionInformation = 0x31,
            SystemRangeStartInformation = 0x32,
            SystemVerifierInformation = 0x33,
            SystemVerifierThunkExtend = 0x34,
            SystemSessionProcessInformation = 0x35,
            SystemLoadGdiDriverInSystemSpace = 0x36,
            SystemNumaProcessorMap = 0x37,
            SystemPrefetcherInformation = 0x38,
            SystemExtendedProcessInformation = 0x39,
            SystemRecommendedSharedDataAlignment = 0x3A,
            SystemComPlusPackage = 0x3B,
            SystemNumaAvailableMemory = 0x3C,
            SystemProcessorPowerInformation = 0x3D,
            SystemEmulationBasicInformation = 0x3E,
            SystemEmulationProcessorInformation = 0x3F,
            SystemExtendedHandleInformation = 0x40,
            SystemLostDelayedWriteInformation = 0x41,
            SystemBigPoolInformation = 0x42,
            SystemSessionPoolTagInformation = 0x43,
            SystemSessionMappedViewInformation = 0x44,
            SystemHotpatchInformation = 0x45,
            SystemObjectSecurityMode = 0x46,
            SystemWatchdogTimerHandler = 0x47,
            SystemWatchdogTimerInformation = 0x48,
            SystemLogicalProcessorInformation = 0x49,
            SystemWow64SharedInformationObsolete = 0x4A,
            SystemRegisterFirmwareTableInformationHandler = 0x4B,
            SystemFirmwareTableInformation = 0x4C,
            SystemModuleInformationEx = 0x4D,
            SystemVerifierTriageInformation = 0x4E,
            SystemSuperfetchInformation = 0x4F,
            SystemMemoryListInformation = 0x50,
            SystemFileCacheInformationEx = 0x51,
            SystemThreadPriorityClientIdInformation = 0x52,
            SystemProcessorIdleCycleTimeInformation = 0x53,
            SystemVerifierCancellationInformation = 0x54,
            SystemProcessorPowerInformationEx = 0x55,
            SystemRefTraceInformation = 0x56,
            SystemSpecialPoolInformation = 0x57,
            SystemProcessIdInformation = 0x58,
            SystemErrorPortInformation = 0x59,
            SystemBootEnvironmentInformation = 0x5A,
            SystemHypervisorInformation = 0x5B,
            SystemVerifierInformationEx = 0x5C,
            SystemTimeZoneInformation = 0x5D,
            SystemImageFileExecutionOptionsInformation = 0x5E,
            SystemCoverageInformation = 0x5F,
            SystemPrefetchPatchInformation = 0x60,
            SystemVerifierFaultsInformation = 0x61,
            SystemSystemPartitionInformation = 0x62,
            SystemSystemDiskInformation = 0x63,
            SystemProcessorPerformanceDistribution = 0x64,
            SystemNumaProximityNodeInformation = 0x65,
            SystemDynamicTimeZoneInformation = 0x66,
            SystemCodeIntegrityInformation = 0x67,
            SystemProcessorMicrocodeUpdateInformation = 0x68,
            SystemProcessorBrandString = 0x69,
            SystemVirtualAddressInformation = 0x6A,
            SystemLogicalProcessorAndGroupInformation = 0x6B,
            SystemProcessorCycleTimeInformation = 0x6C,
            SystemStoreInformation = 0x6D,
            SystemRegistryAppendString = 0x6E,
            SystemAitSamplingValue = 0x6F,
            SystemVhdBootInformation = 0x70,
            SystemCpuQuotaInformation = 0x71,
            SystemNativeBasicInformation = 0x72,
            SystemErrorPortTimeouts = 0x73,
            SystemLowPriorityIoInformation = 0x74,
            SystemBootEntropyInformation = 0x75,
            SystemVerifierCountersInformation = 0x76,
            SystemPagedPoolInformationEx = 0x77,
            SystemSystemPtesInformationEx = 0x78,
            SystemNodeDistanceInformation = 0x79,
            SystemAcpiAuditInformation = 0x7A,
            SystemBasicPerformanceInformation = 0x7B,
            SystemQueryPerformanceCounterInformation = 0x7C,
            SystemSessionBigPoolInformation = 0x7D,
            SystemBootGraphicsInformation = 0x7E,
            SystemScrubPhysicalMemoryInformation = 0x7F,
            SystemBadPageInformation = 0x80,
            SystemProcessorProfileControlArea = 0x81,
            SystemCombinePhysicalMemoryInformation = 0x82,
            SystemEntropyInterruptTimingInformation = 0x83,
            SystemConsoleInformation = 0x84,
            SystemPlatformBinaryInformation = 0x85,
            SystemPolicyInformation = 0x86,
            SystemHypervisorProcessorCountInformation = 0x87,
            SystemDeviceDataInformation = 0x88,
            SystemDeviceDataEnumerationInformation = 0x89,
            SystemMemoryTopologyInformation = 0x8A,
            SystemMemoryChannelInformation = 0x8B,
            SystemBootLogoInformation = 0x8C,
            SystemProcessorPerformanceInformationEx = 0x8D,
            SystemCriticalProcessErrorLogInformation = 0x8E,
            SystemSecureBootPolicyInformation = 0x8F,
            SystemPageFileInformationEx = 0x90,
            SystemSecureBootInformation = 0x91,
            SystemEntropyInterruptTimingRawInformation = 0x92,
            SystemPortableWorkspaceEfiLauncherInformation = 0x93,
            SystemFullProcessInformation = 0x94,
            SystemKernelDebuggerInformationEx = 0x95,
            SystemBootMetadataInformation = 0x96,
            SystemSoftRebootInformation = 0x97,
            SystemElamCertificateInformation = 0x98,
            SystemOfflineDumpConfigInformation = 0x99,
            SystemProcessorFeaturesInformation = 0x9A,
            SystemRegistryReconciliationInformation = 0x9B,
            SystemEdidInformation = 0x9C,
            SystemManufacturingInformation = 0x9D,
            SystemEnergyEstimationConfigInformation = 0x9E,
            SystemHypervisorDetailInformation = 0x9F,
            SystemProcessorCycleStatsInformation = 0xA0,
            SystemVmGenerationCountInformation = 0xA1,
            SystemTrustedPlatformModuleInformation = 0xA2,
            SystemKernelDebuggerFlags = 0xA3,
            SystemCodeIntegrityPolicyInformation = 0xA4,
            SystemIsolatedUserModeInformation = 0xA5,
            SystemHardwareSecurityTestInterfaceResultsInformation = 0xA6,
            SystemSingleModuleInformation = 0xA7,
            SystemAllowedCpuSetsInformation = 0xA8,
            SystemDmaProtectionInformation = 0xA9,
            SystemInterruptCpuSetsInformation = 0xAA,
            SystemSecureBootPolicyFullInformation = 0xAB,
            SystemCodeIntegrityPolicyFullInformation = 0xAC,
            SystemAffinitizedInterruptProcessorInformation = 0xAD,
            SystemRootSiloInformation = 0xAE,
            SystemCpuSetInformation = 0xAF,
            SystemCpuSetTagInformation = 0xB0,
            SystemWin32WerStartCallout = 0xB1,
            SystemSecureKernelProfileInformation = 0xB2,
            SystemCodeIntegrityPlatformManifestInformation = 0xB3,
            SystemInterruptSteeringInformation = 0xB4,
            SystemSuppportedProcessorArchitectures = 0xB5,
            SystemMemoryUsageInformation = 0xB6,
            SystemCodeIntegrityCertificateInformation = 0xB7,
            SystemPhysicalMemoryInformation = 0xB8,
            SystemControlFlowTransition = 0xB9,
            SystemKernelDebuggingAllowed = 0xBA,
            SystemActivityModerationExeState = 0xBB,
            SystemActivityModerationUserSettings = 0xBC,
            SystemCodeIntegrityPoliciesFullInformation = 0xBD,
            SystemCodeIntegrityUnlockInformation = 0xBE,
            SystemIntegrityQuotaInformation = 0xBF,
            SystemFlushInformation = 0xC0,
            SystemProcessorIdleMaskInformation = 0xC1,
            SystemSecureDumpEncryptionInformation = 0xC2,
            SystemWriteConstraintInformation = 0xC3,
            SystemKernelVaShadowInformation = 0xC4,
            SystemHypervisorSharedPageInformation = 0xC5,
            SystemFirmwareBootPerformanceInformation = 0xC6,
            SystemCodeIntegrityVerificationInformation = 0xC7,
            SystemFirmwarePartitionInformation = 0xC8,
            SystemSpeculationControlInformation = 0xC9,
            SystemDmaGuardPolicyInformation = 0xCA,
            SystemEnclaveLaunchControlInformation = 0xCB,
            SystemWorkloadAllowedCpuSetsInformation = 0xCC,
            SystemCodeIntegrityUnlockModeInformation = 0xCD,
            SystemLeapSecondInformation = 0xCE,
            SystemFlags2Information = 0xCF,
            SystemSecurityModelInformation = 0xD0,
            SystemCodeIntegritySyntheticCacheInformation = 0xD1,
            MaxSystemInfoClass = 0xD2
        }
        public enum NtStatus : uint
        {
            // Success
            Success = 0x00000000,
            Wait0 = 0x00000000,
            Wait1 = 0x00000001,
            Wait2 = 0x00000002,
            Wait3 = 0x00000003,
            Wait63 = 0x0000003f,
            Abandoned = 0x00000080,
            AbandonedWait0 = 0x00000080,
            AbandonedWait1 = 0x00000081,
            AbandonedWait2 = 0x00000082,
            AbandonedWait3 = 0x00000083,
            AbandonedWait63 = 0x000000bf,
            UserApc = 0x000000c0,
            KernelApc = 0x00000100,
            Alerted = 0x00000101,
            Timeout = 0x00000102,
            Pending = 0x00000103,
            Reparse = 0x00000104,
            MoreEntries = 0x00000105,
            NotAllAssigned = 0x00000106,
            SomeNotMapped = 0x00000107,
            OpLockBreakInProgress = 0x00000108,
            VolumeMounted = 0x00000109,
            RxActCommitted = 0x0000010a,
            NotifyCleanup = 0x0000010b,
            NotifyEnumDir = 0x0000010c,
            NoQuotasForAccount = 0x0000010d,
            PrimaryTransportConnectFailed = 0x0000010e,
            PageFaultTransition = 0x00000110,
            PageFaultDemandZero = 0x00000111,
            PageFaultCopyOnWrite = 0x00000112,
            PageFaultGuardPage = 0x00000113,
            PageFaultPagingFile = 0x00000114,
            CrashDump = 0x00000116,
            ReparseObject = 0x00000118,
            NothingToTerminate = 0x00000122,
            ProcessNotInJob = 0x00000123,
            ProcessInJob = 0x00000124,
            ProcessCloned = 0x00000129,
            FileLockedWithOnlyReaders = 0x0000012a,
            FileLockedWithWriters = 0x0000012b,

            // Informational
            Informational = 0x40000000,
            ObjectNameExists = 0x40000000,
            ThreadWasSuspended = 0x40000001,
            WorkingSetLimitRange = 0x40000002,
            ImageNotAtBase = 0x40000003,
            RegistryRecovered = 0x40000009,

            // Warning
            Warning = 0x80000000,
            GuardPageViolation = 0x80000001,
            DatatypeMisalignment = 0x80000002,
            Breakpoint = 0x80000003,
            SingleStep = 0x80000004,
            BufferOverflow = 0x80000005,
            NoMoreFiles = 0x80000006,
            HandlesClosed = 0x8000000a,
            PartialCopy = 0x8000000d,
            DeviceBusy = 0x80000011,
            InvalidEaName = 0x80000013,
            EaListInconsistent = 0x80000014,
            NoMoreEntries = 0x8000001a,
            LongJump = 0x80000026,
            DllMightBeInsecure = 0x8000002b,

            // Error
            Error = 0xc0000000,
            Unsuccessful = 0xc0000001,
            NotImplemented = 0xc0000002,
            InvalidInfoClass = 0xc0000003,
            InfoLengthMismatch = 0xc0000004,
            AccessViolation = 0xc0000005,
            InPageError = 0xc0000006,
            PagefileQuota = 0xc0000007,
            InvalidHandle = 0xc0000008,
            BadInitialStack = 0xc0000009,
            BadInitialPc = 0xc000000a,
            InvalidCid = 0xc000000b,
            TimerNotCanceled = 0xc000000c,
            InvalidParameter = 0xc000000d,
            NoSuchDevice = 0xc000000e,
            NoSuchFile = 0xc000000f,
            InvalidDeviceRequest = 0xc0000010,
            EndOfFile = 0xc0000011,
            WrongVolume = 0xc0000012,
            NoMediaInDevice = 0xc0000013,
            NoMemory = 0xc0000017,
            NotMappedView = 0xc0000019,
            UnableToFreeVm = 0xc000001a,
            UnableToDeleteSection = 0xc000001b,
            IllegalInstruction = 0xc000001d,
            AlreadyCommitted = 0xc0000021,
            AccessDenied = 0xc0000022,
            BufferTooSmall = 0xc0000023,
            ObjectTypeMismatch = 0xc0000024,
            NonContinuableException = 0xc0000025,
            BadStack = 0xc0000028,
            NotLocked = 0xc000002a,
            NotCommitted = 0xc000002d,
            InvalidParameterMix = 0xc0000030,
            ObjectNameInvalid = 0xc0000033,
            ObjectNameNotFound = 0xc0000034,
            ObjectNameCollision = 0xc0000035,
            ObjectPathInvalid = 0xc0000039,
            ObjectPathNotFound = 0xc000003a,
            ObjectPathSyntaxBad = 0xc000003b,
            DataOverrun = 0xc000003c,
            DataLate = 0xc000003d,
            DataError = 0xc000003e,
            CrcError = 0xc000003f,
            SectionTooBig = 0xc0000040,
            PortConnectionRefused = 0xc0000041,
            InvalidPortHandle = 0xc0000042,
            SharingViolation = 0xc0000043,
            QuotaExceeded = 0xc0000044,
            InvalidPageProtection = 0xc0000045,
            MutantNotOwned = 0xc0000046,
            SemaphoreLimitExceeded = 0xc0000047,
            PortAlreadySet = 0xc0000048,
            SectionNotImage = 0xc0000049,
            SuspendCountExceeded = 0xc000004a,
            ThreadIsTerminating = 0xc000004b,
            BadWorkingSetLimit = 0xc000004c,
            IncompatibleFileMap = 0xc000004d,
            SectionProtection = 0xc000004e,
            EasNotSupported = 0xc000004f,
            EaTooLarge = 0xc0000050,
            NonExistentEaEntry = 0xc0000051,
            NoEasOnFile = 0xc0000052,
            EaCorruptError = 0xc0000053,
            FileLockConflict = 0xc0000054,
            LockNotGranted = 0xc0000055,
            DeletePending = 0xc0000056,
            CtlFileNotSupported = 0xc0000057,
            UnknownRevision = 0xc0000058,
            RevisionMismatch = 0xc0000059,
            InvalidOwner = 0xc000005a,
            InvalidPrimaryGroup = 0xc000005b,
            NoImpersonationToken = 0xc000005c,
            CantDisableMandatory = 0xc000005d,
            NoLogonServers = 0xc000005e,
            NoSuchLogonSession = 0xc000005f,
            NoSuchPrivilege = 0xc0000060,
            PrivilegeNotHeld = 0xc0000061,
            InvalidAccountName = 0xc0000062,
            UserExists = 0xc0000063,
            NoSuchUser = 0xc0000064,
            GroupExists = 0xc0000065,
            NoSuchGroup = 0xc0000066,
            MemberInGroup = 0xc0000067,
            MemberNotInGroup = 0xc0000068,
            LastAdmin = 0xc0000069,
            WrongPassword = 0xc000006a,
            IllFormedPassword = 0xc000006b,
            PasswordRestriction = 0xc000006c,
            LogonFailure = 0xc000006d,
            AccountRestriction = 0xc000006e,
            InvalidLogonHours = 0xc000006f,
            InvalidWorkstation = 0xc0000070,
            PasswordExpired = 0xc0000071,
            AccountDisabled = 0xc0000072,
            NoneMapped = 0xc0000073,
            TooManyLuidsRequested = 0xc0000074,
            LuidsExhausted = 0xc0000075,
            InvalidSubAuthority = 0xc0000076,
            InvalidAcl = 0xc0000077,
            InvalidSid = 0xc0000078,
            InvalidSecurityDescr = 0xc0000079,
            ProcedureNotFound = 0xc000007a,
            InvalidImageFormat = 0xc000007b,
            NoToken = 0xc000007c,
            BadInheritanceAcl = 0xc000007d,
            RangeNotLocked = 0xc000007e,
            DiskFull = 0xc000007f,
            ServerDisabled = 0xc0000080,
            ServerNotDisabled = 0xc0000081,
            TooManyGuidsRequested = 0xc0000082,
            GuidsExhausted = 0xc0000083,
            InvalidIdAuthority = 0xc0000084,
            AgentsExhausted = 0xc0000085,
            InvalidVolumeLabel = 0xc0000086,
            SectionNotExtended = 0xc0000087,
            NotMappedData = 0xc0000088,
            ResourceDataNotFound = 0xc0000089,
            ResourceTypeNotFound = 0xc000008a,
            ResourceNameNotFound = 0xc000008b,
            ArrayBoundsExceeded = 0xc000008c,
            FloatDenormalOperand = 0xc000008d,
            FloatDivideByZero = 0xc000008e,
            FloatInexactResult = 0xc000008f,
            FloatInvalidOperation = 0xc0000090,
            FloatOverflow = 0xc0000091,
            FloatStackCheck = 0xc0000092,
            FloatUnderflow = 0xc0000093,
            IntegerDivideByZero = 0xc0000094,
            IntegerOverflow = 0xc0000095,
            PrivilegedInstruction = 0xc0000096,
            TooManyPagingFiles = 0xc0000097,
            FileInvalid = 0xc0000098,
            InstanceNotAvailable = 0xc00000ab,
            PipeNotAvailable = 0xc00000ac,
            InvalidPipeState = 0xc00000ad,
            PipeBusy = 0xc00000ae,
            IllegalFunction = 0xc00000af,
            PipeDisconnected = 0xc00000b0,
            PipeClosing = 0xc00000b1,
            PipeConnected = 0xc00000b2,
            PipeListening = 0xc00000b3,
            InvalidReadMode = 0xc00000b4,
            IoTimeout = 0xc00000b5,
            FileForcedClosed = 0xc00000b6,
            ProfilingNotStarted = 0xc00000b7,
            ProfilingNotStopped = 0xc00000b8,
            NotSameDevice = 0xc00000d4,
            FileRenamed = 0xc00000d5,
            CantWait = 0xc00000d8,
            PipeEmpty = 0xc00000d9,
            CantTerminateSelf = 0xc00000db,
            InternalError = 0xc00000e5,
            InvalidParameter1 = 0xc00000ef,
            InvalidParameter2 = 0xc00000f0,
            InvalidParameter3 = 0xc00000f1,
            InvalidParameter4 = 0xc00000f2,
            InvalidParameter5 = 0xc00000f3,
            InvalidParameter6 = 0xc00000f4,
            InvalidParameter7 = 0xc00000f5,
            InvalidParameter8 = 0xc00000f6,
            InvalidParameter9 = 0xc00000f7,
            InvalidParameter10 = 0xc00000f8,
            InvalidParameter11 = 0xc00000f9,
            InvalidParameter12 = 0xc00000fa,
            MappedFileSizeZero = 0xc000011e,
            TooManyOpenedFiles = 0xc000011f,
            Cancelled = 0xc0000120,
            CannotDelete = 0xc0000121,
            InvalidComputerName = 0xc0000122,
            FileDeleted = 0xc0000123,
            SpecialAccount = 0xc0000124,
            SpecialGroup = 0xc0000125,
            SpecialUser = 0xc0000126,
            MembersPrimaryGroup = 0xc0000127,
            FileClosed = 0xc0000128,
            TooManyThreads = 0xc0000129,
            ThreadNotInProcess = 0xc000012a,
            TokenAlreadyInUse = 0xc000012b,
            PagefileQuotaExceeded = 0xc000012c,
            CommitmentLimit = 0xc000012d,
            InvalidImageLeFormat = 0xc000012e,
            InvalidImageNotMz = 0xc000012f,
            InvalidImageProtect = 0xc0000130,
            InvalidImageWin16 = 0xc0000131,
            LogonServer = 0xc0000132,
            DifferenceAtDc = 0xc0000133,
            SynchronizationRequired = 0xc0000134,
            DllNotFound = 0xc0000135,
            IoPrivilegeFailed = 0xc0000137,
            OrdinalNotFound = 0xc0000138,
            EntryPointNotFound = 0xc0000139,
            ControlCExit = 0xc000013a,
            PortNotSet = 0xc0000353,
            DebuggerInactive = 0xc0000354,
            CallbackBypass = 0xc0000503,
            PortClosed = 0xc0000700,
            MessageLost = 0xc0000701,
            InvalidMessage = 0xc0000702,
            RequestCanceled = 0xc0000703,
            RecursiveDispatch = 0xc0000704,
            LpcReceiveBufferExpected = 0xc0000705,
            LpcInvalidConnectionUsage = 0xc0000706,
            LpcRequestsNotAllowed = 0xc0000707,
            ResourceInUse = 0xc0000708,
            ProcessIsProtected = 0xc0000712,
            VolumeDirty = 0xc0000806,
            FileCheckedOut = 0xc0000901,
            CheckOutRequired = 0xc0000902,
            BadFileType = 0xc0000903,
            FileTooLarge = 0xc0000904,
            FormsAuthRequired = 0xc0000905,
            VirusInfected = 0xc0000906,
            VirusDeleted = 0xc0000907,
            TransactionalConflict = 0xc0190001,
            InvalidTransaction = 0xc0190002,
            TransactionNotActive = 0xc0190003,
            TmInitializationFailed = 0xc0190004,
            RmNotActive = 0xc0190005,
            RmMetadataCorrupt = 0xc0190006,
            TransactionNotJoined = 0xc0190007,
            DirectoryNotRm = 0xc0190008,
            CouldNotResizeLog = 0xc0190009,
            TransactionsUnsupportedRemote = 0xc019000a,
            LogResizeInvalidSize = 0xc019000b,
            RemoteFileVersionMismatch = 0xc019000c,
            CrmProtocolAlreadyExists = 0xc019000f,
            TransactionPropagationFailed = 0xc0190010,
            CrmProtocolNotFound = 0xc0190011,
            TransactionSuperiorExists = 0xc0190012,
            TransactionRequestNotValid = 0xc0190013,
            TransactionNotRequested = 0xc0190014,
            TransactionAlreadyAborted = 0xc0190015,
            TransactionAlreadyCommitted = 0xc0190016,
            TransactionInvalidMarshallBuffer = 0xc0190017,
            CurrentTransactionNotValid = 0xc0190018,
            LogGrowthFailed = 0xc0190019,
            ObjectNoLongerExists = 0xc0190021,
            StreamMiniversionNotFound = 0xc0190022,
            StreamMiniversionNotValid = 0xc0190023,
            MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
            CantOpenMiniversionWithModifyIntent = 0xc0190025,
            CantCreateMoreStreamMiniversions = 0xc0190026,
            HandleNoLongerValid = 0xc0190028,
            NoTxfMetadata = 0xc0190029,
            LogCorruptionDetected = 0xc0190030,
            CantRecoverWithHandleOpen = 0xc0190031,
            RmDisconnected = 0xc0190032,
            EnlistmentNotSuperior = 0xc0190033,
            RecoveryNotNeeded = 0xc0190034,
            RmAlreadyStarted = 0xc0190035,
            FileIdentityNotPersistent = 0xc0190036,
            CantBreakTransactionalDependency = 0xc0190037,
            CantCrossRmBoundary = 0xc0190038,
            TxfDirNotEmpty = 0xc0190039,
            IndoubtTransactionsExist = 0xc019003a,
            TmVolatile = 0xc019003b,
            RollbackTimerExpired = 0xc019003c,
            TxfAttributeCorrupt = 0xc019003d,
            EfsNotAllowedInTransaction = 0xc019003e,
            TransactionalOpenNotAllowed = 0xc019003f,
            TransactedMappingUnsupportedRemote = 0xc0190040,
            TxfMetadataAlreadyPresent = 0xc0190041,
            TransactionScopeCallbacksNotSet = 0xc0190042,
            TransactionRequiredPromotion = 0xc0190043,
            CannotExecuteFileInTransaction = 0xc0190044,
            TransactionsNotFrozen = 0xc0190045,

            MaximumNtStatus = 0xffffffff
        }

        #endregion

        public static IntPtr NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS infoClass, uint infoLength = 0)
        {
            if (infoLength == 0)
                infoLength = 0x10000;

            var infoPtr = Marshal.AllocHGlobal((int)infoLength);

            var tries = 0;
            while (true)
            {
                var result = NtQuerySystemInformation(infoClass, infoPtr, infoLength, out infoLength);

                if (result == NtStatus.Success)
                    return infoPtr;

                Marshal.FreeHGlobal(infoPtr);  //free pointer when not Successful

                if (result != NtStatus.InfoLengthMismatch && result != NtStatus.BufferOverflow && result != NtStatus.BufferTooSmall)
                {
                    //throw new Exception("Unhandled NtStatus " + result);
                    return IntPtr.Zero;
                }

                if (++tries > 5)
                    return IntPtr.Zero;

                infoPtr = Marshal.AllocHGlobal((int)infoLength);
            }
        }

        
    }
}
