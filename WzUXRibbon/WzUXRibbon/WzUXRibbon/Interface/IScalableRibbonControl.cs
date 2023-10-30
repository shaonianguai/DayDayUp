using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzUXRibbon
{
    public interface IScalableRibbonControl
    {
        void ResetScale();

        void Enlarge();

        void Reduce();

        event EventHandler Scaled;
    }
}
