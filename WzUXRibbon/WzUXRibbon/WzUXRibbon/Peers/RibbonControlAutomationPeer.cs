using System.Windows.Automation.Peers;
using WzUXRibbon.Controls;

namespace WzUXRibbon.Automation.Peers
{
    public class RibbonControlAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonControlAutomationPeer(RibbonControl owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }
    }
}
