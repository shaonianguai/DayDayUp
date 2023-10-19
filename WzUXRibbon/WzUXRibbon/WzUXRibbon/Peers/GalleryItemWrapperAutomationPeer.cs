using System.Windows.Automation.Peers;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Automation.Peers
{
    public class GalleryItemWrapperAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <inheritdoc cref="FrameworkElementAutomationPeer" />
        public GalleryItemWrapperAutomationPeer(GalleryItem owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore() => "ListBoxItem";

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.ListItem;
    }
}
