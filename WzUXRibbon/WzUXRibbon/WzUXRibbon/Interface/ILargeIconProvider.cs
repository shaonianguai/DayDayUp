using System.Windows;
using WzUXRibbon.Helpers;

namespace WzUXRibbon
{
    public interface ILargeIconProvider
    {
        object LargeIcon { get; set; }
    }

    public class LargeIconProviderProperties : DependencyObject
    {
        private LargeIconProviderProperties()
        {
        }

        public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof(ILargeIconProvider.LargeIcon), typeof(object), typeof(LargeIconProviderProperties), new PropertyMetadata(LogicalChildSupportHelper.OnLogicalChildPropertyChanged));
    }
}
