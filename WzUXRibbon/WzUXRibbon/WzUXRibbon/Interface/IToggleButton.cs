using System.Windows;

namespace WzUXRibbon
{
    public interface IToggleButton
    {
        string GroupName { get; set; }

        bool? IsChecked { get; set; }

        bool IsLoaded { get; }

        DependencyObject Parent { get; }
    }
}
