using WzUXRibbon.Enumerations;

namespace WzUXRibbon.Extensibility
{
    public interface IRibbonSizeChangedSink
    {
        /// <summary>
        /// Called when the size is changed
        /// </summary>
        /// <param name="previous">Size before change</param>
        /// <param name="current">Size after change</param>
        void OnSizePropertyChanged(RibbonControlSize previous, RibbonControlSize current);
    }
}
