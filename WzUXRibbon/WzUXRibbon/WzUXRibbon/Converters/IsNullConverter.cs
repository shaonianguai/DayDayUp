using System;
using System.Globalization;
using System.Windows.Data;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Converters
{
    public sealed class IsNullConverter : IValueConverter
    {
        /// <summary>
        ///     A singleton instance for <see cref="IsNullConverter" />.
        /// </summary>
        public static readonly IsNullConverter Instance = new IsNullConverter();

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BooleanBoxes.Box(value is null);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
