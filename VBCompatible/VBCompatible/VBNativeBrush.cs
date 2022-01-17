using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace VBCompatible
{
    public class VBNativeBrush : SafeHandle
    {
        public VBNativeBrush(Color color) 
            :base(IntPtr.Zero, true) {
            int intColor = ColorTranslator.ToWin32(color);
            SetHandle(NativeMethods.CreateSolidBrush(intColor));
        }

        protected override bool ReleaseHandle() {
            return NativeMethods.DeleteObject(handle);
        }

        public override bool IsInvalid {
            get {
                return handle == IntPtr.Zero;
            }
        }

        public static implicit operator IntPtr(VBNativeBrush value) {
            return value.handle;
        }
    }
}
