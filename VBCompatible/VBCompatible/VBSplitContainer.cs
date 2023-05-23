using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible
{
    [DesignerCategory("Code")]
    public class VBSplitContainer : SplitContainer
    {
        protected override bool ProcessTabKey(bool forward)
        {
            return false;
        }
    }
}
