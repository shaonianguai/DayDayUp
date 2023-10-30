using System;
using System.Globalization;
using System.Windows.Data;

namespace WzUXRibbonTest
{
    [ValueConversion(typeof(object), typeof(string))]
    public class CheckButtonGroupNameConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value?.GetHashCode()}_{(string)parameter}";
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
