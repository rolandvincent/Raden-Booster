using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Raden_Booster
{
    public class FileToImageIconConverter
    {
        private string filePath;
        private System.Windows.Media.ImageSource icon;

        public string FilePath { get { return filePath; } }

        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                if (icon == null && System.IO.File.Exists(FilePath))
                {
                    try
                    {
                        using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(FilePath))
                        {
                            icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                      sysicon.Handle,
                                      System.Windows.Int32Rect.Empty,
                                      System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                        }
                    }catch
                    {
                        var sii = new WinAPI.Shell32.SHSTOCKICONINFO();
                        sii.cbSize = (UInt32)Marshal.SizeOf(typeof(WinAPI.Shell32.SHSTOCKICONINFO));
                        WinAPI.Shell32.SHGetStockIconInfo(WinAPI.Shell32.SHSTOCKICONID.SIID_APPLICATION,
                            WinAPI.Shell32.SHGSI.SHGSI_ICON,
                            ref sii);
                        icon = Imaging.CreateBitmapSourceFromHIcon(sii.hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }

                }

                return icon;
            }
        }

        public FileToImageIconConverter(string filePath)
        {
            this.filePath = filePath;
        }
    }
}
