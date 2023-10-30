using System.Windows.Controls;
using System.Windows;

namespace WzUXRibbon
{
    public interface IHeaderedControl
    {
        object Header { get; set; }
        DataTemplate HeaderTemplate { get; set; }
        DataTemplateSelector HeaderTemplateSelector { get; set; }
    }
}
