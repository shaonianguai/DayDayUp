﻿using System;
using System.Windows.Data;
using System.Windows.Media;

namespace WzUXRibbon.Converters
{
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToSolidColorBrushValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return null;

                case Color _:
                    return new SolidColorBrush((Color)value);
            }

            throw new InvalidOperationException($"Unsupported type [{value.GetType().Name}], ColorToSolidColorBrushValueConverter.Convert()");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return null;

                case SolidColorBrush brush:
                    return brush.Color;
            }

            throw new InvalidOperationException($"Unsupported type [{value.GetType().Name}], ColorToSolidColorBrushValueConverter.Convert()");
        }
    }
}
