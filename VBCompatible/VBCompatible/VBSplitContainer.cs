using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VBCompatible
{
    class VBSplitContainer : SplitContainer
    {
        protected override bool ProcessTabKey(bool forward)
        {
            Control controlActive = ActiveControl;
            if (controlActive == null)
            {
                return base.ProcessTabKey(forward);
            }
            Control controlNext = GetNextControl(controlActive, forward);
            if (controlNext != null)
            {
                return controlNext.Focus();
            }
            return false;

        }
    }
}
