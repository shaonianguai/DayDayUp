using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Automation;
using System.Windows.Input;
using System.Windows.Threading;
using WzUXRibbon.Automation.Peers;
using WzUXRibbon.Controls;
using WzUXRibbon.Internal;
using WzUXRibbon.Extensions;

namespace WzUXRibbon.Automation.Peers
{

    public class RibbonSplitButtonAutomationPeer : RibbonDropDownButtonAutomationPeer, IInvokeProvider
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonSplitButtonAutomationPeer(SplitButton owner)
            : base(owner)
        {
            this.SplitButtonOnwer = owner;
        }

        private SplitButton SplitButtonOnwer { get; }

        
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.SplitButton;
        }

        
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Invoke:
                    return this;

                default:
                    return base.GetPattern(patternInterface);
            }
        }

        
        protected override string GetAutomationIdCore()
        {
            var id = base.GetAutomationIdCore();

            if (string.IsNullOrEmpty(id))
            {
                if (this.SplitButtonOnwer.Command is RoutedCommand routedCommand
                    && string.IsNullOrEmpty(routedCommand.Name) == false)
                {
                    id = routedCommand.Name;
                }
            }

            return id ?? string.Empty;
        }

        
        protected override string GetNameCore()
        {
            var name = base.GetNameCore();

            if (string.IsNullOrEmpty(name))
            {
                if (this.SplitButtonOnwer.Command is RoutedUICommand routedUiCommand
                    && string.IsNullOrEmpty(routedUiCommand.Text) == false)
                {
                    name = routedUiCommand.Text;
                }
                else if (this.SplitButtonOnwer.Button?.Content is string buttonContent)
                {
                    name = AccessTextHelper.RemoveAccessKeyMarker(buttonContent);
                }
            }

            return name;
        }

        
        public void Invoke()
        {
            if (this.IsEnabled() == false)
            {
                throw new ElementNotEnabledException();
            }

            this.RunInDispatcherAsync(() => this.SplitButtonOnwer.AutomationButtonClick(), DispatcherPriority.Input);
        }
    }
}
