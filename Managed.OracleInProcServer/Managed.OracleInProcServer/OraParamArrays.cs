using System.Collections;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    /// <summary>
    /// OraParamArrayオブジェクトのコレクション
    /// </summary>
    public interface OraParamArrays : IEnumerable<OraParamArray>
    {
        /// <summary>
        /// 要素数を返します。
        /// </summary>
        int Count { get; }
        /// <summary>
        /// コレクションの要素を消去します。
        /// </summary>
        void Clear();
        /// <summary>
        /// OraParamArraysコレクションに配列パラメータを追加します。
        /// </summary>
        /// <param name="Name">パラメータ・コレクションに追加するパラメータの名前。 この名前は、パラメータを識別するため、および関連するSQL文とPL/SQL文内のプレースホルダとして使用されます。</param>
        /// <param name="IOtype">SQL文とPL/SQLブロック内でのパラメータの使用方法を指定するための整数コード。</param>
        /// <param name="varType">この配列パラメータがバインドされるOracle Databaseの型を指定します。 </param>
        /// <param name="Dimension">パラメータの配列要素の数を指定します。</param>
        OraParamArray AddTable(
                string Name,
                paramMode IOtype,
                serverType varType,
                int Dimension
        );
        /// <summary>
        /// OraParamArraysコレクションに配列パラメータを追加します。
        /// </summary>
        /// <param name="Name">パラメータ・コレクションに追加するパラメータの名前。 この名前は、パラメータを識別するため、および関連するSQL文とPL/SQL文内のプレースホルダとして使用されます。</param>
        /// <param name="IOtype">SQL文とPL/SQLブロック内でのパラメータの使用方法を指定するための整数コード。</param>
        /// <param name="varType">この配列パラメータがバインドされるOracle Databaseの型を指定します。 </param>
        /// <param name="Dimension">パラメータの配列要素の数を指定します。</param>
        /// <param name="size">要素のサイズを定義します。 文字および文字列型の表（配列）パラメータにのみ有効です。</param>
        OraParamArray AddTable(
                string Name,
                paramMode IOtype,
                serverType varType,
                int Dimension,
                int size
        );
        /// <summary>
        /// OraParamArraysコレクションから配列パラメータを削除します。
        /// </summary>
        /// <param name="name">削除する配列パラメータのキーを文字列で指定します。</param>
        void Remove(string name);
        /// <summary>
        /// OraParamArraysコレクションから配列パラメータを削除します。
        /// </summary>
        /// <param name="index">削除する配列パラメータのキーを数値で指定します。</param>
        void Remove(int index);
        /// <summary>
        /// コレクション中の OraParamArray オブジェクトを返します。
        /// </summary>
        /// <param name="index">OraParamArray のキーを数値で指定します。</param>
        /// <returns>コレクション中の OraParamArray オブジェクト。</returns>
        OraParamArray this[int index] { get; }
        /// <summary>
        /// コレクション中の OraParamArray オブジェクトを返します。
        /// </summary>
        /// <param name="name">OraParamArray のキーを文字列で指定します。</param>
        /// <returns>コレクション中の OraParamArray オブジェクト。</returns>
        OraParamArray this[string name] { get; }
    }

}
