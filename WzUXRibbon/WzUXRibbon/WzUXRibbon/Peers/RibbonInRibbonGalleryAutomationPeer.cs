using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Automation;
using WzUXRibbon.Controls;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Automation.Peers
{

    public class RibbonInRibbonGalleryAutomationPeer : SelectorAutomationPeer, IExpandCollapseProvider
    {
        private readonly InRibbonGallery owner;

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        public RibbonInRibbonGalleryAutomationPeer(InRibbonGallery owner)
            : base(owner)
        {
            this.owner = owner;
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

        
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.List;
        }

        
        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            return new GalleryItemAutomationPeer(item, this);
        }

        
        public override object GetPattern(PatternInterface patternInterface)
        {
            if(patternInterface == PatternInterface.ExpandCollapse
                || (this.owner.IsDropDownOpen == false && patternInterface == PatternInterface.Scroll))
            {
                return this;
            }
            else
            {
                return base.GetPattern(patternInterface);
            }
        }

        
        public void Collapse()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.owner.SetCurrentValue(InRibbonGallery.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
        }

        
        public void Expand()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            this.owner.SetCurrentValue(InRibbonGallery.IsDropDownOpenProperty, BooleanBoxes.TrueBox);
        }

        
        public ExpandCollapseState ExpandCollapseState => this.owner.IsDropDownOpen
            ? ExpandCollapseState.Expanded
            : ExpandCollapseState.Collapsed;

        // BUG 1555137: Never inline, as we don't want to unnecessarily link the automation DLL
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
        {
            this.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
                oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed,
                newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
        }
    }
}
