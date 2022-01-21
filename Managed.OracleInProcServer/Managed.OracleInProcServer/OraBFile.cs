using System;
using System.IO;
using Managed.OracleInProcServer.Core;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    [Serializable]
    public class OraBFile : OraObject
    {

        internal OracleBFile oraclebFile;

        internal OraBFile(OracleBFile obj) {
            oraclebFile = obj;
        }

        public OraBFile(OraDatabase db) {
            oraclebFile = new OracleBFile(db.Connection);
        }

        public OraBFile(OraDatabase db, string directoryName, string fileName) {
            oraclebFile = new OracleBFile(db.Connection, directoryName, fileName);
        }

        public OracleConnection Connection {
            get {
                return oraclebFile.Connection;
            }
        }

        public string DirectoryName {
            get {
                return oraclebFile.DirectoryName;
            }
            set {
                oraclebFile.DirectoryName = value;
            }
        }

        public bool Exists {
            get {
                return oraclebFile.FileExists;
            }
        }

        public string FileName {
            get {
                return oraclebFile.FileName;
            }
            set {
                oraclebFile.FileName = value;
            }
        }

        public bool IsNull {
            get {
                return oraclebFile.IsNull;
            }
        }

        public bool IsOpen {
            get {
                return oraclebFile.IsOpen;
            }
        }

        public long Offset {
            get {
                return oraclebFile.Position; 
            }
            set {
                oraclebFile.Position = value;
            }
        }

        public int PollingAmount { get; set; }

        public long Size {
            get {
                return oraclebFile.Length;
            }
        }

        public byte[] Value {
            get {
                return oraclebFile.Value;
            }
            set {
                oraclebFile.Position = 0;
                oraclebFile.Write(value, 0, value.Length);
                oraclebFile.SetLength(value.Length);
            }
        }

        public object Clone() {
            return new OraBFile((OracleBFile)oraclebFile.Clone());
        }

        public void Close() {
            oraclebFile.Close();
        }

        public void CloseAll() {

        }

        public int Compare(OraBFile obj, long amount, long dst_offset, long src_offset) {
            return oraclebFile.Compare( src_offset, obj.oraclebFile, dst_offset, amount);
        }

        public void CopyToFile(string filename, int amount, int offset, int chunksize) {
            using (var writer = new FileStream(filename, FileMode.Create)) {
                var buffer = new byte[amount];
                var length = oraclebFile.Read(buffer, offset, amount);
                writer.Write(buffer, 0, length);
            }
        }

        public long MatchPos(byte[] pattern, long offset, long nth) {
            return oraclebFile.Search(pattern, offset, nth);
        }

        public void Open() {
            oraclebFile.OpenFile();
        }

        public int Read(byte[] Chunk) {
            return oraclebFile.Read(Chunk, 0, Chunk.Length);
        }

        public int Read(byte[] Chunk, int chunksize) {
            return oraclebFile.Read(Chunk, 0, chunksize);
        }

        protected override void Dispose(bool disposing) {
            if (oraclebFile != null) {
                oraclebFile.Dispose();
            }
        }
    }
}
