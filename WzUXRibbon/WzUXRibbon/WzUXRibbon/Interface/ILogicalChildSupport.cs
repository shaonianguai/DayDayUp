using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzUXRibbon
{
    public interface ILogicalChildSupport
    {
        void AddLogicalChild(object child);

        void RemoveLogicalChild(object child);
    }
}
