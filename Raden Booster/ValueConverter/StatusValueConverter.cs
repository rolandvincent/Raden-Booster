using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Raden_Booster
{
    public class StatusValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1: return "Suspeded";
                default: return "Running";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  CultureInfo culture)
        {
            var stringValue = value.ToString();
            if (stringValue == "Suspended") return 1;
            else return 0;
        }
    }
}
