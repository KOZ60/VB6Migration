using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VBCompatible
{
    public class VBTextBoxRenderer
    {

        public static void DrawTextBox(Graphics graphics, TextBox textBox, Rectangle clip) {
            string text = textBox.Text;
            if (textBox.PasswordChar != (char)0) {
                text = new string(textBox.PasswordChar, text.Length);
            }
            var renderer = new VBTextBoxRenderer(graphics, clip,
                                        textBox.ForeColor, textBox.BackColor,
                                        textBox.Handle,
                                        text,
                                        textBox.IsEnabled(),
                                        textBox.BorderStyle
                                        );
            renderer.Draw();
        }

        public static void DrawTextBox(Graphics graphics, ComboBox comboBox, Rectangle clip) {
            NativeMethods.ComboBoxInfo info = new NativeMethods.ComboBoxInfo();
            NativeMethods.GetComboBoxInfo(comboBox.Handle, info);
            var renderer = new VBTextBoxRenderer(graphics, clip,
                                        comboBox.ForeColor, comboBox.BackColor,
                                        info.EditBoxHandle,
                                        comboBox.Text,
                                        comboBox.IsEnabled(),
                                        BorderStyle.Fixed3D
                                        );
            renderer.Draw();
        }

        private Graphics Graphics;
        private IntPtr hwnd;
        private Color ForeColor;
        private Color BackColor;
        private Rectangle Clip;
        private bool Enabled;
        private string Text;
        private Rectangle ClientRectangle;
        private IntPtr FontHandle;
        private bool MultiLine;
        private bool DrawBorder;
        private bool HideSelection;
        private SelectionRange selectionRange;
        private int SingleTop;
        private bool Focused;

        private VBTextBoxRenderer(Graphics graphics, 
                                  Rectangle clip, 
                                  Color foreColor, 
                                  Color backColor, 
                                  IntPtr windowHandle, 
                                  string text, 
                                  bool enabled, 
                                  BorderStyle borderStyle) {

            Graphics = graphics;
            Clip = clip;
            ForeColor = foreColor;
            BackColor = backColor;
            hwnd = windowHandle;
            Text = text;
            Enabled = enabled;

            NativeMethods.RECT rect = new NativeMethods.RECT();
            NativeMethods.GetClientRect(hwnd, ref rect);
            ClientRectangle = rect.Rectangle;

            FontHandle = NativeMethods.SendMessage(hwnd, NativeMethods.WM_GETFONT, IntPtr.Zero, IntPtr.Zero);
            int style = (int)(long)NativeMethods.GetWindowLongPtr(hwnd, NativeMethods.GWL_STYLE);
            MultiLine = (style & NativeMethods.ES_MULTILINE) == NativeMethods.ES_MULTILINE;
            HideSelection = (style & NativeMethods.ES_NOHIDESEL) != NativeMethods.ES_NOHIDESEL;

            // FixedSingle のときにはクライアント領域に線が引かれる
            // WS_BORDER が Style から消えてしまっている。
            DrawBorder = borderStyle == BorderStyle.FixedSingle; 
            selectionRange = new SelectionRange(hwnd);

            if (!MultiLine) {
                NativeMethods.SendMessage(hwnd, NativeMethods.EM_GETRECT, IntPtr.Zero, ref rect);
                SingleTop = rect.Top;
            }
            Focused = NativeMethods.GetFocus() == windowHandle;
        }

        private void Draw() {
            Graphics.Clear(BackColor);
            using (var g = new VBGraphics(Graphics, Clip)) {
                g.SetFontHandle(FontHandle);
                DrawText(g, Clip, Text);
            }

            if (DrawBorder) {
                Rectangle borderRect = ClientRectangle.DeflateRect(new Padding(1));
                Graphics.DrawRectangle(SystemPens.WindowFrame, borderRect);
            }
        }

        private void DrawText(VBGraphics g, Rectangle clip, string text) {
            int startLine = GetFirstVisibleLine();
            int lineCount = GetLineCount();
            int bottom = clip.Bottom;
            List<DrawRange> lst = new List<DrawRange>();

            for (int i = startLine; i < lineCount; i++) {

                // 行の最初の文字位置を取得
                int lineStart = GetFirstCharIndexFromLine(i);
                if (lineStart == -1) {
                    break;
                }
                // 行の最初の文字の座標を取得
                Point? pt = GetPositionFromCharIndex(lineStart);
                if (!pt.HasValue || pt.Value.Y > bottom) {
                    break;
                }

                // 行の長さを取得(改行が含まれない)
                int lineLength = GetLineLength(i);
                int lineEnd = lineStart + lineLength - 1;

                if (lineEnd - lineStart + 1 > 0) {
                    DrawRange drawRange = new DrawRange(pt.Value, lineStart, lineEnd);
                    lst.Add(drawRange);
                }
            }

            for (int i = 0; i < lst.Count; i++) {
                DrawLine(g, lst[i], text);
            }

        }

        private enum TextDrawMode
        {
            Normal,
            Highlight,
            Disable,
        }

        private unsafe void DrawLine(VBGraphics g, DrawRange drawRange, string text) {
            Point pt = drawRange.Location;
            TextDrawMode? prevMode = null;
            var sb = new StringBuilder(drawRange.Length * 2);
            for (int pos = drawRange.Start; pos <= drawRange.End; pos++) {
                char c = text[pos];
                TextDrawMode mode = GetDrawMode(pos, selectionRange);
                if (prevMode.HasValue) {
                    if (mode != prevMode) {
                        DrawPart(g, prevMode.Value, pt, sb);
                        sb.Clear();
                        pt = GetPositionFromCharIndex(pos).Value;
                    }
                }
                sb.Append(c);
                prevMode = mode;
            }
            if (sb.Length > 0) {
                DrawPart(g, prevMode.Value, pt, sb);
            }
        }

        private void DrawPart(VBGraphics g, TextDrawMode mode, Point pt, StringBuilder sb) {
            switch (mode) {
                case TextDrawMode.Normal:
                    g.SetColor(ForeColor, BackColor);
                    break;
                case TextDrawMode.Highlight:
                    g.SetColor(SystemColors.HighlightText, SystemColors.Highlight);
                    break;
                case TextDrawMode.Disable:
                    g.SetColor(SystemColors.GrayText, BackColor);
                    break;
            }
            if (MultiLine) {
                g.TextOut(pt.X, pt.Y, sb);
            } else {
                g.TextOut(pt.X, SingleTop, sb);
            }
        }

        private TextDrawMode GetDrawMode(int pos, SelectionRange selectionRange) {
            if (!Enabled) {
                return TextDrawMode.Disable;
            }
            if ((!HideSelection || Focused) && selectionRange.Contains(pos)) {
                return TextDrawMode.Highlight;
            }
            return TextDrawMode.Normal;
        }

        public int GetLineCount() {
            return SendMessageInt(NativeMethods.EM_GETLINECOUNT);
        }

        public int GetFirstVisibleLine() {
            return SendMessageInt(NativeMethods.EM_GETFIRSTVISIBLELINE);
        }

        public Point? GetPositionFromCharIndex(int charIndex) {
            IntPtr pt = NativeMethods.SendMessage(hwnd,
                                NativeMethods.EM_POSFROMCHAR, new IntPtr(charIndex), IntPtr.Zero);
            if (pt == NativeMethods.INVALID_HANDLE_VALUE) {
                return null;
            }
            return new Point(NativeMethods.SignedLOWORD(pt), NativeMethods.SignedHIWORD(pt));
        }

        public int GetFirstCharIndexFromLine(int lineNumber) {
            return SendMessageInt(NativeMethods.EM_LINEINDEX, lineNumber);
        }

        public virtual int GetLineLength(int lineIndex) {
            int charIndex = GetFirstCharIndexFromLine(lineIndex);
            return SendMessageInt(NativeMethods.EM_LINELENGTH, charIndex);
        }

        protected int SendMessageInt(int msg) {
            return SendMessageInt(msg, 0);
        }

        protected int SendMessageInt(int msg, int wParam) {
            return (int)NativeMethods.SendMessage(hwnd, msg, new IntPtr(wParam), IntPtr.Zero);
        }

        private class Range
        {
            protected Range() { }

            public Range(int start, int end) {
                this.Start = start;
                this.End = end;
            }

            public int Start;
            public int End;
            public int Length {
                get {
                    return End - Start + 1;
                }
            }

            public bool Contains(int pos) {
                return (pos >= Start && pos < End);
            }
        }

        private class DrawRange : Range
        {
            public DrawRange(Point pt, int start, int end)
                : base(start, end) {
                Location = pt;
            }
            public Point Location { get; }
        }

        private class SelectionRange : Range
        {
            public SelectionRange(IntPtr hwnd) {
                NativeMethods.SendMessage(
                                 hwnd,
                                 NativeMethods.EM_GETSEL,
                                 out Start, out End);
            }
        }


    }
}
