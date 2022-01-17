using System;
using IO = System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Scripting
{
    /// <summary>
    /// 利用可能なすべてのドライブのコレクションです。このコレクションは、メンバの取得のみ可能です。
    /// </summary>
    public class Drives : MarshalByRefObject, IEnumerable, IEnumerable<Drive>
	{
        Dictionary<string, Drive> m_DriveCollection;

        internal Drives(FileSystemObject fso)
        {
            m_DriveCollection = new Dictionary<string, Drive>();
            IO.DriveInfo[] infos = IO.DriveInfo.GetDrives();
            for (int i = 0; i < infos.Length; i++)
            {
                Drive d = new Drive(fso, infos[i].Name);
                m_DriveCollection.Add(d.DriveLetter, d);
            }
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>オブジェクトを表す文字列を返します。</returns>
        public override string ToString()
        {
            return m_DriveCollection.ToString();
        }

        /// <summary>
        /// コレクションに含まれる件数を返します。
        /// </summary>
        /// <value>コレクションに含まれる件数</value>
        public int Count
        {
            get { return m_DriveCollection.Count; }
        }

        /// <summary>
        /// 指定したドライブ名が使用できるかを取得します。
        /// </summary>
        /// <param name="DriveLetter">ドライブ名</param>
        /// <returns>使用できる場合は True、出来ない場合は False</returns>
        public bool Exists(string DriveLetter)
        {
            return m_DriveCollection.ContainsKey(DriveLetter);
        }

        /// <summary>
        /// 指定したドライブ名を示す Drive オブジェクトを返します。
        /// </summary>
        /// <param name="name">Drive オブジェクトを取得するドライブ名を指定します。</param>
        /// <returns>Drive オブジェクト</returns>
        public Drive this[string name]
        {
            get { return m_DriveCollection[name]; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_DriveCollection.GetEnumerator();
        }

        IEnumerator<Drive> IEnumerable<Drive>.GetEnumerator()
        {
            return m_DriveCollection.Values.GetEnumerator();
        }
    }
}