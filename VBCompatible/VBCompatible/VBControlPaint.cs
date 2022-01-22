using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace VBCompatible
{
    /// <summary>
    /// 共通の Windows コントロールとその要素を描画するために使用するメソッドを提供します。 このクラスは継承できません。
    /// </summary>
    public static class VBControlPaint
    {
        private static readonly ContentAlignment anyTop = ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight;
        private static readonly ContentAlignment anyRight = ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight;
        private static readonly ContentAlignment anyCenter = ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter;
        private static readonly ContentAlignment anyBottom = ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight;
        private static readonly ContentAlignment anyMiddle = ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight;

        [ThreadStatic]
        private static ImageAttributes disabledImageAttr;

        /// <summary>
        /// Label を描画します。
        /// </summary>
        /// <param name="target">ターゲットとなるラベル</param>
        /// <param name="g">描画する Graphics オブジェクト</param>
        /// <param name="enabled">有効な状態のラベルを描画するかどうかを指定します。</param>
        public static void DrawLabel(Label target, Graphics g, bool enabled) {
            Rectangle face = DeflateRect(target.ClientRectangle, target.Padding);
            Color color = enabled ? target.ForeColor : SystemColors.ControlDark;
            TextFormatFlags flags = CreateTextFormatFlags(target) | TextFormatFlags.NoPadding;
            TextRenderer.DrawText(g, target.Text, target.Font, face, color, flags);
        }

        /// <summary>
        /// Rectangle 構造体を Padding 構造体の分だけ縮小します。
        /// </summary>
        /// <param name="rect">対象となる Rectangle 構造体</param>
        /// <param name="padding">対象となる Padding 構造体</param>
        /// <returns>縮小後の Rectangle 構造体</returns>
        public static Rectangle DeflateRect(Rectangle rect, Padding padding) {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }

        /// <summary>
        /// Rectangle 構造体を Padding 構造体の分だけ拡大します。
        /// </summary>
        /// <param name="rect">対象となる Rectangle 構造体</param>
        /// <param name="padding">対象となる Padding 構造体</param>
        /// <returns>縮小後の Rectangle 構造体</returns>
        public static Rectangle InflateRect(Rectangle rect, Padding padding) {
            rect.X -= padding.Left;
            rect.Y -= padding.Top;
            rect.Width += padding.Horizontal;
            rect.Height += padding.Vertical;
            return rect;
        }
        /// <summary>
        /// BackColor から Disable 状態のテキストカラーを作成します。
        /// </summary>
        /// <param name="backColor">BackColor</param>
        /// <returns>Disable 状態のテキストカラー</returns>
        public static Color DisabledTextColor(Color backColor) {
            Color disabledTextForeColor = SystemColors.ControlDark;
            if (IsDarker(backColor, SystemColors.Control)) {
                disabledTextForeColor = ControlPaint.Dark(backColor);
            }
            return disabledTextForeColor;
        }

        internal static bool IsDarker(Color c1, Color c2) {
            HLSColor hc1 = new HLSColor(c1);
            HLSColor hc2 = new HLSColor(c2);
            return (hc1.Luminosity < hc2.Luminosity);
        }

        /// <summary>
        /// Control より StringFormat を作成します。
        /// </summary>
        /// <param name="ctl">対象コントロール</param>
        /// <param name="textAlign">対象コントロールの TextAlign プロパティ</param>
        /// <param name="showEllipsis">対象コントロールの ShowEllipsis プロパティ</param>
        /// <param name="useMnemonic">対象コントロールの UseMnemonic プロパティ</param>
        /// <returns>StringFormat</returns>
        public static StringFormat CreateStringFormat(Control ctl, ContentAlignment textAlign, bool showEllipsis, bool useMnemonic) {

            StringFormat stringFormat = StringFormatForAlignment(textAlign);

            if (ctl.RightToLeft == RightToLeft.Yes) {
                stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }

            if (showEllipsis) {
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                stringFormat.FormatFlags |= StringFormatFlags.LineLimit;
            }
            if (!useMnemonic) {
                stringFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            }
            if (ctl.AutoSize) {
                stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            }

            return stringFormat;
        }

        /// <summary>
        /// ContentAlignment から StringFormat を作成します。
        /// </summary>
        /// <param name="align">ContentAlignment</param>
        /// <returns>StringFormat</returns>
        public static StringFormat StringFormatForAlignment(ContentAlignment align) {
            StringFormat output = new StringFormat();
            output.Alignment = TranslateAlignment(align);
            output.LineAlignment = TranslateLineAlignment(align);
            return output;
        }

        /// <summary>
        /// ContentAlignment から TextFormatFlags を作成します。
        /// </summary>
        /// <param name="align">ContentAlignment</param>
        /// <returns>TextFormatFlags</returns>
        public static TextFormatFlags TextFormatFlagsForAlignmentGDI(ContentAlignment align) {
            TextFormatFlags output = new TextFormatFlags();
            output |= TranslateAlignmentForGDI(align);
            output |= TranslateLineAlignmentForGDI(align);
            return output;
        }

        /// <summary>
        /// ContentAlignment から StringAlignment を作成します。
        /// </summary>
        /// <param name="align">ContentAlignment</param>
        /// <returns>StringAlignment</returns>
        public static StringAlignment TranslateAlignment(ContentAlignment align) {
            StringAlignment result;
            if ((align & anyRight) != 0)
                result = StringAlignment.Far;
            else if ((align & anyCenter) != 0)
                result = StringAlignment.Center;
            else
                result = StringAlignment.Near;
            return result;
        }

        /// <summary>
        /// ContentAlignment を TextFormatFlags に変換します。(GDI)
        /// </summary>
        /// <param name="align">ContentAlignment</param>
        /// <returns>TextFormatFlags</returns>
        public static TextFormatFlags TranslateAlignmentForGDI(ContentAlignment align) {
            TextFormatFlags result;
            if ((align & anyBottom) != 0)
                result = TextFormatFlags.Bottom;
            else if ((align & anyMiddle) != 0)
                result = TextFormatFlags.VerticalCenter;
            else
                result = TextFormatFlags.Top;
            return result;
        }

        /// <summary>
        /// ContentAlignment を StringAlignment に変換します。
        /// </summary>
        /// <param name="align">ContentAlignment</param>
        /// <returns>StringAlignment</returns>
        public static StringAlignment TranslateLineAlignment(ContentAlignment align) {
            StringAlignment result;
            if ((align & anyBottom) != 0) {
                result = StringAlignment.Far;
            } else if ((align & anyMiddle) != 0) {
                result = StringAlignment.Center;
            } else {
                result = StringAlignment.Near;
            }
            return result;
        }

        /// <summary>
        /// ContentAlignment を TextFormatFlags に変換します。(GDI)
        /// </summary>
        /// <param name="align">ContentAlignment</param>
        /// <returns>TextFormatFlags</returns>
        public static TextFormatFlags TranslateLineAlignmentForGDI(ContentAlignment align) {
            TextFormatFlags result;
            if ((align & anyRight) != 0)
                result = TextFormatFlags.Right;
            else if ((align & anyCenter) != 0)
                result = TextFormatFlags.HorizontalCenter;
            else
                result = TextFormatFlags.Left;
            return result;
        }

        /// <summary>
        /// 指定したコントロールから TextFormatFlags を作成します。
        /// </summary>
        /// <param name="ctl">対象コントロール</param>
        /// <param name="textAlign">対象コントロールの TextAlign プロパティ</param>
        /// <returns>TextFormatFlags</returns>
        public static TextFormatFlags CreateTextFormatFlags(Control ctl, ContentAlignment textAlign) {
            return CreateTextFormatFlags(ctl, textAlign, false);
        }
        /// <summary>
        /// 指定したコントロールから TextFormatFlags を作成します。
        /// </summary>
        /// <param name="ctl">対象コントロール</param>
        /// <param name="textAlign">対象コントロールの TextAlign プロパティ</param>
        /// <param name="showEllipsis">対象コントロールの ShowEllipsis プロパティ</param>
        /// <returns>TextFormatFlags</returns>
        public static TextFormatFlags CreateTextFormatFlags(Control ctl, ContentAlignment textAlign, bool showEllipsis) {
            return CreateTextFormatFlags(ctl, textAlign, showEllipsis, true);
        }
        /// <summary>
        /// 指定したコントロールから TextFormatFlags を作成します。
        /// </summary>
        /// <param name="ctl">対象コントロール</param>
        /// <param name="textAlign">対象コントロールの TextAlign プロパティ</param>
        /// <param name="showEllipsis">対象コントロールの ShowEllipsis プロパティ</param>
        /// <param name="useMnemonic">対象コントロールの UseMnemonic プロパティ</param>
        /// <returns>TextFormatFlags</returns>
        public static TextFormatFlags CreateTextFormatFlags(Control ctl, ContentAlignment textAlign, bool showEllipsis, bool useMnemonic) {

            textAlign = RtlTranslateContent(ctl, textAlign);
            TextFormatFlags flags = TextFormatFlagsForAlignmentGDI(textAlign);
            flags |= TextFormatFlags.NoPadding;

            if (showEllipsis) {
                flags |= TextFormatFlags.EndEllipsis;
            } else {
                flags |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
            }

            if (ctl.RightToLeft == RightToLeft.Yes) {
                flags |= TextFormatFlags.RightToLeft;
            }
            if (!useMnemonic) {
                flags |= TextFormatFlags.NoPrefix;
            }
            return flags;
        }

        private static ContentAlignment RtlTranslateContent(Control target, ContentAlignment align) {
            if (RightToLeft.Yes == target.RightToLeft) {

                if ((align & anyTop) != 0) {
                    switch (align) {
                        case ContentAlignment.TopLeft:
                            return ContentAlignment.TopRight;
                        case ContentAlignment.TopRight:
                            return ContentAlignment.TopLeft;
                    }
                }
                if ((align & anyMiddle) != 0) {
                    switch (align) {
                        case ContentAlignment.MiddleLeft:
                            return ContentAlignment.MiddleRight;
                        case ContentAlignment.MiddleRight:
                            return ContentAlignment.MiddleLeft;
                    }
                }

                if ((align & anyBottom) != 0) {
                    switch (align) {
                        case ContentAlignment.BottomLeft:
                            return ContentAlignment.BottomRight;
                        case ContentAlignment.BottomRight:
                            return ContentAlignment.BottomLeft;
                    }
                }
            }
            return align;
        }

        internal static TextFormatFlags CreateTextFormatFlags(Label target) {
            TextFormatFlags flags = CreateTextFormatFlags(target, target.TextAlign, target.AutoEllipsis, target.UseMnemonic);
            return flags;
        }

        private static TextFormatFlags CreateTextFormatFlags(TextBox target) {
            ContentAlignment textAlign;
            switch (target.TextAlign) {
                case HorizontalAlignment.Center:
                    textAlign = ContentAlignment.MiddleCenter;
                    break;

                case HorizontalAlignment.Right:
                    textAlign = ContentAlignment.MiddleRight;
                    break;

                default:
                    textAlign = ContentAlignment.MiddleLeft;
                    break;
            }
            return CreateTextFormatFlags(target, textAlign);
        }

        /// <summary>
        /// コントロールの枠線を描画します。
        /// </summary>
        /// <param name="target">対象コントロール</param>
        /// <param name="borderStyle">対象コントロールの BorderStyle </param>
        public static void DrawBorder(Control target, BorderStyle borderStyle) {
            if (borderStyle == BorderStyle.None) return;

            IntPtr hDC = NativeMethods.GetWindowDC(target.Handle);
            if (hDC != IntPtr.Zero) {
                using (Graphics g = Graphics.FromHdc(hDC)) {
                    Rectangle bounds = new Rectangle(Point.Empty, target.Size);
                    DrawBorder(g, bounds, borderStyle);
                }
                NativeMethods.ReleaseDC(target.Handle, hDC);
            }
        }

        /// <summary>
        /// 枠線を描画します。
        /// </summary>
        /// <param name="g">対象の Graphics オブジェクト</param>
        /// <param name="bounds">描画する範囲</param>
        /// <param name="borderStyle">対象コントロールの BorderStyle </param>
        public static void DrawBorder(Graphics g, Rectangle bounds, BorderStyle borderStyle) {
            switch (borderStyle) {
                case BorderStyle.FixedSingle:
                    ControlPaint.DrawBorder(g, bounds, SystemColors.WindowFrame, ButtonBorderStyle.Solid);
                    break;
                case BorderStyle.Fixed3D:
                    if (Application.RenderWithVisualStyles)
                        ControlPaint.DrawBorder(g, bounds, SystemColors.ControlDark, ButtonBorderStyle.Solid);
                    else
                        ControlPaint.DrawBorder3D(g, bounds, Border3DStyle.SunkenOuter);
                    break;
            }
        }

        internal static void DrawComboButton(Control target) {
            IntPtr hDC = NativeMethods.GetWindowDC(target.Handle);
            int vWidth = SystemInformation.VerticalScrollBarWidth;
            if (hDC != IntPtr.Zero) {
                using (Graphics g = Graphics.FromHdc(hDC)) {
                    Rectangle face = target.ClientRectangle;
                    face.X = face.Width - vWidth;
                    face.Width = vWidth;
                    if (ComboBoxRenderer.IsSupported)
                        ComboBoxRenderer.DrawDropDownButton(g, face, System.Windows.Forms.VisualStyles.ComboBoxState.Normal);
                    else
                        ControlPaint.DrawComboButton(g, face, ButtonState.Inactive);
                }
                NativeMethods.ReleaseDC(target.Handle, hDC);
            }
        }

        internal static StringAlignment ToVertical(ContentAlignment textAlign) {
            switch (textAlign) {
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomRight:
                    return StringAlignment.Far;
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    return StringAlignment.Center;
                default:
                    return StringAlignment.Near;
            }
        }

        internal static StringAlignment ToHorizontal(ContentAlignment textAlign) {
            switch (textAlign) {
                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    return StringAlignment.Far;
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    return StringAlignment.Center;
                default:
                    return StringAlignment.Near;
            }
        }
        internal static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect) {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, Point.Empty, RightToLeft.No);
        }
        internal static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset) {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, scrollOffset, RightToLeft.No);
        }

        internal static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage, ImageLayout imageLayout) {

            Rectangle result = bounds;

            if (backgroundImage != null) {
                switch (imageLayout) {
                    case ImageLayout.Stretch:
                        result.Size = bounds.Size;
                        break;

                    case ImageLayout.None:
                        result.Size = backgroundImage.Size;
                        break;

                    case ImageLayout.Center:
                        result.Size = backgroundImage.Size;
                        Size szCtl = bounds.Size;

                        if (szCtl.Width > result.Width) {
                            result.X = (szCtl.Width - result.Width) / 2;
                        }
                        if (szCtl.Height > result.Height) {
                            result.Y = (szCtl.Height - result.Height) / 2;
                        }
                        break;

                    case ImageLayout.Zoom:
                        Size imageSize = backgroundImage.Size;
                        float xRatio = (float)bounds.Width / (float)imageSize.Width;
                        float yRatio = (float)bounds.Height / (float)imageSize.Height;
                        if (xRatio < yRatio) {
                            result.Width = bounds.Width;
                            result.Height = (int)((imageSize.Height * xRatio) + .5);
                            if (bounds.Y >= 0) {
                                result.Y = (bounds.Height - result.Height) / 2;
                            }
                        } else {
                            result.Height = bounds.Height;
                            result.Width = (int)((imageSize.Width * yRatio) + .5);
                            if (bounds.X >= 0) {
                                result.X = (bounds.Width - result.Width) / 2;
                            }
                        }

                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 背景を描画します。
        /// </summary>
        /// <param name="g">対象の Graphics オブジェクト</param>
        /// <param name="backgroundImage">背景に描画する Image</param>
        /// <param name="backColor">背景色</param>
        /// <param name="backgroundImageLayout">イメージの位置</param>
        /// <param name="bounds">描画する範囲</param>
        /// <param name="clipRect">クリップする範囲</param>
        /// <param name="scrollOffset">スクロールオフセット</param>
        /// <param name="rightToLeft">RightToLeft プロパティ</param>
        public static void DrawBackgroundImage(
                                    Graphics g,
                                    Image backgroundImage,
                                    Color backColor,
                                    ImageLayout backgroundImageLayout,
                                    Rectangle bounds,
                                    Rectangle clipRect,
                                    Point scrollOffset,
                                    RightToLeft rightToLeft) {
            if (g == null) {
                throw new ArgumentNullException("g");
            }

            if (backgroundImageLayout == ImageLayout.Tile) {

                using (TextureBrush textureBrush = new TextureBrush(backgroundImage, WrapMode.Tile)) {
                    if (scrollOffset != Point.Empty) {
                        Matrix transform = textureBrush.Transform;
                        transform.Translate(scrollOffset.X, scrollOffset.Y);
                        textureBrush.Transform = transform;
                    }
                    g.FillRectangle(textureBrush, clipRect);
                }
            } else {

                Rectangle imageRectangle = CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);

                if (rightToLeft == RightToLeft.Yes && backgroundImageLayout == ImageLayout.None) {
                    imageRectangle.X += clipRect.Width - imageRectangle.Width;
                }

                SolidBrush brush = VBGraphicsCache.GetSolidBrush(backColor);
                g.FillRectangle(brush, clipRect);

                if (!clipRect.Contains(imageRectangle)) {
                    if (backgroundImageLayout == ImageLayout.Stretch || backgroundImageLayout == ImageLayout.Zoom) {
                        imageRectangle.Intersect(clipRect);
                        g.DrawImage(backgroundImage, imageRectangle);
                    } else if (backgroundImageLayout == ImageLayout.None) {
                        imageRectangle.Offset(clipRect.Location);
                        Rectangle imageRect = imageRectangle;
                        imageRect.Intersect(clipRect);
                        Rectangle partOfImageToDraw = new Rectangle(Point.Empty, imageRect.Size);
                        g.DrawImage(backgroundImage, imageRect, partOfImageToDraw.X, partOfImageToDraw.Y, partOfImageToDraw.Width,
                            partOfImageToDraw.Height, GraphicsUnit.Pixel);
                    } else {
                        Rectangle imageRect = imageRectangle;
                        imageRect.Intersect(clipRect);
                        Rectangle partOfImageToDraw = new Rectangle(new Point(imageRect.X - imageRectangle.X, imageRect.Y - imageRectangle.Y)
                                    , imageRect.Size);

                        g.DrawImage(backgroundImage, imageRect, partOfImageToDraw.X, partOfImageToDraw.Y, partOfImageToDraw.Width,
                            partOfImageToDraw.Height, GraphicsUnit.Pixel);
                    }
                } else {
                    ImageAttributes imageAttrib = new ImageAttributes();
                    imageAttrib.SetWrapMode(WrapMode.TileFlipXY);
                    g.DrawImage(backgroundImage, imageRectangle, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttrib);
                    imageAttrib.Dispose();

                }
            }
        }

        /// <summary>
        /// イメージを描画する範囲を取得
        /// </summary>
        /// <param name="target">対象のコントロール</param>
        /// <param name="img">描画イメージ</param>
        /// <param name="imgAlign">イメージを描画する位置を示す ContentAlignment  </param>
        /// <returns>イメージを描画する範囲を示す Rectangle</returns>
        public static Rectangle GetImageRect(Control target, Image img, ContentAlignment imgAlign) {
            int X, Y;
            Rectangle face = DeflateRect(target.ClientRectangle, target.Padding);
            Size imgSize = img.Size;

            // X
            if ((imgAlign & anyRight) != 0)
                X = face.Right - imgSize.Width;
            else if ((imgAlign & anyCenter) != 0)
                X = face.Left + (face.Width - imgSize.Width) / 2;
            else
                X = face.Left;

            // Y
            if ((imgAlign & anyBottom) != 0)
                Y = face.Bottom - imgSize.Height;
            else if ((imgAlign & anyMiddle) != 0)
                Y = face.Top + (face.Height - imgSize.Height) / 2;
            else
                Y = face.Top;

            return new Rectangle(new Point(X, Y), imgSize);
        }

        /// <summary>
        /// VBOptionButton , VBCheckBox のテキスト描画範囲を取得します。
        /// </summary>
        /// <param name="control">ターゲットコントロール</param>
        /// <param name="align">コンテンツの配置方法</param>
        /// <param name="glyphSize">グリフサイズ</param>
        /// <param name="crevice">幅を調整するサイズ</param>
        /// <returns>テキスト描画範囲を占める Rectangle 構造体。</returns>
        public static Rectangle GlyphTextFace(Control control, ContentAlignment align, Size glyphSize, int crevice) {
            Rectangle face = VBControlPaint.DeflateRect(control.ClientRectangle, control.Padding);
            if ((align & anyRight) != 0) {
            } else if ((align & anyCenter) != 0) {
            } else {
                face.X += glyphSize.Width + crevice;
                face.Width -= glyphSize.Width + crevice;
            }
            return face;
        }

        /// <summary>
        /// VBOptionButton , VBCheckBox のグリフ描画位置を取得します。
        /// </summary>
        /// <param name="control">ターゲットコントロール</param>
        /// <param name="align">コンテンツの配置方法</param>
        /// <param name="glyphSize">グリフサイズ</param>
        /// <returns>グリフ描画位置を示す Pint 構造体。</returns>
        public static Point GlyphLocation(Control control, ContentAlignment align, Size glyphSize) {
            Rectangle face = VBControlPaint.DeflateRect(control.ClientRectangle, control.Padding);
            Point p = new Point(0, 0);

            if ((align & anyRight) != 0) {
                p.X = face.Right - glyphSize.Width - 1;
            } else if ((align & anyCenter) != 0) {
                p.X = face.X + (face.Width - glyphSize.Width) / 2;
            } else {
                p.X = face.X;
            }

            if ((align & anyBottom) != 0) {
                p.Y = face.Bottom - glyphSize.Height;
            } else if ((align & anyMiddle) != 0) {
                p.Y = face.Y + (face.Height - glyphSize.Height) / 2;
            } else {
                p.Y = face.Y;
            }

            return p;
        }

        /// <summary>
        /// FlatStyle の枠を描画します。
        /// </summary>
        /// <param name="target">対象となるコントロール</param>
        /// <param name="flatBorderColor">枠の色</param>
        /// <param name="flatBorderSize">枠線の幅</param>
        public static void DrawFlatBorder(Control target, Color flatBorderColor, FlatBorderSize flatBorderSize) {
            IntPtr hDC = NativeMethods.GetWindowDC(target.Handle);
            if (hDC != IntPtr.Zero) {
                using (Graphics g = Graphics.FromHdc(hDC)) {
                    Rectangle face = new Rectangle(new Point(0, 0), target.Size);
                    if (flatBorderSize == FlatBorderSize.None) {
                        flatBorderColor = target.BackColor;
                        flatBorderSize = FlatBorderSize.Normal;
                    }
                    DrawFlatBorder(g, face, flatBorderColor, flatBorderSize);
                }
                NativeMethods.ReleaseDC(target.Handle, hDC);
            }
        }

        /// <summary>
        /// FlatStyle の枠を描画します。
        /// </summary>
        /// <param name="g">描画するコントロールの Graphics オブジェクト</param>
        /// <param name="face">枠の位置、大きさ</param>
        /// <param name="flatBorderColor">枠の色</param>
        /// <param name="flatBorderSize">枠線の幅</param>
        public static void DrawFlatBorder(Graphics g, Rectangle face, Color flatBorderColor, FlatBorderSize flatBorderSize) {
            DrawFlatBorder(g, face, flatBorderColor, flatBorderSize, DrawRectangleFlags.Full);
        }
        /// <summary>
        /// FlatStyle の枠を描画します。
        /// </summary>
        /// <param name="g">描画するコントロールの Graphics オブジェクト</param>
        /// <param name="face">枠の位置、大きさ</param>
        /// <param name="flatBorderColor">枠の色</param>
        /// <param name="flatBorderSize">枠線の幅</param>
        /// <param name="flags">描画する辺を表す DrawRectangleFlags 列挙体を指定します。</param>
        public static void DrawFlatBorder(Graphics g, Rectangle face, Color flatBorderColor, FlatBorderSize flatBorderSize, DrawRectangleFlags flags) {
            int borderSize;

            switch (flatBorderSize) {
                case FlatBorderSize.Normal:
                    borderSize = 1;
                    break;
                case FlatBorderSize.Bold:
                    borderSize = 2;
                    break;
                case FlatBorderSize.None:
                default:
                    return;
            }

            Pen p = VBGraphicsCache.GetPen(flatBorderColor, borderSize, DashStyle.Solid, PenAlignment.Inset);
            DrawRectangle(g, p, face, flags);
        }

        /// <summary>
        /// 均等割り付けを考慮して TextRenderer.DrawText を拡張します。
        /// </summary>
        /// <param name="g">テキストを描画する Graphics オブジェクト。 </param>
        /// <param name="text">描画するテキスト。 </param>
        /// <param name="font">描画するテキストに適用される Font。</param>
        /// <param name="bounds">テキストの境界を表す Rectangle。</param>
        /// <param name="color">描画するテキストに適用される Color。 </param>
        /// <param name="flags">TextFormatFlags</param>
        /// <param name="textJustify">均等割り付けのスタイル</param>
        /// <param name="JustifyPadding">埋め込みを指定します。</param>
        public static void DrawText(Graphics g, string text, Font font, Rectangle bounds, Color color, TextFormatFlags flags, TextJustifyStyles textJustify, Padding JustifyPadding) {

            // 均等割り付けを使用しない場合
            if (textJustify == TextJustifyStyles.None) {
                TextRenderer.DrawText(g, text, font, bounds, color, flags);
                return;
            }

            bounds = DeflateRect(bounds, JustifyPadding);

            // テキストの横幅が指定された枠を超える場合、均等割り付けしない
            SizeF sizeF = g.MeasureString(text, font, new Point(0, 0), StringFormat.GenericTypographic);
            float width = sizeF.Width;
            if (width > (float)bounds.Width) {
                TextRenderer.DrawText(g, text, font, bounds, color, flags);
                return;
            }

            // テキストを分解
            List<string> strs = BreakTextIntoAlignmentUnits(text);
            if (strs.Count == 0) {
                return;
            }

            // ひと文字の場合は、中央寄せ
            if (strs.Count == 1) {
                flags ^= ~TextFormatFlags.Left;
                flags ^= ~TextFormatFlags.Right;
                flags |= TextFormatFlags.HorizontalCenter;
                TextRenderer.DrawText(g, text, font, bounds, color, flags);
                return;
            }

            // 均等割り付けで描画
            DrawTextJustify(g, font, color, bounds, flags, textJustify, strs, width);
        }

        private static void DrawTextJustify(Graphics g, Font font, Color foreColor, Rectangle bounds, TextFormatFlags flags,
                                            TextJustifyStyles textJustify, List<string> alignmentUnits, float widthOfText) {

            float single = 0f;
            float[] width = new float[alignmentUnits.Count];
            for (int i = 0; i < alignmentUnits.Count; i++) {
                SizeF sizeF = g.MeasureString(alignmentUnits[i], font, new Point(0, 0), StringFormat.GenericTypographic);
                width[i] = sizeF.Width;
                single = single + width[i];
            }
            float leftMargin = 0f;
            float paddingWidth = 0f;
            if (textJustify != TextJustifyStyles.Justify) {
                paddingWidth = ((float)bounds.Width - single) / (float)(alignmentUnits.Count + 1);
                leftMargin = paddingWidth;
            } else if ((float)bounds.Width >= widthOfText && widthOfText > (float)(bounds.Width)) {
                paddingWidth = ((float)bounds.Width - single) / (float)(alignmentUnits.Count - 1);
                leftMargin = 0f;
            } else if (alignmentUnits.Count <= 1) {
                paddingWidth = 0f;
                leftMargin = ((float)bounds.Width - single) / 2f;
            } else {
                paddingWidth = ((float)bounds.Width - single) / (float)(alignmentUnits.Count - 1);
                leftMargin = 0f;
            }

            TextFormatFlags newFlags = TextFormatFlags.NoPadding;
            if ((flags & TextFormatFlags.VerticalCenter) == TextFormatFlags.VerticalCenter) {
                newFlags |= TextFormatFlags.VerticalCenter;
            }
            if ((flags & TextFormatFlags.Bottom) == TextFormatFlags.Bottom) {
                newFlags |= TextFormatFlags.Bottom;
            }

            for (int i = 0; i < alignmentUnits.Count; i++) {
                Point pt = new Point(bounds.Left + (int)leftMargin, bounds.Top);
                Size sz = new Size((int)(width[i] + paddingWidth), bounds.Height);
                TextRenderer.DrawText(g, alignmentUnits[i], font, new Rectangle(pt, sz), foreColor, newFlags);
                leftMargin = leftMargin + (width[i] + paddingWidth);
            }
        }

        private static List<string> BreakTextIntoAlignmentUnits(string text) {
            List<string> strs = new List<string>();
            TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(text);
            bool flag = true;
            StringBuilder stringBuilder = new StringBuilder();
            while (textElementEnumerator.MoveNext()) {
                string textElement = textElementEnumerator.GetTextElement();
                if (textElement.Length != 1 || !char.IsWhiteSpace(textElement, 0)) {
                    if (flag) {
                        flag = false;
                    } else if (stringBuilder.Length > 0) {
                        strs.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                    }
                    stringBuilder.Append(textElement);
                } else {
                    if (stringBuilder.Length <= 0) {
                        continue;
                    }
                    strs.Add(stringBuilder.ToString());
                    stringBuilder.Length = 0;
                    flag = true;
                }
            }
            if (stringBuilder.Length > 0) {
                strs.Add(stringBuilder.ToString());
            }
            return strs;
        }

        /// <summary>
        /// 指定したイメージを無効状態で描画します。 
        /// </summary>
        /// <param name="graphics">描画する Graphics。</param>
        /// <param name="image">描画する Image。</param>
        /// <param name="imageBounds">描画する領域を示す Rectangle 構造体</param>
        /// <param name="background">コントロールの背景色</param>
        /// <param name="unscaledImage">Image を描画領域に合わせる場合は false。そうでない場合は true</param>
        public static void DrawImageDisabled(Graphics graphics, Image image, Rectangle imageBounds, Color background, bool unscaledImage) {
            Size size = image.Size;
            if (disabledImageAttr == null) {
                float[][] singleArray = new float[][] { new float[] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f }, new float[] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f }, new float[] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f }, default(float[]), default(float[]) };
                float[] singleArray1 = new float[] { default(float), default(float), default(float), 1f, default(float) };
                singleArray[3] = singleArray1;
                singleArray[4] = new float[] { 0.38f, 0.38f, 0.38f, 0f, 1f };
                ColorMatrix colorMatrix = new ColorMatrix(singleArray);
                disabledImageAttr = new ImageAttributes();
                disabledImageAttr.ClearColorKey();
                disabledImageAttr.SetColorMatrix(colorMatrix);
            }
            if (!unscaledImage) {
                graphics.DrawImage(image, imageBounds, 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, disabledImageAttr);
            } else {
                using (Bitmap bitmap = new Bitmap(image.Width, image.Height)) {
                    using (Graphics graphic = Graphics.FromImage(bitmap)) {
                        graphic.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, disabledImageAttr);
                    }
                    graphics.DrawImageUnscaled(bitmap, imageBounds);
                }
            }
        }

        /// <summary>
        /// 非クライアント領域に枠を描画します。
        /// </summary>
        /// <param name="m">WM_NCPAINT の Window メッセージ</param>
        /// <param name="borderColor">線の色を指定します。</param>
        /// <param name="backColor">背景色を指定します。</param>
        public static void DrawNonClientBorder(ref Message m, Color borderColor, Color backColor) {
            DrawNonClientBorder(ref m, borderColor, backColor, DrawRectangleFlags.Full);
        }

        /// <summary>
        /// 非クライアント領域に枠を描画します。
        /// </summary>
        /// <param name="m">WM_NCPAINT の Window メッセージ</param>
        /// <param name="borderColor">線の色を指定します。</param>
        /// <param name="backColor">背景色を指定します。</param>
        /// <param name="flags">枠を描画する辺を指定します。</param>
        public static void DrawNonClientBorder(ref Message m, Color borderColor, Color backColor, DrawRectangleFlags flags) {

            // ウインドウ領域を求める
            NativeMethods.RECT ncRect = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(m.HWnd, ref ncRect);
            Rectangle ncFace = new Rectangle(new Point(0, 0), ncRect.Size);

            // クライアント領域を求める
            Rectangle clRect = GetClientRectNC(m.HWnd);

            // スクロールバーが表示されているか判定
            int style = (int)NativeMethods.GetWindowLongPtr(m.HWnd, NativeMethods.GWL_STYLE);
            bool vertSB = (style & NativeMethods.WS_VSCROLL) == NativeMethods.WS_VSCROLL;
            bool horzSB = (style & NativeMethods.WS_HSCROLL) == NativeMethods.WS_HSCROLL;
            int sbWidth = vertSB ? clRect.Width + SystemInformation.VerticalScrollBarWidth : clRect.Width;
            int sbHeight = horzSB ? clRect.Height + SystemInformation.HorizontalScrollBarHeight : clRect.Height;
            // クライアント領域＋スクロールバーの領域
            Rectangle sbRect = new Rectangle(clRect.Location, new Size(sbWidth, sbHeight));

            // スクロールバーが表示されていれば描画してもらう
            if (vertSB || horzSB) {
                IntPtr hRgn = NativeMethods.CreateRectRgn(0, 0, 10, 10);
                IntPtr hRgnSB = CreateRectRgn(ncRect.Location, sbRect);
                IntPtr hRgnCL = CreateRectRgn(ncRect.Location, clRect);
                NativeMethods.CombineRgn(hRgn, hRgnSB, hRgnCL, NativeMethods.CombineRgnStyles.RGN_XOR);
                NativeMethods.DefWindowProc(m.HWnd, NativeMethods.WM_NCPAINT, hRgn, IntPtr.Zero);
                NativeMethods.DeleteObject(hRgn);
                NativeMethods.DeleteObject(hRgnSB);
                NativeMethods.DeleteObject(hRgnCL);
            }

            IntPtr hdc = NativeMethods.GetWindowDC(m.HWnd);
            if (hdc != IntPtr.Zero) {
                using (Graphics g = Graphics.FromHdc(hdc)) {
                    // クライアント領域＋スクロールバーの領域を除いた領域を背景色で塗りつぶす
                    Region region = new Region(ncFace);
                    region.Exclude(sbRect);
                    g.FillRegion(VBGraphicsCache.GetSolidBrush(backColor), region);

                    // 線を引く
                    DrawRectangle(g, VBGraphicsCache.GetPen(borderColor), ncFace, flags);
                }
                NativeMethods.ReleaseDC(m.HWnd, hdc);
            }
        }

        private static IntPtr CreateRectRgn(Point location, Rectangle rect) {
            Rectangle screenRect = new Rectangle(rect.Location + (Size)location, rect.Size);
            return NativeMethods.CreateRectRgn(screenRect.Left, screenRect.Top, screenRect.Right, screenRect.Bottom);
        }
        /// <summary>
        /// 非クライアント領域に対するクライアント領域の座標とサイズを取得します。
        /// </summary>
        /// <param name="handle">ウインドウハンドル</param>
        /// <returns>非クライアント領域に対するクライアント領域の座標を示す Rectangle 構造体</returns>
        public static Rectangle GetClientRectNC(IntPtr handle) {

            // 非クライアント領域
            var ncRect = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(handle, ref ncRect);

            // クライアント領域の座標取得
            var clRect = new NativeMethods.RECT();
            NativeMethods.GetClientRect(handle, ref clRect);

            // クライアント座標をスクリーン座標に変換し左上隅を求める
            NativeMethods.POINT clLocation = new NativeMethods.POINT(clRect.Location);
            NativeMethods.ClientToScreen(handle, ref clLocation);
            Point clPoint = clLocation.Location;

            clPoint = clPoint - (Size)ncRect.Location;
            // 左上隅とサイズからクライアント領域作成
            return new Rectangle(clPoint, clRect.Size);
        }

        /// <summary>
        /// 辺を指定して、四角形を描画します。
        /// </summary>
        /// <param name="g">対象の Graphics オブジェクト</param>
        /// <param name="pen">線分の色、幅、およびスタイルを決定する Pen。 </param>
        /// <param name="bounds">描画する四角形を表す Rectangle 構造体。</param>
        /// <param name="flag">描画する辺を表す DrawRectAngleFlags 列挙体</param>
        public static void DrawRectangle(Graphics g, Pen pen, Rectangle bounds, DrawRectangleFlags flag) {
            int penWidth = (int)pen.Width;
            List<Rectangle> rects = new List<Rectangle>();

            if ((flag & DrawRectangleFlags.Top) == DrawRectangleFlags.Top) {
                rects.Add(new Rectangle(new Point(bounds.Left, bounds.Top), new Size(bounds.Width, penWidth)));
            }
            if ((flag & DrawRectangleFlags.Left) == DrawRectangleFlags.Left) {
                rects.Add(new Rectangle(new Point(bounds.Left, bounds.Top), new Size(penWidth, bounds.Height)));
            }
            if ((flag & DrawRectangleFlags.Right) == DrawRectangleFlags.Right) {
                rects.Add(new Rectangle(new Point(bounds.Right - penWidth, bounds.Top), new Size(penWidth, bounds.Height)));
            }
            if ((flag & DrawRectangleFlags.Bottom) == DrawRectangleFlags.Bottom) {
                rects.Add(new Rectangle(new Point(bounds.Left, bounds.Bottom - penWidth), new Size(bounds.Width, penWidth)));
            }
            if (rects.Count > 0) {
                g.FillRectangles(VBGraphicsCache.GetSolidBrush(pen.Color), rects.ToArray());
            }
        }

        /// <summary>
        /// TreeNodeStates を ButtonState に変換します。
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static ButtonState ToFlatButtonState(TreeNodeStates state) {
            ButtonState bs = ButtonState.Flat;
            if ((state & TreeNodeStates.Selected) == TreeNodeStates.Selected) {
                bs |= ButtonState.Pushed;
            }
            if ((state & TreeNodeStates.Grayed) == TreeNodeStates.Grayed) {
                bs |= ButtonState.Inactive;
            }
            if ((state & TreeNodeStates.Checked) == TreeNodeStates.Checked) {
                bs |= ButtonState.Checked;
            }
            return bs;
        }

        private static Size _CheckBoxSize = new Size(10, 10);
        private static Size _RadioButtonSize = new Size(11, 11);

        /// <summary>
        /// フラットなチェックボックスを描画します。
        /// </summary>
        /// <param name="g">対象となる Graphics オブジェクト</param>
        /// <param name="foreColor">前景色</param>
        /// <param name="backColor">背景色</param>
        /// <param name="rect">対象となる領域</param>
        /// <param name="enabled">活性のとき true。そうでないときは false。</param>
        /// <param name="state">チェックボックスのステータス</param>
        public static void DrawFlatCheckBox(Graphics g, Color foreColor, Color backColor, Rectangle rect, bool enabled, CheckState state) {
            var left = rect.Left + (rect.Width - _CheckBoxSize.Width) / 2;
            var top = rect.Top + (rect.Height - _CheckBoxSize.Height) / 2;
            var checkBoxRect = new Rectangle(new Point(left, top), _CheckBoxSize);
            Pen forePen;
            Brush backBrush;
            Brush backIndeterminate;
            if (enabled) {
                forePen = VBGraphicsCache.GetPen(foreColor);
                backBrush = VBGraphicsCache.GetSolidBrush(backColor);
                backIndeterminate = CreateMeshBrush(foreColor, backColor);
            } else {
                Color tmpColor = ControlPaint.LightLight(foreColor);
                forePen = VBGraphicsCache.GetPen(tmpColor);
                backBrush = VBGraphicsCache.GetSolidBrush(ControlPaint.LightLight(tmpColor));
                backIndeterminate = CreateMeshBrush(tmpColor, ControlPaint.LightLight(tmpColor));
            }
            switch (state) {
                case CheckState.Checked:
                    g.FillRectangle(backBrush, checkBoxRect);
                    g.DrawRectangle(forePen, checkBoxRect);
                    DrawCheckBoxCheck(g, forePen, checkBoxRect);
                    break;
                case CheckState.Unchecked:
                    g.FillRectangle(backBrush, checkBoxRect);
                    g.DrawRectangle(forePen, checkBoxRect);
                    break;
                case CheckState.Indeterminate:
                    g.FillRectangle(backIndeterminate, checkBoxRect);
                    g.DrawRectangle(forePen, checkBoxRect);
                    break;
            }
            if (backIndeterminate != null) {
                backIndeterminate.Dispose();
            }
        }

        /// <summary>
        /// 網目状のブラシを作成します。
        /// </summary>
        /// <param name="color1">色１</param>
        /// <param name="color2">色２</param>
        public static Brush CreateMeshBrush(Color color1, Color color2) {
            using (Bitmap b = new Bitmap(2, 2)) {
                b.SetPixel(0, 0, color1);
                b.SetPixel(0, 1, color2);
                b.SetPixel(1, 1, color1);
                b.SetPixel(1, 0, color2);
                return new TextureBrush(b);
            }
        }
        static void DrawCheckBoxCheck(Graphics g, Pen forePen, Rectangle checkBoxRect) {
            g.DrawLine(forePen, new Point(checkBoxRect.Left + 2, checkBoxRect.Top + 5), new Point(checkBoxRect.Left + 2, checkBoxRect.Top + 7));
            g.DrawLine(forePen, new Point(checkBoxRect.Left + 3, checkBoxRect.Top + 6), new Point(checkBoxRect.Left + 3, checkBoxRect.Top + 8));
            g.DrawLine(forePen, new Point(checkBoxRect.Left + 4, checkBoxRect.Top + 7), new Point(checkBoxRect.Left + 4, checkBoxRect.Top + 9));

            g.DrawLine(forePen, new Point(checkBoxRect.Left + 5, checkBoxRect.Top + 6), new Point(checkBoxRect.Left + 5, checkBoxRect.Top + 8));
            g.DrawLine(forePen, new Point(checkBoxRect.Left + 6, checkBoxRect.Top + 5), new Point(checkBoxRect.Left + 6, checkBoxRect.Top + 7));
            g.DrawLine(forePen, new Point(checkBoxRect.Left + 7, checkBoxRect.Top + 4), new Point(checkBoxRect.Left + 7, checkBoxRect.Top + 6));
            g.DrawLine(forePen, new Point(checkBoxRect.Left + 8, checkBoxRect.Top + 3), new Point(checkBoxRect.Left + 8, checkBoxRect.Top + 5));
        }


        private struct HLSColor
        {
            private const int ShadowAdj = -333;
            private const int HilightAdj = 500;
            private const int WatermarkAdj = -50;

            private const int Range = 240;
            private const int HLSMax = Range;
            private const int RGBMax = 255;
            private const int Undefined = HLSMax * 2 / 3;

            private int hue;
            private int saturation;
            private int luminosity;

            private bool isSystemColors_Control;

            public HLSColor(Color color) {
                isSystemColors_Control = (color.ToKnownColor() == SystemColors.Control.ToKnownColor());
                int r = color.R;
                int g = color.G;
                int b = color.B;
                int max, min;        /* max and min RGB values */
                int sum, dif;
                int Rdelta, Gdelta, Bdelta;  /* intermediate value: % of spread from max */

                /* calculate lightness */
                max = Math.Max(Math.Max(r, g), b);
                min = Math.Min(Math.Min(r, g), b);
                sum = max + min;

                luminosity = (((sum * HLSMax) + RGBMax) / (2 * RGBMax));

                dif = max - min;
                if (dif == 0) {       /* r=g=b --> achromatic case */
                    saturation = 0;                         /* saturation */
                    hue = Undefined;                 /* hue */
                } else {                           /* chromatic case */
                    /* saturation */
                    if (luminosity <= (HLSMax / 2))
                        saturation = (int)(((dif * (int)HLSMax) + (sum / 2)) / sum);
                    else
                        saturation = (int)((int)((dif * (int)HLSMax) + (int)((2 * RGBMax - sum) / 2))
                                            / (2 * RGBMax - sum));
                    /* hue */
                    Rdelta = (int)((((max - r) * (int)(HLSMax / 6)) + (dif / 2)) / dif);
                    Gdelta = (int)((((max - g) * (int)(HLSMax / 6)) + (dif / 2)) / dif);
                    Bdelta = (int)((((max - b) * (int)(HLSMax / 6)) + (dif / 2)) / dif);

                    if ((int)r == max)
                        hue = Bdelta - Gdelta;
                    else if ((int)g == max)
                        hue = (HLSMax / 3) + Rdelta - Bdelta;
                    else /* B == cMax */
                        hue = ((2 * HLSMax) / 3) + Gdelta - Rdelta;

                    if (hue < 0)
                        hue += HLSMax;
                    if (hue > HLSMax)
                        hue -= HLSMax;
                }
            }

            public int Hue {
                get {
                    return hue;
                }
            }

            public int Luminosity {
                get {
                    return luminosity;
                }
            }

            public int Saturation {
                get {
                    return saturation;
                }
            }

            public Color Darker(float percDarker) {
                if (isSystemColors_Control) {
                    // With the usual color scheme, ControlDark/DarkDark is not exactly
                    // what we would otherwise calculate
                    if (percDarker == 0.0f) {
                        return SystemColors.ControlDark;
                    } else if (percDarker == 1.0f) {
                        return SystemColors.ControlDarkDark;
                    } else {
                        Color dark = SystemColors.ControlDark;
                        Color darkDark = SystemColors.ControlDarkDark;

                        int dr = dark.R - darkDark.R;
                        int dg = dark.G - darkDark.G;
                        int db = dark.B - darkDark.B;

                        return Color.FromArgb((byte)(dark.R - (byte)(dr * percDarker)),
                                              (byte)(dark.G - (byte)(dg * percDarker)),
                                              (byte)(dark.B - (byte)(db * percDarker)));
                    }
                } else {
                    int oneLum = 0;
                    int zeroLum = NewLuma(ShadowAdj, true);

                    /*                                        
                    if (luminosity < 40) {
                        zeroLum = NewLuma(120, ShadowAdj, true);
                    }
                    else {
                        zeroLum = NewLuma(ShadowAdj, true);
                    }
                    */

                    return ColorFromHLS(hue, zeroLum - (int)((zeroLum - oneLum) * percDarker), saturation);
                }
            }

            public override bool Equals(object o) {
                if (!(o is HLSColor)) {
                    return false;
                }

                HLSColor c = (HLSColor)o;
                return hue == c.hue &&
                       saturation == c.saturation &&
                       luminosity == c.luminosity &&
                       isSystemColors_Control == c.isSystemColors_Control;
            }

            public static bool operator ==(HLSColor a, HLSColor b) {
                return a.Equals(b);
            }

            public static bool operator !=(HLSColor a, HLSColor b) {
                return !a.Equals(b);
            }

            public override int GetHashCode() {
                return hue << 6 | saturation << 2 | luminosity;
            }

            public Color Lighter(float percLighter) {
                if (isSystemColors_Control) {
                    // With the usual color scheme, ControlLight/LightLight is not exactly
                    // what we would otherwise calculate
                    if (percLighter == 0.0f) {
                        return SystemColors.ControlLight;
                    } else if (percLighter == 1.0f) {
                        return SystemColors.ControlLightLight;
                    } else {
                        Color light = SystemColors.ControlLight;
                        Color lightLight = SystemColors.ControlLightLight;

                        int dr = light.R - lightLight.R;
                        int dg = light.G - lightLight.G;
                        int db = light.B - lightLight.B;

                        return Color.FromArgb((byte)(light.R - (byte)(dr * percLighter)),
                                              (byte)(light.G - (byte)(dg * percLighter)),
                                              (byte)(light.B - (byte)(db * percLighter)));
                    }
                } else {
                    int zeroLum = luminosity;
                    int oneLum = NewLuma(HilightAdj, true);

                    /*
                    if (luminosity < 40) {
                        zeroLum = 120;
                        oneLum = NewLuma(120, HilightAdj, true);
                    }
                    else {
                        zeroLum = luminosity;
                        oneLum = NewLuma(HilightAdj, true);
                    }
                    */

                    return ColorFromHLS(hue, zeroLum + (int)((oneLum - zeroLum) * percLighter), saturation);
                }
            }

            private int NewLuma(int n, bool scale) {
                return NewLuma(luminosity, n, scale);
            }

            private int NewLuma(int luminosity, int n, bool scale) {
                if (n == 0)
                    return luminosity;

                if (scale) {
                    if (n > 0) {
                        return (int)(((int)luminosity * (1000 - n) + (Range + 1L) * n) / 1000);
                    } else {
                        return (int)(((int)luminosity * (n + 1000)) / 1000);
                    }
                }

                int newLum = luminosity;
                newLum += (int)((long)n * Range / 1000);

                if (newLum < 0)
                    newLum = 0;
                if (newLum > HLSMax)
                    newLum = HLSMax;

                return newLum;
            }

            private Color ColorFromHLS(int hue, int luminosity, int saturation) {
                byte r, g, b;                      /* RGB component values */
                int magic1, magic2;       /* calculated magic numbers (really!) */

                if (saturation == 0) {                /* achromatic case */
                    r = g = b = (byte)((luminosity * RGBMax) / HLSMax);
                    if (hue != Undefined) {
                        /* ERROR */
                    }
                } else {                         /* chromatic case */
                    /* set up magic numbers */
                    if (luminosity <= (HLSMax / 2))
                        magic2 = (int)((luminosity * ((int)HLSMax + saturation) + (HLSMax / 2)) / HLSMax);
                    else
                        magic2 = luminosity + saturation - (int)(((luminosity * saturation) + (int)(HLSMax / 2)) / HLSMax);
                    magic1 = 2 * luminosity - magic2;

                    /* get RGB, change units from HLSMax to RGBMax */
                    r = (byte)(((HueToRGB(magic1, magic2, (int)(hue + (int)(HLSMax / 3))) * (int)RGBMax + (HLSMax / 2))) / (int)HLSMax);
                    g = (byte)(((HueToRGB(magic1, magic2, hue) * (int)RGBMax + (HLSMax / 2))) / HLSMax);
                    b = (byte)(((HueToRGB(magic1, magic2, (int)(hue - (int)(HLSMax / 3))) * (int)RGBMax + (HLSMax / 2))) / (int)HLSMax);
                }
                return Color.FromArgb(r, g, b);
            }

            private int HueToRGB(int n1, int n2, int hue) {
                /* range check: note values passed add/subtract thirds of range */

                /* The following is redundant for WORD (unsigned int) */
                if (hue < 0)
                    hue += HLSMax;

                if (hue > HLSMax)
                    hue -= HLSMax;

                /* return r,g, or b value from this tridrant */
                if (hue < (HLSMax / 6))
                    return (n1 + (((n2 - n1) * hue + (HLSMax / 12)) / (HLSMax / 6)));
                if (hue < (HLSMax / 2))
                    return (n2);
                if (hue < ((HLSMax * 2) / 3))
                    return (n1 + (((n2 - n1) * (((HLSMax * 2) / 3) - hue) + (HLSMax / 12)) / (HLSMax / 6)));
                else
                    return (n1);

            }

            /// <summary>
            /// 辺を指定して、四角形を描画します。
            /// </summary>
            /// <param name="g">対象の Graphics オブジェクト</param>
            /// <param name="pen">線分の色、幅、およびスタイルを決定する Pen。 </param>
            /// <param name="bounds">描画する四角形を表す Rectangle 構造体。</param>
            /// <param name="flag">描画する辺を表す DrawRectAngleFlags 列挙体</param>
            public static void DrawRectangle(Graphics g, Pen pen, Rectangle bounds, DrawRectangleFlags flag) {
                int penWidth = (int)pen.Width;
                List<Rectangle> rects = new List<Rectangle>();

                if ((flag & DrawRectangleFlags.Top) == DrawRectangleFlags.Top) {
                    rects.Add(new Rectangle(new Point(bounds.Left, bounds.Top), new Size(bounds.Width, penWidth)));
                }
                if ((flag & DrawRectangleFlags.Left) == DrawRectangleFlags.Left) {
                    rects.Add(new Rectangle(new Point(bounds.Left, bounds.Top), new Size(penWidth, bounds.Height)));
                }
                if ((flag & DrawRectangleFlags.Right) == DrawRectangleFlags.Right) {
                    rects.Add(new Rectangle(new Point(bounds.Right - penWidth, bounds.Top), new Size(penWidth, bounds.Height)));
                }
                if ((flag & DrawRectangleFlags.Bottom) == DrawRectangleFlags.Bottom) {
                    rects.Add(new Rectangle(new Point(bounds.Left, bounds.Bottom - penWidth), new Size(bounds.Width, penWidth)));
                }
                if (rects.Count > 0) {
                    g.FillRectangles(VBGraphicsCache.GetSolidBrush(pen.Color), rects.ToArray());
                }
            }
        }
    }
}
