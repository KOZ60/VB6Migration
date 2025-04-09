using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MSWinsockLib
{
    [Serializable]
    internal class ByteBuffer : IDisposable
    {
        const int initialSize = 10240;
        IntPtr m_Pointer;
        int m_Length;
        int m_AllocSize;

        public int Length
        {
            get { return m_Length; }
        }

        public IntPtr Pointer
        {
            get { return m_Pointer; }
        }

        public ByteBuffer() : this(initialSize) { }

        public ByteBuffer(int size)
        {
            m_Pointer = Marshal.AllocHGlobal(size);
            m_AllocSize = size;
            Clear();
        }

        private void Realloc(int size)
        {
            IntPtr newPointer = Marshal.AllocHGlobal(size);
            NativeMethods.RtlMoveMemory(newPointer, m_Pointer, m_Length);

            Marshal.FreeHGlobal(m_Pointer);

            m_Pointer = newPointer;
            m_AllocSize = size;
        }

        public void Clear()
        {
            m_Length = 0;
        }

        bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // マネージ状態を破棄します (マネージ オブジェクト)
                }

                // アンマネージ リソース (アンマネージ オブジェクト) を解放

                if (m_Pointer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(m_Pointer);
                    m_Pointer = IntPtr.Zero;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ByteBuffer()
        {
            Dispose(false);
        }

        public void Add(byte[] data)
        {
            int nSize = data.Length;
            if (m_Length + nSize > m_AllocSize)
            {
                int addSize = ((nSize / initialSize) + 1) * initialSize;
                Realloc(checked (m_AllocSize + addSize));
            }
            Marshal.Copy(data, 0, m_Pointer, nSize);
            m_Length = m_Length + nSize;
        }

        public byte[] Get()
        {
            byte[] work = new byte[m_Length];
            Marshal.Copy(m_Pointer, work, 0, m_Length);
            return work;
        }

        public void Remove(int size)
        {
            byte[] work = this.Get();
            int newLength = m_Length - size;
            Marshal.Copy(work, size, m_Pointer, newLength);
            m_Length = newLength;
        }
    }
}
