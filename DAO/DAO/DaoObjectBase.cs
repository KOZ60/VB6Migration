using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// DAO 互換ライブラリの基底クラス
    /// </summary>
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class DaoObjectBase : MarshalByRefObject, IDisposable
    {
        bool _IsDisposed = false;

        /// <summary>
        /// このクラスによって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。</param>
        protected abstract void Dispose(bool disposing);

        private void DisposeInternal(bool disposing) {
            if (!_IsDisposed) {
                _IsDisposed = true;
                Dispose(disposing);
            }
        }

        /// <summary>
        /// このオブジェクトが Dispose されたかどうかを取得します。
        /// </summary>
        public bool IsDisposed {
            get {
                return _IsDisposed;
            }
        }

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        public void Close() {
            this.Dispose();
        }

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        public void Dispose() {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ファイナライザ
        /// </summary>
        ~DaoObjectBase() {
            DisposeInternal(true);
        }
    }
}
