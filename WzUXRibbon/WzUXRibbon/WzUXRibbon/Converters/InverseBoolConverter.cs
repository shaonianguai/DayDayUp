using System;
using System.Globalization;
using System.Windows.Data;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BooleanBoxes.Box(!(bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BooleanBoxes.Box(!(bool)value);
        }
    }
}
