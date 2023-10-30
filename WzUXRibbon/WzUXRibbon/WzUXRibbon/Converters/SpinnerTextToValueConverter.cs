using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WzUXRibbon.Converters
{

    public class SpinnerTextToValueConverter : IValueConverter
    {
        public static readonly SpinnerTextToValueConverter DefaultInstance = new SpinnerTextToValueConverter();

        
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converterParam = (Tuple<string, double>)parameter;
            var format = converterParam.Item1;
            var previousValue = converterParam.Item2;

            return this.TextToDouble((string)value, format, previousValue, culture);
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.DoubleToText((double)value, (string)parameter, culture);
        }

        public virtual double TextToDouble(string text, string format, double previousValue, CultureInfo culture)
        {
            var stringBuilder = new StringBuilder();

            foreach (var symbol in text)
            {
                if (char.IsDigit(symbol)
                    || symbol == ','
                    || symbol == '.'
                    || (symbol == '-' && stringBuilder.Length == 0))
                {
                    stringBuilder.Append(symbol);
                }
            }

            text = stringBuilder.ToString();

            if (double.TryParse(text, NumberStyles.Any, culture, out var doubleValue) == false)
            {
                doubleValue = previousValue;
            }

            return doubleValue;
        }

        public virtual string DoubleToText(double value, string format, CultureInfo culture)
        {
            return value.ToString(format, culture);
        }
    }
}
