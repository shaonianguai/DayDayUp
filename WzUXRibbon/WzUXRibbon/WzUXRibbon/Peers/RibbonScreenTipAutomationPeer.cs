using System.Windows.Automation.Peers;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Automation.Peers
{
    public class RibbonScreenTipAutomationPeer : ToolTipAutomationPeer
    {
        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        public RibbonScreenTipAutomationPeer(ScreenTip owner)
            : base(owner)
        {
        }

        
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        
        protected override string GetNameCore()
        {
            var name = base.GetNameCore();

            if (string.IsNullOrEmpty(name))
            {
                name = ((ScreenTip)this.Owner).Title;
            }

            return name;
        }
    }
}
