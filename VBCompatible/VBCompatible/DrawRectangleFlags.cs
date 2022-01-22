using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBCompatible
{
    /// <summary>
    /// 描画する辺を表します。
    /// </summary>
    [Flags()]
    public enum DrawRectangleFlags
    {
        /// <summary>上辺を描画します。</summary>
        Top = 0x1,
        /// <summary>左辺を描画します。</summary>
        Left = 0x2,
        /// <summary>右辺を描画します。</summary>
        Right = 0x4,
        /// <summary>下辺を描画します。</summary>
        Bottom = 0x8,
        /// <summary>すべての辺を描画します。</summary>
        Full = 0xf
    }
}
