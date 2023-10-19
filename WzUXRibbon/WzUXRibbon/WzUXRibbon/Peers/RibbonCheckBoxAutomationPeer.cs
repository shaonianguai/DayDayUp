﻿namespace WzUXRibbon.Automation.Peers
{
    public class RibbonCheckBoxAutomationPeer : System.Windows.Automation.Peers.CheckBoxAutomationPeer
    {
        /// <summary>Initializes a new instance of the <see cref="T:ToggleButtonAutomationPeer" /> class.</summary>
        /// <param name="owner">The element associated with this automation peer.</param>
        public RibbonCheckBoxAutomationPeer(WzUXRibbon.Controls.CheckBox owner)
            : base(owner)
        {
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
