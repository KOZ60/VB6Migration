using System;
using System.Runtime.InteropServices;

namespace VBCompatible
{
    /// <summary>
    /// ポインターを保持するクラス
    /// </summary>
    public class VBSafePointer : SafeHandle
    {
        GCHandle _GCHandle;

        internal VBSafePointer(object obj)
            : base(IntPtr.Zero, true) {
            _GCHandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            SetHandle(_GCHandle.AddrOfPinnedObject());
        }

        /// <summary>
        /// ハンドル値が無効かどうかを示す値を取得します。
        /// </summary>
        public override bool IsInvalid {
            get {
                return !_GCHandle.IsAllocated;
            }
        }

        /// <summary>
        /// ハンドルを解放します。
        /// </summary>
        protected override bool ReleaseHandle() {
            _GCHandle.Free();
            return true;
        }

        /// <summary>
        /// ハンドルが表すオブジェクトを取得します。
        /// </summary>
        public object Target {
            get {
                return _GCHandle.Target;
            }
        }

        /// <summary>
        /// 現在の VBSafePointer を表す System.String を返します。
        /// </summary>
        public override string ToString() {
            if (!this.IsInvalid) {
                return string.Format("0x{0:X}", handle);
            } else {
                return "Invalid";
            }
        }

        /// <summary>
        /// IntPtr に暗黙の型変換を行います。
        /// </summary>
        /// <param name="value">型変換を行う VBSafePointer</param>
        /// <returns>型変換された IntPtr</returns>
        public static implicit operator IntPtr(VBSafePointer value) {
            return value.handle;
        }

    }
}
