using System;
using System.IO;
using System.Data.Common;
using OracleInProcServer.Core;

namespace OracleInProcServer
{
    [Serializable]
    public class OraBFile : OraObject
    {

        internal Stream oraclebFile;

        internal OraBFile(object obj) {
            oraclebFile = (Stream)obj;
        }

        public OraBFile(OraDatabase db) {
            oraclebFile = (Stream)Activator.CreateInstance(OracleTypes.OracleBFile, db.Connection);
        }

        public OraBFile(OraDatabase db, string directoryName, string fileName) {
            oraclebFile = (Stream)Activator.CreateInstance(OracleTypes.OracleBFile, db.Connection, directoryName, fileName);
        }

        public DbConnection Connection {
            get {
                return oraclebFile.GetProperty<DbConnection>("Connection");
            }
        }

        public string DirectoryName {
            get {
                return oraclebFile.GetProperty<string>("DirectoryName");
            }
            set {
                oraclebFile.SetProperty("DirectoryName", value);
            }
        }

        public bool Exists {
            get {
                return oraclebFile.GetProperty<bool>("FileExists");
            }
        }

        public string FileName {
            get {
                return oraclebFile.GetProperty<string>("FileName");
            }
            set {
                oraclebFile.SetProperty("FileName", value);
            }
        }

        public bool IsNull {
            get {
                return oraclebFile.GetProperty<bool>("IsNull");
            }
        }

        public bool IsOpen {
            get {
                return oraclebFile.GetProperty<bool>("IsOpen");
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
                return oraclebFile.GetProperty<byte[]>("Value");
            }
            set {
                oraclebFile.Position = 0;
                oraclebFile.Write(value, 0, value.Length);
                oraclebFile.SetLength(value.Length);
            }
        }

        public object Clone() {
            return new OraBFile(oraclebFile.Invoke("Clone"));
        }

        public void Close() {
            oraclebFile.Close();
        }

        public void CloseAll() {

        }

        public int Compare(OraBFile obj, int amount, int dst_offset, int src_offset) {
            return (int)oraclebFile.Invoke("Compare", (long)src_offset, obj.oraclebFile, (long)dst_offset, (long)amount);
        }

        public void CopyToFile(string filename, int amount, int offset, int chunksize) {
            using (var writer = new FileStream(filename, FileMode.Create)) {
                var buffer = new byte[amount];
                var length = oraclebFile.Read(buffer, offset, amount);
                writer.Write(buffer, 0, length);
            }
        }

        public int MatchPos(byte[] pattern, int offset, int nth) {
            return (int)(long)oraclebFile.Invoke("Search", pattern, offset, nth);
        }

        public void Open() {
            oraclebFile.Invoke("OpenFile");
        }

        public int Read(byte[] Chunk) {
            return (int)oraclebFile.Read(Chunk, 0, Chunk.Length);
        }

        public int Read(byte[] Chunk, int chunksize) {
            return (int)oraclebFile.Read(Chunk, 0, chunksize);
        }

        protected override void Dispose(bool disposing) {
            if (oraclebFile != null) {
                oraclebFile.Dispose();
            }
        }
    }
}
