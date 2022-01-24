using System.Windows.Forms;

namespace VBCompatible
{
    public class VBSplitContainer : SplitContainer
    {
        protected override bool ProcessTabKey(bool forward)
        {
            return false;
        }
    }
}
