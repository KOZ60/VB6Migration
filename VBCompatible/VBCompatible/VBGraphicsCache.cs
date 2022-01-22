using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VBCompatible
{
    public class VBGraphicsCache
    {
        public static Pen GetPen(Color color) {
            return GetPen(color, 1.0f, DashStyle.Solid, PenAlignment.Center);
        }

        public static Pen GetPen(Color color, float width) {
            return GetPen(color, width, DashStyle.Solid, PenAlignment.Center);
        }

        public static Pen GetPen(Color color, DashStyle style) {
            return GetPen(color, 1.0f, style, PenAlignment.Center);
        }

        public static Pen GetPen(Color color, float width, DashStyle style) {
            return GetPen(color, width, style, PenAlignment.Center);
        }

        public static Pen GetPen(Color color, float width, DashStyle style, PenAlignment alignment) {
            if (_PensCache == null) {
                _PensCache = new PensCache();
            }
            var key = new Tuple<Color, float, DashStyle, PenAlignment>(color, width, style, alignment);
            return _PensCache[key];
        }

        public static SolidBrush GetSolidBrush(Color color) {
            if (_SolidBrshesCache == null) {
                _SolidBrshesCache = new SolidBrshesCache();
            }
            return _SolidBrshesCache[color];
        }

        public static VBNativeBrush GetNativeBrush(Color color) {
            if (_NativeBrushesCache == null) {
                _NativeBrushesCache = new NativeBrushesCache();
            }
            return _NativeBrushesCache[color];
        }

        [ThreadStatic]
        static SolidBrshesCache _SolidBrshesCache;

        [ThreadStatic]
        static PensCache _PensCache;

        [ThreadStatic]
        static NativeBrushesCache _NativeBrushesCache;

        private class SolidBrshesCache : VBCache<Color, SolidBrush>
        {
            protected override SolidBrush CreateItem(Color key) {
                return new SolidBrush(key);
            }
        }

        private class PensCache : VBCache<Tuple<Color, float, DashStyle, PenAlignment>, Pen>
        {
            protected override Pen CreateItem(Tuple<Color, float, DashStyle, PenAlignment> key) {
                Pen pen = new Pen(key.Item1, key.Item2);
                if (key.Item3 != DashStyle.Solid) {
                    pen.DashStyle = key.Item3;
                }
                if (key.Item4 != PenAlignment.Center) {
                    pen.Alignment = key.Item4;
                }
                return pen;
            }
        }

        private class NativeBrushesCache : VBCache<Color, VBNativeBrush>
        {
            protected override VBNativeBrush CreateItem(Color key) {
                return new VBNativeBrush(key);
            }
        }

    }
}
