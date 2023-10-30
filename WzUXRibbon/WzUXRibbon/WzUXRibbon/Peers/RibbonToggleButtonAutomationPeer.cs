namespace WzUXRibbon.Automation.Peers
{

    public class RibbonToggleButtonAutomationPeer : System.Windows.Automation.Peers.ToggleButtonAutomationPeer
    {
        /// <summary>Initializes a new instance of the <see cref="T:ToggleButtonAutomationPeer" /> class.</summary>
        /// <param name="owner">The element associated with this automation peer.</param>
        public RibbonToggleButtonAutomationPeer(WzUXRibbon.Controls.ToggleButton owner)
            : base(owner)
        {
        }

        
        protected override string GetClassNameCore()
        {
            return "RibbonToggleButton";
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
