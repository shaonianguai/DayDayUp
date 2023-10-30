using System;
using System.Globalization;
using System.Windows.Data;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Converters
{
    public class EqualsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == parameter
                || (value != null && value.Equals(parameter)))
            {
                return VisibilityBoxes.Visible;
            }

            return VisibilityBoxes.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
