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

        private bool CallOnPeint;
        private bool PrintMessage;

        /// <summary>
        /// コントロールのオーナードローを補佐します。
        /// </summary>
        /// <param name="owner">対象のコントロール。</param>
        /// <param name="callOnPaint">OnPaint/OnPaintBackground を呼び出して描画するときは True。
        /// コントロールに描画を任せるときは False。</param>
        /// <param name="printMessage">コントロールに描画を任せるときに WM_PRINTCLIENT を送るときは Ture。
        /// WM_PAINT を送るときは False。</param>
        public VBOnwerDraw(Control owner, bool callOnPaint, bool printMessage)
            : base(owner) {

            CallOnPeint = callOnPaint;
            PrintMessage = printMessage;

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
                    Rectangle clip = Rectangle.FromLTRB(ps.rcPaint_left, ps.rcPaint_top,
                                                    ps.rcPaint_right, ps.rcPaint_bottom);
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
                        pevent.Graphics.Restore(state);
                        OnPaint(pevent);
                    }
                }
            } else {
                DrawNative(g, clip);
            }
        }

        public virtual void DrawNative(Graphics g, Rectangle clip) {
            using (var brush = new SolidBrush(Owner.BackColor)) {
                g.FillRectangle(brush, clip);
            }
            IntPtr hdc = g.GetHdc();
            try {
                Message m;

                m = Message.Create(Handle, NativeMethods.WM_ERASEBKGND, hdc, IntPtr.Zero);
                DefWndProc(ref m);

                if (PrintMessage) {
                    int flags = NativeMethods.PRF_ERASEBKGND | NativeMethods.PRF_CLIENT;
                    if (Owner.IsSelectable()) {
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
