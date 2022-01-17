using System;
using System.Drawing;
using System.Text;

namespace VBCompatible
{
    /// <summary>
    /// Integer と Color の相互変換を行うクラス
    /// </summary>
    public class OLE_COLOR : IComparable, ICloneable, IEquatable<OLE_COLOR>
    {
        private int m_OleColor;

        private OLE_COLOR(int value) {
            m_OleColor = value;
        }

        private OLE_COLOR(Color value) {
            m_OleColor = ColorTranslator.ToOle(value);
        }

        /// <summary>
        /// Integer から OLE_COLOR へ暗黙の型変換を行います。
        /// </summary>
        public static implicit operator OLE_COLOR(int value) {
            return new OLE_COLOR(value);
        }

        /// <summary>
        /// Color から OLE_COLOR へ暗黙の型変換を行います。
        /// </summary>
        public static implicit operator OLE_COLOR(Color value) {
            return new OLE_COLOR(value);
        }

        /// <summary>
        /// OLE_COLOR から int へ暗黙の型変換を行います。
        /// </summary>
        public static implicit operator int(OLE_COLOR value) {
            return value.m_OleColor;
        }

        /// <summary>
        /// OLE_COLOR から Color へ暗黙の型変換を行います。
        /// </summary>
        public static implicit operator Color(OLE_COLOR value) {
            if (value == null) {
                return ColorTranslator.FromOle(0);
            }
            return ColorTranslator.FromOle(value.m_OleColor);
        }

        /// <summary>
        /// 指定した 2 つの OLE_COLOR オブジェクトを比較します。
        /// </summary>
        /// <param name="a">第１の引数</param>
        /// <param name="b">第２の引数</param>
        /// <returns>
        /// 2 つの比較対照値の構文上の関係を示す 32 ビット符号付き整数。 
        /// 0 : 2 つの文字列は等価。
        /// 0 より小さい値 : a が b より小さい。
        /// 0 より大きい値 : a が b より大きくなっています。
        /// </returns>
        public static int Compare(OLE_COLOR a, OLE_COLOR b) {

            // 両方 Null かインスタンスが同一ならゼロ
            if (object.ReferenceEquals((object)a, (object)b)) {
                return 0;
            }

            // a が null なら b のほうが大きい
            if ((object)a == null) {
                return -1;
            }

            // b が null なら a のほうが大きい
            if ((object)b == null) {
                return 1;
            }

            return a.m_OleColor - b.m_OleColor;
        }

        /// <summary>
        /// 現在のインスタンスを別のオブジェクトと比較し、現在のインスタンスの並べ替え順序での位置が、比較対象のオブジェクトと比べて前か、後か、または同じかを示す整数を返します。
        /// </summary>
        /// <param name="obj">このインスタンスと比較するオブジェクト。</param>
        public int CompareTo(object obj) {
            if (obj is int) {
                return Compare(this, (int)obj);
            } else if (obj is Color) {
                return Compare(this, (Color)obj);
            } else {
                return Compare(this, (OLE_COLOR)obj);
            }
        }

        /// <summary>
        /// このインスタンスと指定した Object が同じ型と値を表しているかどうかを示す値を返します。
        /// </summary>
        /// <param name="obj">このインスタンスと比較するオブジェクト。</param>
        /// <returns>インスタンスが等しいときは true。そうでない場合は false。</returns>
        public override bool Equals(object obj) {
            return this.CompareTo(obj) == 0;
        }

        /// <summary>
        /// このインスタンスのハッシュ コードを返します。
        /// </summary>
        public override int GetHashCode() {
            return m_OleColor.GetHashCode();
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>現在のオブジェクトを説明する文字列。 </returns>
        public override string ToString() {
            Color c = this;
            StringBuilder sb = new StringBuilder(32);
            sb.Append(GetType().Name);
            sb.Append(" [");
            if (c.IsEmpty) {
                sb.Append("Empty");
            } else if (c.IsNamedColor || c.IsKnownColor || c.IsSystemColor) {
                sb.Append(c.Name);
            } else {
                sb.Append(string.Format("A={0}, R={1}, G={2}, B{3}", c.A, c.R, c.G, c.B));
            }
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// オブジェクトの簡易コピーを行います。
        /// </summary>
        public object Clone() {
            return new OLE_COLOR(m_OleColor);
        }

        /// <summary>
        /// このインスタンスと指定した OLE_COLOR が同じ値を表しているかどうかを示す値を返します。
        /// </summary>
        /// <param name="other">このインスタンスと比較するオブジェクト。</param>
        /// <returns>インスタンスが等しいときは true。そうでない場合は false。</returns>
        public bool Equals(OLE_COLOR other) {
            return this.CompareTo(other) == 0;
        }

        /// <summary>= 演算子</summary>
        public static bool operator ==(OLE_COLOR a, OLE_COLOR b) {
            return Compare(a, b) == 0;
        }

        /// <summary>!= 演算子</summary>
        public static bool operator !=(OLE_COLOR a, OLE_COLOR b) {
            return Compare(a, b) != 0;
        }
    }
}
