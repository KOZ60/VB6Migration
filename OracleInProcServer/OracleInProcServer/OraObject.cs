using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace OracleInProcServer
{
    public abstract class OraObject : IDisposable
    {
        public DbProviderFactory Factory {
            get {
                return OraClient.Factory;
            }
        }

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
