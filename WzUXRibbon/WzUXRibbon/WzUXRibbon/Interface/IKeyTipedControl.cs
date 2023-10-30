using WzUXRibbon.Data;

namespace WzUXRibbon
{
    public interface IKeyTipedControl
    {
        string KeyTip { get; set; }
        KeyTipPressedResult OnKeyTipPressed();
        void OnKeyTipBack();
    }
}
