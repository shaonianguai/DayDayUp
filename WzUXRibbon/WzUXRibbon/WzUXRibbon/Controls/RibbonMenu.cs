using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows;

namespace WzUXRibbon.Controls
{
    [ContentProperty(nameof(Items))]
    public class RibbonMenu : MenuBase
    {
        #region Constructors

        static RibbonMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonMenu), new FrameworkPropertyMetadata(typeof(RibbonMenu)));
        }

        #endregion

        #region Overrides

        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }

        
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is System.Windows.Controls.MenuItem
                   || item is Separator;
        }

        #endregion
    }
}
