using System;
using System.ComponentModel;
using System.Globalization;
using WzUXRibbon.Data;

namespace WzUXRibbon.Converters
{
    public class SizeDefinitionConverter : TypeConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType.IsAssignableFrom(typeof(string));
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new RibbonControlSizeDefinition(value as string);
        }
    }
}
