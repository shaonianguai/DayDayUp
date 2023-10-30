using System;
using System.ComponentModel;
using System.Globalization;
using WzUXRibbon.Data;

namespace WzUXRibbon.Converters
{
    public class RibbonGroupBoxStateDefinitionConverter : TypeConverter
    {
        
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType.IsAssignableFrom(typeof(string));
        }

        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new RibbonGroupBoxStateDefinition(value as string);
        }
    }
}
