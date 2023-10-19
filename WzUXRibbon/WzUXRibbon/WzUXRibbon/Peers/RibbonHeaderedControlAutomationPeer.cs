using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows;

namespace WzUXRibbon.Automation.Peers
{

    public abstract class RibbonHeaderedControlAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected RibbonHeaderedControlAutomationPeer(FrameworkElement owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        /// <inheritdoc />
        protected override string GetNameCore()
        {
            var name = base.GetNameCore();

            if (string.IsNullOrEmpty(name))
            {
                name = (this.Owner as IHeaderedControl)?.Header as string;
            }

            return name;
        }
    }
}
