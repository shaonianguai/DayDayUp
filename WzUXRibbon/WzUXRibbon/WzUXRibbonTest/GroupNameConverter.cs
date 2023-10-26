using System;
using System.Globalization;
using System.Windows.Data;

namespace WzUXRibbonTest
{
    [ValueConversion(typeof(object), typeof(string))]
    public class CheckButtonGroupNameConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value?.GetHashCode()}_{(string)parameter}";
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
