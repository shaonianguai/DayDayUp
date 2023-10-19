using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace WzUXRibbon.Controls
{

    public class RibbonGroupsContainerScrollViewer : ScrollViewer
    {
        static RibbonGroupsContainerScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonGroupsContainerScrollViewer), new FrameworkPropertyMetadata(typeof(RibbonGroupsContainerScrollViewer)));
        }

        /// <inheritdoc />
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            if (this.ScrollInfo is null)
            {
                return;
            }

            // Prevent scrolling when a popup is open
            if (Mouse.Captured is IDropDownControl dropDownControl
                && dropDownControl.IsDropDownOpen == true
                && dropDownControl.DropDownPopup != null
                && dropDownControl as RibbonControl == null)
            {
                return;
            }

            if (e.Delta < 0)
            {
                this.LineRight();
            }
            else
            {
                this.LineLeft();
            }

            e.Handled = true;
        }
    }
}
