namespace VBCompatible
{
    using System;
    using System.Drawing;

    static class VBSystem
    {
        [ThreadStatic]
        private static Font _DefaultFont;
        private const byte SHIFTJIS_CHARSET = 128;

        public static Font DefaultFont {
            get {
                if (_DefaultFont == null) {
                    _DefaultFont = new Font("ＭＳ Ｐゴシック", 9.0f,
                        FontStyle.Regular, GraphicsUnit.Point, SHIFTJIS_CHARSET);
                }
                return _DefaultFont;
            }
        }

    }
}
