using System;
using System.Drawing;
using System.Text;

namespace VBCompatible
{
    public class VBGraphics : IDisposable
    {
        readonly IntPtr hdc;
        IntPtr? oldFont;
        readonly Graphics graphics;
        public readonly Rectangle Clip;

        public VBGraphics(Graphics g, Rectangle clip) {
            graphics = g;
            Clip = clip;
            g.SetClip(clip);
            hdc = g.GetHdc();
        }

        public void SetFontHandle(IntPtr fontHandle) {
            IntPtr old = NativeMethods.SelectObject(hdc, fontHandle);
            if (!oldFont.HasValue) {
                oldFont = old;
            }
        }

        public void SetColor(Color foreColor, Color backColor) {
            NativeMethods.SetTextColor(hdc, ColorTranslator.ToWin32(foreColor));
            NativeMethods.SetBkColor(hdc, ColorTranslator.ToWin32(backColor));
        }

        public void TextOut(int x, int y, string text) {
            if (!string.IsNullOrEmpty(text)) {
                NativeMethods.TextOut(hdc, x, y, text, text.Length);
            }
        }

        public void TextOut(int x, int y, StringBuilder sb) {
            NativeMethods.TextOut(hdc, x, y, sb, sb.Length);
        }

        private bool disposedValue;
        private void CallDispose(bool disposing) {
            if (!disposedValue) {
                disposedValue = true;
                Dispose(disposing);
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
            }
            if (oldFont.HasValue) {
                NativeMethods.SelectObject(hdc, oldFont.Value);
            }
            graphics.ReleaseHdc();
        }

        ~VBGraphics() {
            CallDispose(false);
        }

        public void Dispose() {
            CallDispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
