using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace VBCompatible
{
    /// <summary>
    /// グローバルメモリを管理する SafeHandle です。
    /// </summary>
    public class VBSafeGlobalMemory : SafeHandle
    {
        static IntPtr INVALID_HANDLE = IntPtr.Zero;

        /// <summary>
        /// グローバルメモリハンドルから VBSafeGlobalMemory のインスタンスを作成します。
        /// </summary>
        /// <param name="hGlobal">グローバルメモリハンドル</param>
        /// <returns>VBSafeGlobalMemory のインスタンス</returns>
        public static VBSafeGlobalMemory FromHGlobal(IntPtr hGlobal) {
            return new VBSafeGlobalMemory(hGlobal);
        }

        /// <summary>
        /// 指定したサイズのグローバルメモリを確保して VBSafeGlobalMemory のインスタンスを作成します。
        /// </summary>
        /// <param name="size">グローバルメモリハンドル</param>
        /// <returns>VBSafeGlobalMemory のインスタンス</returns>
        public static VBSafeGlobalMemory FromSize(int size) {
            IntPtr handle = NativeMethods.GlobalAlloc(NativeMethods.GMEM_MOVEABLE, (IntPtr)size);
            if (handle == INVALID_HANDLE) {
                throw new Win32Exception();
            }
            return new VBSafeGlobalMemory(handle);
        }

        /// <summary>
        /// グローバルメモリハンドルを指定して VBSafeGlobalMemory のインスタンスを作成します。
        /// </summary>
        /// <param name="hMem">グローバルメモリハンドル。</param>
        private VBSafeGlobalMemory(IntPtr hMem)
            : base(INVALID_HANDLE, true) {
            handle = hMem;
        }

        /// <summary>
        /// ハンドル値が無効かどうかを示す値を取得します。
        /// </summary>
        public override bool IsInvalid {
            get {
                return handle == INVALID_HANDLE;
            }
        }

        /// <summary>
        /// グローバルメモリをロックしてアドレスを取得します。
        /// </summary>
        /// <returns>グローバルメモリのアドレス。</returns>
        public IntPtr Lock() {
            return NativeMethods.GlobalLock(handle);
        }

        /// <summary>
        /// グローバルメモリをアンロックします。
        /// </summary>
        /// <returns>参照カウントがゼロでない場合は true、そうでなければ false を返します。</returns>
        public bool Unlock() {
            return NativeMethods.GlobalUnlock(handle);
        }

        /// <summary>
        /// グローバルメモリを再割り当てします。
        /// </summary>
        /// <param name="size">再割り当てするサイズ</param>
        /// <returns>再割り当てされたメモリオブジェクトのハンドル</returns>
        public IntPtr ReAlloc(int size) {
            IntPtr hMem = NativeMethods.GlobalReAlloc(handle, (IntPtr)size, NativeMethods.GMEM_MOVEABLE);
            if (hMem == INVALID_HANDLE) {
                throw new Win32Exception();
            }
            return hMem;
        }

        /// <summary>
        /// グローバルメモリのサイズを取得します。
        /// </summary>
        public int Size {
            get {
                return NativeMethods.GlobalSize(handle);
            }
        }

        /// <summary>
        /// ハンドルを解放するために必要なコードを実行します。
        /// </summary>
        /// <returns>ハンドルが正常に解放された場合は true。深刻なエラーが発生した場合は false。</returns>
        protected override bool ReleaseHandle() {
            if (base.handle != INVALID_HANDLE) {
                while (Unlock()) { }
                if (NativeMethods.GlobalFree(handle) != INVALID_HANDLE) {
                    throw new Win32Exception();
                }
                base.handle = INVALID_HANDLE;
                return true;
            } else {
                return false;
            }
        }
    }
}
