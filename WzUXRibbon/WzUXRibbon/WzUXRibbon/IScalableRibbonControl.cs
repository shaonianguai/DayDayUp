using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzUXRibbon
{
    public interface IScalableRibbonControl
    {
        /// <summary>
        /// Resets the scale.
        /// </summary>
        void ResetScale();

        /// <summary>
        /// Enlarge control size.
        /// </summary>
        void Enlarge();

        /// <summary>
        /// Reduce control size.
        /// </summary>
        void Reduce();

        /// <summary>
        /// Occurs when control is scaled.
        /// </summary>
        event EventHandler Scaled;
    }
}
