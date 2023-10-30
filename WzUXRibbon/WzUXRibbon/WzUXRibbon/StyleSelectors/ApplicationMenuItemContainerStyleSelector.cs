using System.Windows.Controls;
using System.Windows;

namespace WzUXRibbon.StyleSelectors
{
    public class ApplicationMenuItemContainerStyleSelector : StyleSelector
    {
        public static ApplicationMenuItemContainerStyleSelector Instance { get; } = new ApplicationMenuItemContainerStyleSelector();

        public override Style SelectStyle(object item, DependencyObject container)
        {
            switch (item)
            {
                case MenuItem _:
                    return (container as FrameworkElement)?.TryFindResource("Ribbon.Styles.ApplicationMenu.MenuItem") as Style;
            }

            return base.SelectStyle(item, container);
        }
    }
}
