using System;
using Microsoft.Win32.SafeHandles;

namespace VBCompatible
{
    public class VBSafeDCHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public VBSafeDCHandle(IntPtr hdc) : base(true) {
            SetHandle(hdc);
        }

        protected override bool ReleaseHandle() {
            return NativeMethods.DeleteDC(handle);
        }
    }
}
