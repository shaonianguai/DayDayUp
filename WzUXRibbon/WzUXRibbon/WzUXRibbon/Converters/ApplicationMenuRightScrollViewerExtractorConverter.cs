using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Converters
{
    public sealed class ApplicationMenuRightScrollViewerExtractorConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ApplicationMenu menu)
            {
                return menu.Template.FindName("PART_ScrollViewer", menu) as UIElement;
            }

            return value;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
