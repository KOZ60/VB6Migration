using System;
using System.Drawing;

namespace VBCompatible
{
    public class VBNativeBrushCache
    {
        [ThreadStatic]
        static NativeBrushes brushes;

        static NativeBrushes Brushes {
            get {
                if (brushes == null) {
                    brushes = new NativeBrushes();
                }
                return brushes;
            }
        }

        private class NativeBrushes : VBCache<Color, VBNativeBrush>
        {
            public NativeBrushes() : base(256) { }

            protected override VBNativeBrush CreateItem(Color key) {
                return new VBNativeBrush(key);
            }
        }

        public static VBNativeBrush GetNativeBrush(Color color) {
            return Brushes[color];
        }
    }
}
