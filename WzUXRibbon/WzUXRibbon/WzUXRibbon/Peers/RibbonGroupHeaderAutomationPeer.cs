using System.Windows.Automation.Peers;
using System.Windows;

namespace WzUXRibbon.Automation.Peers
{

    public class RibbonGroupHeaderAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonGroupHeaderAutomationPeer(FrameworkElement owner)
            : base(owner)
        {
        }

        
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Header;
        }

        
        protected override bool IsContentElementCore()
        {
            return false;
        }

        
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        
        protected override string GetNameCore()
        {
            var parent = this.GetParent();
            if (parent != null)
            {
                return parent.GetName();
            }

            return string.Empty;
        }
    }
}
