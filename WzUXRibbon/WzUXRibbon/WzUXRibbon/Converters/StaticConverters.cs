using System.Windows;

namespace WzUXRibbon.Converters
{

    public static class StaticConverters
    {
        public static readonly InvertNumericConverter InvertNumericConverter = new InvertNumericConverter();

        public static readonly ThicknessConverter ThicknessConverter = new ThicknessConverter();

        public static readonly ObjectToImageConverter ObjectToImageConverter = new ObjectToImageConverter();

        public static readonly ColorToSolidColorBrushValueConverter ColorToSolidColorBrushValueConverter = new ColorToSolidColorBrushValueConverter();

        public static readonly EqualsToVisibilityConverter EqualsToVisibilityConverter = new EqualsToVisibilityConverter();

        public static readonly InverseBoolConverter InverseBoolConverter = new InverseBoolConverter();
    }
}
