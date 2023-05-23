namespace VBCompatible.VB6
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;

    [StandardModule]
    public static class Support
    {
        internal static double m_TwipsPerPixelX;
        internal static double m_TwipsPerPixelY;
        internal static bool m_IsTwipsPerPixelSetUp;
        internal const int TwipsPerInch = 1440;
        internal const int TwipsPerPoint = 20;
        internal const int HimetricPerMM = 100;
        internal const double MMPerInch = 25.4;
        internal const double CMPerInch = 2.54;
        internal const double HiMetricPerInch = 2540;
        internal const double TwipsPerMM = 56.6929133858268;
        internal const double TwipsPerCM = 566.929133858268;
        internal const double TwipsPerHiMetric = 0.566929133858268;
        internal const int TwipsPerCharHoriz = 120;
        internal const int TwipsPerCharVert = 240;

        /// <summary>
        /// 式を指定した書式に変換し、その文字列の値を返します。
        /// </summary>
        /// <param name="Expression">
        /// 必ず指定します。任意の式を指定します。引数 expression に指定したデータは、引数 format の書式に従って変換されます。
        /// </param>
        /// <param name="Style">省略可能です。定義済み書式または表示書式指定文字を指定します。</param>
        /// <param name="DayOfWeek">省略可能です。週の 1 日目を指定する定数を指定します。</param>
        /// <param name="WeekOfYear">省略可能です。年の第 1 週を指定する定数を指定します。</param>
        /// <returns>変換された文字列。</returns>
        public static string Format(object Expression,
                                string Style = "",
                                FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday,
                                FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1) {
            if (Expression is long) {
                Expression = new decimal(Convert.ToInt64(Expression));
            } else if (Expression is char) {
                Expression = Expression.ToString();
            }
            int nResult;
            string str;
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(NativeMethods.VARIANT)));
            try {
                NativeMethods.VariantInit(ptr);
                try {
                    Marshal.GetNativeVariantForObject(Expression, ptr);
                    int dwFlags = (Thread.CurrentThread.CurrentCulture.Calendar is HijriCalendar)
                                    ? NativeMethods.VAR_CALENDAR_HIJRI : NativeMethods.VAR_FORMAT_NOSUBSTITUTE;
                    nResult = NativeMethods.VarFormat(ptr, ref Style, (int)DayOfWeek, (int)WeekOfYear, dwFlags, out str);
                } finally {
                    NativeMethods.VariantClear(ptr);
                }
            } finally {
                Marshal.FreeCoTaskMem(ptr);
            }
            if (nResult < 0) {
                throw new ArgumentException();
            }
            return str;
        }

        public static Font FontChangeBold(Font CurrentFont, bool Bold) {
            return FontChangeStyle(CurrentFont, FontStyle.Bold, Bold);
        }

        public static Font FontChangeGdiCharSet(Font CurrentFont, byte GdiCharSet) {
            return new Font(CurrentFont.FontFamily, CurrentFont.SizeInPoints, CurrentFont.Style,
                GraphicsUnit.Point, GdiCharSet, CurrentFont.GdiVerticalFont);
        }

        public static Font FontChangeItalic(Font CurrentFont, bool Italic) {
            return FontChangeStyle(CurrentFont, FontStyle.Italic, Italic);
        }

        public static Font FontChangeName(Font CurrentFont, string Name) {
            return new Font(Name, CurrentFont.SizeInPoints, CurrentFont.Style, 
                GraphicsUnit.Point, CurrentFont.GdiCharSet, CurrentFont.GdiVerticalFont);
        }

        public static Font FontChangeSize(Font CurrentFont, float Size) {
            if (Math.Round(CurrentFont.SizeInPoints, 2) == Math.Round(Size, 2)) {
                return CurrentFont;
            }
            return new Font(CurrentFont.Name, Size, CurrentFont.Style, GraphicsUnit.Point, CurrentFont.GdiCharSet);
        }

        public static Font FontChangeStrikeout(Font CurrentFont, bool Strikeout) {
            return FontChangeStyle(CurrentFont, FontStyle.Strikeout, Strikeout);
        }

        private static Font FontChangeStyle(Font CurrentFont, FontStyle StyleBit, bool NewValue) {
            if ((CurrentFont.Style & StyleBit) != FontStyle.Regular == NewValue) {
                return CurrentFont;
            }
            FontStyle style = CurrentFont.Style & ~StyleBit;
            if (NewValue) {
                style |= StyleBit;
            }
            return new Font(CurrentFont, style);
        }

        public static Font FontChangeUnderline(Font CurrentFont, bool Underline) {
            return FontChangeStyle(CurrentFont, FontStyle.Underline, Underline);
        }

        public static double FromPixelsUserHeight(double Height, double ScaleHeight, int OriginalHeightInPixels) {
            return Height * ScaleHeight / OriginalHeightInPixels;
        }

        public static double FromPixelsUserWidth(double Width, double ScaleWidth, int OriginalWidthInPixels) {
            return Width * ScaleWidth / OriginalWidthInPixels;
        }

        public static double FromPixelsUserX(double X, double ScaleLeft, double ScaleWidth, int OriginalWidthInPixels) {
            return X * ScaleWidth / OriginalWidthInPixels + ScaleLeft;
        }

        public static double FromPixelsUserY(double Y, double ScaleTop, double ScaleHeight, int OriginalHeightInPixels) {
            return Y * ScaleHeight / OriginalHeightInPixels + ScaleTop;
        }

        public static double FromPixelsX(double X, ScaleMode ToScale) {
            switch (ToScale) {
                case ScaleMode.Points: {
                        return PixelsToTwipsX(X) / TwipsPerPoint;
                    }
                case ScaleMode.Characters: {
                        return PixelsToTwipsX(X) / TwipsPerCharHoriz;
                    }
                case ScaleMode.Inches: {
                        return PixelsToTwipsX(X) / TwipsPerInch;
                    }
                case ScaleMode.Millimeters: {
                        return PixelsToTwipsX(X) / TwipsPerMM;
                    }
                case ScaleMode.Centimeters: {
                        return PixelsToTwipsX(X) / TwipsPerCM;
                    }
                case ScaleMode.Himetric: {
                        return PixelsToTwipsX(X) / TwipsPerHiMetric;
                    }
                default: {
                        throw new ArgumentOutOfRangeException(nameof(ToScale));
                    }
            }
        }

        public static double FromPixelsY(double Y, ScaleMode ToScale) {
            switch (ToScale) {
                case ScaleMode.Points: {
                        return PixelsToTwipsY(Y) / TwipsPerPoint;
                    }
                case ScaleMode.Characters: {
                        return PixelsToTwipsY(Y) / TwipsPerCharVert;
                    }
                case ScaleMode.Inches: {
                        return PixelsToTwipsY(Y) / TwipsPerInch;
                    }
                case ScaleMode.Millimeters: {
                        return PixelsToTwipsY(Y) / TwipsPerMM;
                    }
                case ScaleMode.Centimeters: {
                        return PixelsToTwipsY(Y) / TwipsPerCM;
                    }
                case ScaleMode.Himetric: {
                        return PixelsToTwipsY(Y) / TwipsPerHiMetric;
                    }
                default: {
                        throw new ArgumentOutOfRangeException(nameof(ToScale));
                    }
            }
        }

        public static Control GetActiveControl() {
            Form activeForm = Form.ActiveForm;
            if (activeForm == null) {
                return null;
            }
            return activeForm.ActiveControl;
        }

        public static bool GetCancel(Button btn) {
            return btn.FindForm().CancelButton == btn;
        }

        public static bool GetDefault(Button btn) {
            return btn.FindForm().AcceptButton == btn;
        }

        public static string GetEXEName() {
            return Path.GetFileNameWithoutExtension(Assembly.GetCallingAssembly().Location);
        }

        public static IntPtr GetHInstance() {
            Module modules = Assembly.GetCallingAssembly().GetModules()[0];
            return Marshal.GetHINSTANCE(modules);
        }

        public static string GetPath() {
            string directoryName = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            return directoryName.Replace('/', '\\');
        }

        public static double PixelsToTwipsX(double X) {
            SetUpTwipsPerPixel(false);
            return X * m_TwipsPerPixelX;
        }

        public static double PixelsToTwipsY(double Y) {
            SetUpTwipsPerPixel(false);
            return Y * m_TwipsPerPixelY;
        }

        public static void SendKeys(string Keys, bool Wait = false) {
            if (!Wait) {
                System.Windows.Forms.SendKeys.Send(Keys);
            } else {
                System.Windows.Forms.SendKeys.SendWait(Keys);
            }
        }

        public static void SetCancel(Button btn, bool Cancel) {
            if (Cancel) {
                btn.FindForm().CancelButton = btn;
            } else if (btn.FindForm().CancelButton == btn) {
                btn.FindForm().CancelButton = null;
            }
        }

        public static void SetDefault(Button btn, bool Default) {
            if (Default) {
                btn.FindForm().AcceptButton = btn;
            } else if (btn.FindForm().AcceptButton == btn) {
                btn.FindForm().AcceptButton = null;
            }
        }

        private static void SetUpTwipsPerPixel(bool Force = false) {
            if (!m_IsTwipsPerPixelSetUp || Force) {
                m_TwipsPerPixelX = 0;
                m_TwipsPerPixelY = 0;
                IntPtr dC = NativeMethods.GetDC(IntPtr.Zero);
                if (dC != IntPtr.Zero) {
                    try {
                        m_TwipsPerPixelX = 1440 / (double)NativeMethods.GetDeviceCaps(dC, NativeMethods.LOGPIXELSX);
                        m_TwipsPerPixelY = 1440 / (double)NativeMethods.GetDeviceCaps(dC, NativeMethods.LOGPIXELSY);
                    } finally {
                        NativeMethods.ReleaseDC(IntPtr.Zero, dC);
                    }
                }
                m_IsTwipsPerPixelSetUp = true;
                if (m_TwipsPerPixelX == 0 || m_TwipsPerPixelY == 0) {
                    m_TwipsPerPixelX = 15;
                    m_TwipsPerPixelY = 15;
                }
            }
        }

        public static double ToPixelsUserHeight(double Height, double ScaleHeight, int OriginalHeightInPixels) {
            return Height / ScaleHeight * OriginalHeightInPixels;
        }

        public static double ToPixelsUserWidth(double Width, double ScaleWidth, int OriginalWidthInPixels) {
            return Width / ScaleWidth * (double)OriginalWidthInPixels;
        }

        public static double ToPixelsUserX(double X, double ScaleLeft, double ScaleWidth, int OriginalWidthInPixels) {
            return (X - ScaleLeft) / ScaleWidth * (double)OriginalWidthInPixels;
        }

        public static double ToPixelsUserY(double Y, double ScaleTop, double ScaleHeight, int OriginalHeightInPixels) {
            return (Y - ScaleTop) / ScaleHeight * (double)OriginalHeightInPixels;
        }

        public static double ToPixelsX(double X, ScaleMode FromScale) {
            switch (FromScale) {
                case ScaleMode.Points: {
                        return TwipsToPixelsX(X * TwipsPerPoint);
                    }
                case ScaleMode.Characters: {
                        return TwipsToPixelsX(X * TwipsPerCharHoriz);
                    }
                case ScaleMode.Inches: {
                        return TwipsToPixelsX(X * TwipsPerInch);
                    }
                case ScaleMode.Millimeters: {
                        return TwipsToPixelsX(X * TwipsPerMM);
                    }
                case ScaleMode.Centimeters: {
                        return TwipsToPixelsX(X * TwipsPerCM);
                    }
                case ScaleMode.Himetric: {
                        return TwipsToPixelsX(X * TwipsPerHiMetric);
                    }
                default: {
                        throw new ArgumentOutOfRangeException(nameof(FromScale));
                    }
            }
        }

        public static double ToPixelsY(double Y, ScaleMode FromScale) {
            switch (FromScale) {
                case ScaleMode.Points: {
                        return TwipsToPixelsY(Y * TwipsPerPoint);
                    }
                case ScaleMode.Characters: {
                        return TwipsToPixelsY(Y * TwipsPerCharVert);
                    }
                case ScaleMode.Inches: {
                        return TwipsToPixelsY(Y * TwipsPerInch);
                    }
                case ScaleMode.Millimeters: {
                        return TwipsToPixelsY(Y * TwipsPerMM);
                    }
                case ScaleMode.Centimeters: {
                        return TwipsToPixelsY(Y * TwipsPerCM);
                    }
                case ScaleMode.Himetric: {
                        return TwipsToPixelsY(Y * TwipsPerHiMetric);
                    }
                default: {
                        throw new ArgumentOutOfRangeException(nameof(FromScale));
                    }
            }
        }

        public static float TwipsPerPixelX() {
            SetUpTwipsPerPixel(false);
            return (float)m_TwipsPerPixelX;
        }

        public static float TwipsPerPixelY() {
            SetUpTwipsPerPixel(false);
            return (float)m_TwipsPerPixelY;
        }

        public static double TwipsToPixelsX(double X) {
            SetUpTwipsPerPixel(false);
            return X / m_TwipsPerPixelX;
        }

        public static double TwipsToPixelsY(double Y) {
            SetUpTwipsPerPixel(false);
            return Y / m_TwipsPerPixelY;
        }

        public static void WhatsThisMode(Form Form) {
            IntPtr intPtr = new IntPtr(NativeMethods.SC_CONTEXTHELP);
            if (Form.Visible) {
                NativeMethods.PostMessage(Form.Handle, NativeMethods.WM_SYSCOMMAND, intPtr, IntPtr.Zero);
            }
        }
    }
}
