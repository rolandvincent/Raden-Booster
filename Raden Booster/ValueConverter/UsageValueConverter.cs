using System;
using System.Globalization;
using System.Windows.Data;

namespace Raden_Booster
{
    public class UsageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            return string.Format("{0:#,0.##} K", (long)value / 1024d);    
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  CultureInfo culture)
        {
            return 0;
        }
    }
}
