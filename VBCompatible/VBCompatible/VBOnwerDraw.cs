using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace VBCompatible
{
    public class VBOnwerDraw : VBNativeWindow
    {
        private static MethodInfo OnPaintMethodInfo =
                    typeof(Control).GetMethod("OnPaint", BindingFlags.Instance | BindingFlags.NonPublic);

        private static MethodInfo OnPaintBackgroundMethodInfo =
                    typeof(Control).GetMethod("OnPaintBackground", BindingFlags.Instance | BindingFlags.NonPublic);

        private delegate void OnPaintBackgroundDelegate(PaintEventArgs e);
        private delegate void OnPaintDelegate(PaintEventArgs e);
        private readonly OnPaintDelegate OnPaint;
        private readonly OnPaintBackgroundDelegate OnPaintBackground;

        private readonly bool CallOnPeint;
        private readonly NativeDrawMode DrawMode;

        /// <summary>
        /// コントロールのオーナードローを補佐します。
        /// </summary>
        /// <param name="owner">対象のコントロール。</param>
        /// <param name="callOnPaint">OnPaint/OnPaintBackground を呼び出して描画するときは True。
        /// コントロールに描画を任せるときは False。</param>
        /// <param name="drawMode">コントロールに描画を任せるときに WM_PRINTCLIENT を送るときは WmPrint。
        /// WM_PAINT を送るときは WmPaint。</param>
        public VBOnwerDraw(Control owner, bool callOnPaint, NativeDrawMode drawMode)
            : base(owner) {

            CallOnPeint = callOnPaint;
            DrawMode = drawMode;

            if (callOnPaint) {
                OnPaint = OnPaintMethodInfo.CreateDelegate<OnPaintDelegate>(owner);
                OnPaintBackground = OnPaintBackgroundMethodInfo.CreateDelegate<OnPaintBackgroundDelegate>(owner);
            }
        }

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case NativeMethods.WM_PAINT:
                    WmPaint(ref m);
                    break;

                case NativeMethods.WM_PRINTCLIENT:
                    WmPrintClient(ref m);
                    break;

                case NativeMethods.WM_ERASEBKGND:
                    break;

                case NativeMethods.WM_VSCROLL:
                case NativeMethods.WM_HSCROLL:
                    base.WndProc(ref m);
                    Owner.Invalidate();
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void WmPaint(ref Message m) {
            if (m.WParam == IntPtr.Zero) {
                var ps = new NativeMethods.PAINTSTRUCT();
                IntPtr hdc = NativeMethods.BeginPaint(m.HWnd, ref ps);
                try {
                    Rectangle clip = ps.rcPaint.Rectangle;
                    if (clip.Width > 0 && clip.Height > 0) {
                        IntPtr oldPal = NativeMethods.SetUpPalette(hdc, false, false);
                        try {
                            BufferedGraphicsContext bufferContext = BufferedGraphicsManager.Current;
                            using (var bufferedGraphics = bufferContext.Allocate(hdc, Owner.ClientRectangle)) {
                                using (var g = bufferedGraphics.Graphics) {
                                    OnUserPaint(g, clip);
                                    bufferedGraphics.Render();
                                }
                            }
                        } finally {
                            if (oldPal != IntPtr.Zero) {
                                NativeMethods.SelectPalette(hdc, oldPal, 0);
                            }
                        }
                    }

                } finally {
                    NativeMethods.EndPaint(m.HWnd, ref ps);
                }
            } else {
                using (var g = Graphics.FromHdc(m.WParam)) {
                    OnUserPaint(g, Owner.ClientRectangle);
                }
            }
        }

        private void WmPrintClient(ref Message m) {
            using (var g = Graphics.FromHdc(m.WParam)) {
                OnUserPaint(g, Owner.ClientRectangle);
            }
        }

        private void OnUserPaint(Graphics g, Rectangle clip) {
            g.SetClip(clip);
            if (CallOnPeint) {
                using (PaintEventArgs pevent = new PaintEventArgs(g, clip)) {
                    GraphicsState state = pevent.Graphics.Save();
                    try {
                        OnPaintBackground(pevent);
                    } finally {
                    }
                    try {
                        pevent.Graphics.Restore(state);
                        OnPaint(pevent);
                    } finally {

                    }
                }
            } else {
                DrawNative(g, clip);
            }
        }

        public void DrawNative(PaintEventArgs pe) {
            DrawNative(pe.Graphics, pe.ClipRectangle, DrawMode);
        }

        public void DrawNative(Graphics g, Rectangle clip) {
            DrawNative(g, clip, DrawMode);
        }

        public virtual void DrawNative(Graphics g, Rectangle clip, NativeDrawMode drawMode) {
            var brush = VBGraphicsCache.GetSolidBrush(Owner.BackColor);
            g.FillRectangle(brush, clip);
            IntPtr hdc = g.GetHdc();
            try {
                Message m;

                m = Message.Create(Handle, NativeMethods.WM_ERASEBKGND, hdc, IntPtr.Zero);
                DefWndProc(ref m);

                if (drawMode == NativeDrawMode.WmPrint) {
                    int flags = NativeMethods.PRF_CLIENT;
                    if (!Owner.IsContainer()) {
                        flags |= NativeMethods.PRF_CHILDREN;
                    }
                    m = Message.Create(Handle, NativeMethods.WM_PRINTCLIENT, hdc, (IntPtr)flags);
                } else {
                    m = Message.Create(Handle, NativeMethods.WM_PAINT, hdc, IntPtr.Zero);
                }
                DefWndProc(ref m);

            } finally {
                g.ReleaseHdc();
            }
        }
    }
}
