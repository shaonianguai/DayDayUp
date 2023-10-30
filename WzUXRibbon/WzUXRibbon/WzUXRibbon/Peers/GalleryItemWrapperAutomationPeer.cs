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

        
        protected override string GetClassNameCore() => "ListBoxItem";

        
        protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.ListItem;
    }
}
