using WzUXRibbon.Data;

namespace WzUXRibbon
{
    public interface IKeyTipedControl
    {
        /// <summary>
        /// Gets and sets KeyTip for element.
        /// </summary>
        string KeyTip { get; set; }

        /// <summary>
        /// Handles key tip pressed
        /// </summary>
        KeyTipPressedResult OnKeyTipPressed();

        /// <summary>
        /// Handles back navigation with KeyTips
        /// </summary>
        void OnKeyTipBack();
    }
}
