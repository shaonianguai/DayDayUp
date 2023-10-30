using System.Windows.Controls;
using System.Windows;

namespace WzUXRibbon.StyleSelectors
{
    public class SplitedApplicationMenuItemItemContainerStyleSelector : StyleSelector
    {
        public static SplitedApplicationMenuItemItemContainerStyleSelector Instance { get; } = new SplitedApplicationMenuItemItemContainerStyleSelector();

        public override Style SelectStyle(object item, DependencyObject container)
        {
            switch (item)
            {
                case MenuItem _:
                    return (container as FrameworkElement)?.TryFindResource("Ribbon.Styles.ApplicationMenu.MenuItemSecondLevel") as Style;
            }

            return base.SelectStyle(item, container);
        }
    }
}
