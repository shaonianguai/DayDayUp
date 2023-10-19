using System;
using System.Globalization;
using System.Windows.Data;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Converters
{
#pragma warning disable WPF0072
    [ValueConversion(typeof(bool), typeof(bool))]
#pragma warning restore WPF0072
    public class InverseBoolConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BooleanBoxes.Box(!(bool)value);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BooleanBoxes.Box(!(bool)value);
        }
    }
}
