using Microsoft.Win32;
using System;
using System.Windows.Media;

namespace Raden_Booster
{
    public enum StarupType
    {
        Registry,
        Shortcut
    }
    public enum SourceDestination
    {
        HKCU,
        HKLM,
        HKLM32
    }
    public class StartupData
    {
        public ImageSource Icon
        {
            get
            {
                FileToImageIconConverter fileToImage = new FileToImageIconConverter(Location);
                return fileToImage.Icon;
            }
        }
        public String Name { get; set; }
        public String Location { get; set; }
        public String CommandLine { get; set; }
        public String Publisher { get; set; } = "";
        public StarupType Type;
        public String Source { get; set; }
        public String SourceName { get; set; }
        public SourceDestination RegDestination;
        public String StartupEnablePath { get; set; }
        public String StartupPath
        {
            get
            {
                if (Type == StarupType.Shortcut)
                {
                    return RegDestination == SourceDestination.HKCU ? "Appdata" : "ProgramData";
                }
                else
                {
                    return RegDestination == SourceDestination.HKCU ? "HKCU" : "HKLM";
                }
            }
        }
        public bool? Enabled
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor > 1)
                {
                    byte[] binaryContent;
                    if (RegDestination == SourceDestination.HKCU)
                        binaryContent = Registry.CurrentUser.OpenSubKey(StartupEnablePath)?.GetValue(SourceName, new byte[] { 0x2, 0x0, 0x0 }) as byte[];
                    else
                    {
                        if (Environment.Is64BitOperatingSystem)
                            binaryContent = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(StartupEnablePath).GetValue(SourceName, new byte[] { 0x2, 0x0, 0x0 }) as byte[];
                        else
                            binaryContent = Registry.LocalMachine.OpenSubKey(StartupEnablePath).GetValue(SourceName, new byte[] { 0x2, 0x0, 0x0 }) as byte[];
                    }
                    return binaryContent[0] == 0x2;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major > 6 && Environment.OSVersion.Version.Minor > 1)
                {
                    if (RegDestination == SourceDestination.HKCU)
                        Registry.SetValue(Registry.CurrentUser.Name + "\\" + StartupEnablePath, SourceName, value == true ? new byte[] { 0x2, 0x0, 0x0 } : new byte[] { 0x3, 0x0, 0x0 }, RegistryValueKind.Binary);
                    else
                    {
                        if (Environment.Is64BitOperatingSystem)
                            RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(StartupEnablePath, true).SetValue(SourceName, value == true ? new byte[] { 0x2, 0x0, 0x0 } : new byte[] { 0x3, 0x0, 0x0 }, RegistryValueKind.Binary);
                        else
                            Registry.LocalMachine.OpenSubKey(StartupEnablePath, true).SetValue(SourceName, value == true ? new byte[] { 0x2, 0x0, 0x0 } : new byte[] { 0x3, 0x0, 0x0 }, RegistryValueKind.Binary);
                    }
                }
            }
        }

        public bool SupportEnabled
        {
            get => Enabled != null;
        }
    }
}
