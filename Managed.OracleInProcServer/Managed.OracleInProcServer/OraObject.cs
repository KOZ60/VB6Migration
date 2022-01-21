using System;

namespace Managed.OracleInProcServer
{
    public abstract class OraObject : IDisposable
    {
        private bool disposedValue = false;

        private void DisposeInternal(bool disposing) {
            if (!disposedValue) {
                disposedValue = true;
                Dispose(disposing);
            }
        }

        protected abstract void Dispose(bool disposing);

        public void Dispose() {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
        }
    }

}
