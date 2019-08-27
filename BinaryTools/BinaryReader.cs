using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryTools
{
    public class DataLengthNotEnoughException : Exception
    {
        public DataLengthNotEnoughException(string message) : base(message)
        {
        }
    }

    public class BinaryReader : IDisposable
    {
        private readonly Stream _stream = null;
        
        public bool IsDataLittleEndian { get; set; }
        private bool IsMachineLittleEndian = BitConverter.IsLittleEndian;

        public BinaryReader(Stream stream, bool isDataLittleEndian = true)
        {
            if (!stream.CanRead) throw new Exception("The stream specified does not support read opeeration.");

            _stream = stream;
            IsDataLittleEndian = isDataLittleEndian;
        }

        public async Task<int> ReadAsync(byte[] buffer, int index, int count)
        {
            return await _stream.ReadAsync(buffer, index, count);
        }

        private async Task GetBytesAndExpandTo8Bytes(byte[] buffer, int readLength)
        {
            if (buffer.Length < 8)
                throw new Exception("size of specified buffer is less than 8 bytes.");
            if (readLength < 0 || readLength > 8)
                throw new Exception("specified readLength is more than 8 [bytes]");

            var offset = IsDataLittleEndian ? 0 : 8 - readLength;
            if (await ReadAsync(buffer, offset, readLength) != readLength)
                throw new DataLengthNotEnoughException("Can not read enough data.");
        }

        public async Task<UInt64> ReadUIntAsync(int lengthInByte)
        {
            if (!(lengthInByte == 2 || lengthInByte == 4 || lengthInByte == 8))
                throw new Exception("lengthInByte must be 2, 4 or 8.");

            byte[] data = new byte[8];
            await GetBytesAndExpandTo8Bytes(data, lengthInByte);

            if (IsDataLittleEndian != IsMachineLittleEndian) Array.Reverse(data);
            return BitConverter.ToUInt64(data, 0);
        }

        public async Task<UInt16> ReadUInt16Async()
        {
            return (UInt16)await ReadUIntAsync(2);
        }
        public async Task<UInt32> ReadUInt32Async()
        {
            return (UInt32)await ReadUIntAsync(4);
        }
        public async Task<UInt64> ReadUInt64Async()
        {
            return (UInt64)await ReadUIntAsync(8);
        }

        public async Task<Int64> ReadIntAsync(int lengthInByte)
        {
            if (!(lengthInByte == 2 || lengthInByte == 4 || lengthInByte == 8))
                throw new Exception("lengthInByte must be 2, 4 or 8.");

            byte[] data = new byte[8];
            await GetBytesAndExpandTo8Bytes(data, lengthInByte);

            if (IsDataLittleEndian != IsMachineLittleEndian) Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }

        public async Task<Int16> ReadInt16Async()
        {
            return (Int16)await ReadUIntAsync(2);
        }
        public async Task<Int32> ReadInt32Async()
        {
            return (Int32)await ReadUIntAsync(4);
        }
        public async Task<Int64> ReadInt64Async()
        {
            return (Int64)await ReadUIntAsync(8);
        }

        public async Task<Byte> ReadByteAsync()
        {
            byte[] data = new byte[1];
            if (await ReadAsync(data, 0, 1) != 1)
                throw new DataLengthNotEnoughException("cannot read enough data");
            return data[0];
        }

        public async Task<SByte> ReadSByteAsync()
        {
            return Convert.ToSByte(await ReadByteAsync());
        }

        public async Task<Boolean> ReadBooleanAsync()
        {
            return Convert.ToBoolean(await ReadByteAsync());
        }

        public async Task<string> ReadStringAsync(System.Text.Encoding encoding)
        {
            var buffers = new System.Collections.Generic.List<byte[]>();
            var length = 0;

            while (true)
            {
                var buffer = new byte[256];
                int read = await ReadAsync(buffer, 0, buffer.Length);
                if (read == 0) break;

                buffers.Add(buffer);

                var index = Array.IndexOf(buffer, (byte)0);
                if (index != -1)
                {
                    length += index;
                    _stream.Seek(-(read - index - 1), SeekOrigin.Current);
                    break;
                } else
                {
                    length += read;
                }
            }

            if (buffers.Count == 0) throw new DataLengthNotEnoughException("Cannot read enough data.");

            return encoding.GetString(buffers.Aggregate((a, b) => { return a.Concat(b).ToArray(); }), 0, length);
        }

        public async Task<string> ReadStringAsync(long length, System.Text.Encoding encoding)
        {
            var buffer = new byte[length];
            await ReadAsync(buffer, 0, buffer.Length);
            return encoding.GetString(buffer);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    //_stream.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~BinaryReader()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
