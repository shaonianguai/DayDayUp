using WzUXRibbon.Data;
using WzUXRibbon.Enumerations;

namespace WzUXRibbon
{
    public interface IRibbonControl : IHeaderedControl, IKeyTipedControl, ILogicalChildSupport
    {
        /// <summary>
        /// Gets or sets Size for the element
        /// </summary>
        RibbonControlSize Size { get; set; }

        /// <summary>
        /// Gets or sets SizeDefinition for element
        /// </summary>
        RibbonControlSizeDefinition SizeDefinition { get; set; }

        /// <summary>
        /// Gets or sets Icon for the element
        /// </summary>
        object Icon { get; set; }
    }
}
