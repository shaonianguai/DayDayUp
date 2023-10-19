﻿using System.Windows.Automation.Peers;

namespace WzUXRibbon.Automation.Peers
{

    public class RibbonButtonAutomationPeer : ButtonAutomationPeer
    {
        /// <summary>Initializes a new instance of the <see cref="T:ButtonAutomationPeer" /> class.</summary>
        /// <param name="owner">The element associated with this automation peer.</param>
        public RibbonButtonAutomationPeer(WzUXRibbon.Controls.Button owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "RibbonButton";
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

        /// <inheritdoc />
        protected override string GetAccessKeyCore()
        {
            var text = ((WzUXRibbon.Controls.Button)this.Owner).KeyTip;
            if (string.IsNullOrEmpty(text))
            {
                text = base.GetAccessKeyCore();
            }

            return text;
        }

        /// <inheritdoc />
        protected override string GetHelpTextCore()
        {
            var text = base.GetHelpTextCore();

            if (string.IsNullOrEmpty(text))
            {
                if (((WzUXRibbon.Controls.Button)this.Owner).ToolTip is WzUXRibbon.Controls.ScreenTip ribbonToolTip)
                {
                    text = ribbonToolTip.Text;
                }
            }

            return text;
        }
    }
}
