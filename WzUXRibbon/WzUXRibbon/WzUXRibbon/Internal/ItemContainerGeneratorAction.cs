using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace WzUXRibbon.Internal
{
    internal class ItemContainerGeneratorAction
    {
        public ItemContainerGeneratorAction(ItemContainerGenerator generator, Action action)
        {
            this.Generator = generator;
            this.Action = action;
        }

        public ItemContainerGenerator Generator { get; }

        public Action Action { get; }

        public bool IsWaitingForGenerator { get; private set; }

        public void QueueAction()
        {
            if (this.Generator.Status != GeneratorStatus.ContainersGenerated)
            {
                if (this.IsWaitingForGenerator)
                {
                    return;
                }

                this.IsWaitingForGenerator = true;
                this.Generator.StatusChanged += this.HandleItemContainerGenerator_StatusChanged;
                return;
            }

            this.IsWaitingForGenerator = false;
            this.Generator.StatusChanged -= this.HandleItemContainerGenerator_StatusChanged;

            this.Action();
        }

        private void HandleItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            this.QueueAction();
        }
    }
}
