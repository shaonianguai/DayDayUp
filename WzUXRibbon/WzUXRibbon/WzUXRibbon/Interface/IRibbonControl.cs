using WzUXRibbon.Data;
using WzUXRibbon.Enumerations;

namespace WzUXRibbon
{
    public interface IRibbonControl : IHeaderedControl, IKeyTipedControl, ILogicalChildSupport
    {
        RibbonControlSize Size { get; set; }

        RibbonControlSizeDefinition SizeDefinition { get; set; }

        object Icon { get; set; }
    }
}
