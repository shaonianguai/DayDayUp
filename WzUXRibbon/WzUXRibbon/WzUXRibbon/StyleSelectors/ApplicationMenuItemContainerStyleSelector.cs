using System.Windows.Controls;
using System.Windows;

namespace WzUXRibbon.StyleSelectors
{
    public class ApplicationMenuItemContainerStyleSelector : StyleSelector
    {
        /// <summary>
        ///     A singleton instance for <see cref="ApplicationMenuItemContainerStyleSelector" />.
        /// </summary>
        public static ApplicationMenuItemContainerStyleSelector Instance { get; } = new ApplicationMenuItemContainerStyleSelector();

        /// <inheritdoc />
        public override Style SelectStyle(object item, DependencyObject container)
        {
            switch (item)
            {
                case MenuItem _:
                    return (container as FrameworkElement)?.TryFindResource("Fluent.Ribbon.Styles.ApplicationMenu.MenuItem") as Style;
            }

            return base.SelectStyle(item, container);
        }
    }
}
