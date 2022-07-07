using System;
using System.Globalization;
using System.Windows.Data;

namespace Raden_Booster
{
    public class BoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            var boolValue = (string)value;
            if (boolValue == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  CultureInfo culture)
        {
            var stringValue = (bool)value;
            if (stringValue == true)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
    }
}
