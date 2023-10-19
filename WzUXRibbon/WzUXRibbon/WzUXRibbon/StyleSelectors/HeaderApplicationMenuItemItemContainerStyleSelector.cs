using System.Windows.Controls;
using System.Windows;

namespace WzUXRibbon.StyleSelectors
{
    public class HeaderApplicationMenuItemItemContainerStyleSelector : StyleSelector
    {
        /// <summary>
        ///     A singleton instance for <see cref="HeaderApplicationMenuItemItemContainerStyleSelector" />.
        /// </summary>
        public static HeaderApplicationMenuItemItemContainerStyleSelector Instance { get; } = new HeaderApplicationMenuItemItemContainerStyleSelector();

        /// <inheritdoc />
        public override Style SelectStyle(object item, DependencyObject container)
        {
            switch (item)
            {
                case MenuItem _:
                    return (container as FrameworkElement)?.TryFindResource("Fluent.Ribbon.Styles.ApplicationMenu.MenuItemSecondLevel") as Style;
            }

            return base.SelectStyle(item, container);
        }
    }
}
