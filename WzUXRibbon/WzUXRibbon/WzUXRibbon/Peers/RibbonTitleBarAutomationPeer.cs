using System.Windows.Automation.Peers;
using System.Windows.Controls;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Automation.Peers
{

    public class RibbonTitleBarAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTitleBarAutomationPeer(RibbonTitleBar owner)
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
            var contentPresenter = this.Owner as HeaderedContentControl;

            if (contentPresenter?.Header != null)
            {
                return contentPresenter.Header.ToString();
            }

            return base.GetNameCore();
        }
    }
}
