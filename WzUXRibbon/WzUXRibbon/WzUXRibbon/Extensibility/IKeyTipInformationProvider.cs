using System.Collections.Generic;
using WzUXRibbon.Data;

namespace WzUXRibbon.Extensibility
{
    public interface IKeyTipInformationProvider
    {
        IEnumerable<KeyTipInformation> GetKeyTipInformations(bool hide);
    }
}
