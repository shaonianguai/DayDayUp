using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzUXRibbon.Automation.Peers
{
    public class RibbonTextBoxAutomationPeer : System.Windows.Automation.Peers.TextBoxAutomationPeer
    {
        /// <summary>Initializes a new instance of the <see cref="T:TextBoxAutomationPeer" /> class.</summary>
        /// <param name="owner">The element associated with this automation peer.</param>
        public RibbonTextBoxAutomationPeer(WzUXRibbon.Controls.TextBox owner)
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
                name = (this.Owner as IHeaderedControl)?.Header as string;
            }

            return name;
        }
    }
}
