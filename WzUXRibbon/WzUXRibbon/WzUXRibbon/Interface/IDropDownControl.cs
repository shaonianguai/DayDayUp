using System;
using System.Windows.Controls.Primitives;

namespace WzUXRibbon
{
    public interface IDropDownControl
    {
        Popup DropDownPopup { get; }
        bool IsContextMenuOpened { get; set; }
        bool IsDropDownOpen { get; set; }
        event EventHandler DropDownOpened;
        event EventHandler DropDownClosed;
    }
}
