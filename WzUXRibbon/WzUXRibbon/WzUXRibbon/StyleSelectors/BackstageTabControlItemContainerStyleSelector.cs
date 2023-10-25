using System.Windows.Controls;
using System.Windows;
using WzUXRibbon.Controls;

namespace WzUXRibbon.StyleSelectors
{
    public class BackstageTabControlItemContainerStyleSelector : StyleSelector
    {
        /// <summary>
        ///     A singleton instance for <see cref="BackstageTabControlItemContainerStyleSelector" />.
        /// </summary>
        public static BackstageTabControlItemContainerStyleSelector Instance { get; } = new BackstageTabControlItemContainerStyleSelector();

        /// <inheritdoc />
        public override Style SelectStyle(object item, DependencyObject container)
        {
            switch (item)
            {
                case WzUXRibbon.Controls.Button _:
                    return (container as FrameworkElement)?.TryFindResource("Ribbon.Styles.BackstageTabControl.Button") as Style;

                case SeparatorTabItem _:
                    return (container as FrameworkElement)?.TryFindResource("Ribbon.Styles.BackstageTabControl.SeparatorTabItem") as Style;
            }

            return base.SelectStyle(item, container);
        }
    }
}
