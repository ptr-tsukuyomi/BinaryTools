using System;
using System.IO;
using System.Threading.Tasks;

namespace BinaryTools
{
    public class BinaryWriter : IDisposable
    {
        private readonly Stream _stream = null;

        public bool IsDataLittleEndian { get; set; }
        private bool IsMachineLittleEndian = BitConverter.IsLittleEndian;

        public BinaryWriter(Stream stream, bool isDataLittleEndian = true)
        {
            if (!stream.CanWrite) throw new Exception("The stream specified does not support read opeeration.");

            _stream = stream;
            IsDataLittleEndian = isDataLittleEndian;
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            await _stream.WriteAsync(buffer, offset, count);
        }

        private async Task WriteMultiByteAsync(byte[] buffer, int offset, int count)
        {
            if (IsDataLittleEndian != IsMachineLittleEndian)
            {
                Array.Reverse(buffer);
            }
            await _stream.WriteAsync(buffer, offset, count);
        }

        public async Task WriteUInt16Async(UInt16 x)
        {
            byte[] data = BitConverter.GetBytes(x);
            await WriteMultiByteAsync(data, 0, data.Length);
        }
        public async Task WriteUInt32Async(UInt32 x)
        {
            byte[] data = BitConverter.GetBytes(x);
            await WriteMultiByteAsync(data, 0, data.Length);
        }
        public async Task WriteUInt64Async(UInt64 x)
        {
            byte[] data = BitConverter.GetBytes(x);
            await WriteMultiByteAsync(data, 0, data.Length);
        }

        public async Task WriteInt16Async(Int16 x)
        {
            byte[] data = BitConverter.GetBytes(x);
            await WriteMultiByteAsync(data, 0, data.Length);
        }
        public async Task WriteInt32Async(Int32 x)
        {
            byte[] data = BitConverter.GetBytes(x);
            await WriteMultiByteAsync(data, 0, data.Length);
        }
        public async Task WriteInt64Async(Int64 x)
        {
            byte[] data = BitConverter.GetBytes(x);
            await WriteMultiByteAsync(data, 0, data.Length);
        }

        public async Task WriteByteAsync(Byte x)
        {
            await WriteAsync(new byte[] { x }, 0, 1);
        }

        public async Task WriteSByteAsync(SByte x)
        {
            await WriteByteAsync(Convert.ToByte(x));
        }

        public async Task WriteBooleanAsync(Boolean x)
        {
            await WriteByteAsync(Convert.ToByte(x));
        }

        public async Task WriteStringAsync(String s, System.Text.Encoding encoding, bool nullTerminated = true)
        {
            var data = encoding.GetBytes(s);
            await WriteAsync(data, 0, data.Length);

            if (nullTerminated) await WriteByteAsync(0);
        }

        public async Task WriteSingleAsync(float x)
        {
            var data = BitConverter.GetBytes(x);
            await WriteMultiByteAsync(data, 0, data.Length);
        }

        public async Task WriteDoubleAsync(double x)
        {
            var data = BitConverter.GetBytes(x);
            await WriteMultiByteAsync(data, 0, data.Length);
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
        // ~BinaryWriter()
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
