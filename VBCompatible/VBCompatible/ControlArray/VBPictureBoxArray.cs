using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBPictureBox))]
    public class VBPictureBoxArray : VBControllArray<VBPictureBox>
    {
        public VBPictureBoxArray() { }

        public VBPictureBoxArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBPictureBox target) {
            base.HookUpControl(target);
            if (LoadCompleted != null) target.LoadCompleted += LoadCompleted;
            if (LoadProgressChanged != null) target.LoadProgressChanged += LoadProgressChanged;
            if (SizeModeChanged != null) target.SizeModeChanged += SizeModeChanged;
        }

        public AsyncCompletedEventHandler LoadCompleted;
        public ProgressChangedEventHandler LoadProgressChanged;
        public EventHandler SizeModeChanged;

    }
}
