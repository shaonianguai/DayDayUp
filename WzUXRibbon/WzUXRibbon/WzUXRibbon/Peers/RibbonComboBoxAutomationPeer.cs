﻿using System.Windows.Controls;

namespace WzUXRibbon.Automation.Peers
{
    public class RibbonComboBoxAutomationPeer : System.Windows.Automation.Peers.ComboBoxAutomationPeer
    {
        /// <summary>Initializes a new instance of the <see cref="T:ComboBoxAutomationPeer" /> class.</summary>
        /// <param name="owner">The element associated with this automation peer.</param>
        public RibbonComboBoxAutomationPeer(ComboBox owner)
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
