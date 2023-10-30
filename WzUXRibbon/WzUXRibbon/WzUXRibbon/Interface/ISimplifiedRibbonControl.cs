using WzUXRibbon.Data;

namespace WzUXRibbon
{
    public interface ISimplifiedRibbonControl : ISimplifiedStateControl
    {
        RibbonControlSizeDefinition SimplifiedSizeDefinition { get; set; }

        bool IsSimplified { get; }
    }
}
