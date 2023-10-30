using System.Windows;
using WzUXRibbon.Helpers;

namespace WzUXRibbon
{
    public interface IMediumIconProvider
    {
        object MediumIcon { get; set; }
    }

    public class MediumIconProviderProperties : DependencyObject
    {
        private MediumIconProviderProperties()
        {
        }

        public static readonly DependencyProperty MediumIconProperty = DependencyProperty.Register(nameof(IMediumIconProvider.MediumIcon), typeof(object), typeof(MediumIconProviderProperties), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));
    }
}
