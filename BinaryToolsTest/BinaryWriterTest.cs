using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BinaryToolsTest
{
    [TestClass]
    public class BinaryWriterTest
    {
        [TestMethod]
        public void TestWriteBigEndianUInt()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var UInt16_data = (UInt16)0x8765;
                var UInt32_data = 0x87654321;
                var UInt64_data = 0x0123456789ABCDEFUL;

                var result = new byte[8];

                var memoryStream = new System.IO.MemoryStream(result);

                using (var binaryWriter = new BinaryTools.BinaryWriter(memoryStream, false))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteUInt16Async(UInt16_data);
                    Assert.IsTrue(result.Take(2).SequenceEqual(new byte[] { 0x87, 0x65 }));

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteUInt32Async(UInt32_data);
                    Assert.IsTrue(result.Take(4).SequenceEqual(new byte[] { 0x87, 0x65, 0x43, 0x21 }));

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteUInt64Async(UInt64_data);
                    Assert.IsTrue(result.Take(8).SequenceEqual(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF }));
                }
            }).Wait();
        }

        [TestMethod]
        public void TestWriteLittleEndianUInt()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var UInt16_data = (UInt16)0x8765;
                var UInt32_data = 0x87654321;
                var UInt64_data = 0x0123456789ABCDEFUL;

                var result = new byte[8];

                var memoryStream = new System.IO.MemoryStream(result);

                using (var binaryWriter = new BinaryTools.BinaryWriter(memoryStream, true))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteUInt16Async(UInt16_data);
                    Assert.IsTrue(result.Take(2).SequenceEqual(new byte[] { 0x87, 0x65 } .Reverse()));

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteUInt32Async(UInt32_data);
                    Assert.IsTrue(result.Take(4).SequenceEqual(new byte[] { 0x87, 0x65, 0x43, 0x21 } .Reverse()));

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteUInt64Async(UInt64_data);
                    Assert.IsTrue(result.Take(8).SequenceEqual(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF } .Reverse()));
                }
            }).Wait();
        }

        // Int
        [TestMethod]
        public void TestWriteBigEndianInt()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var Int16_data = (Int16)(-0x789B);
                var Int32_data = -0x789ABCDD;
                var Int64_data = 0x0123456789ABCDEFL;

                var result = new byte[8];

                var memoryStream = new System.IO.MemoryStream(result);

                using (var binaryWriter = new BinaryTools.BinaryWriter(memoryStream, false))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteInt16Async(Int16_data);
                    Assert.IsTrue(result.Take(2).SequenceEqual(new byte[] { 0x65, 0x87 } .Reverse()));

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteInt32Async(Int32_data);
                    Assert.IsTrue(result.Take(4).SequenceEqual(new byte[] { 0x23, 0x43, 0x65, 0x87 } .Reverse()));

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteInt64Async(Int64_data);
                    Assert.IsTrue(result.Take(8).SequenceEqual(new byte[] { 0xEF, 0xCD, 0xAB, 0x89, 0x67, 0x45, 0x23, 0x01 } .Reverse()));
                }
            }).Wait();
        }

        [TestMethod]
        public void TestWriteLittleEndianInt()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var Int16_data = (Int16)(-0x789B);
                var Int32_data = -0x789ABCDD;
                var Int64_data = 0x0123456789ABCDEFL;

                var result = new byte[8];

                var memoryStream = new System.IO.MemoryStream(result);

                using (var binaryWriter = new BinaryTools.BinaryWriter(memoryStream, true))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteInt16Async(Int16_data);
                    Assert.IsTrue(result.Take(2).SequenceEqual(new byte[] { 0x65, 0x87 }));

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteInt32Async(Int32_data);
                    Assert.IsTrue(result.Take(4).SequenceEqual(new byte[] { 0x23, 0x43, 0x65, 0x87 }));

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    await binaryWriter.WriteInt64Async(Int64_data);
                    Assert.IsTrue(result.Take(8).SequenceEqual(new byte[] { 0xEF, 0xCD, 0xAB, 0x89, 0x67, 0x45, 0x23, 0x01 }));
                }
            }).Wait();
        }

        [TestMethod]
        public void TestWriteString()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var memoryStream = new System.IO.MemoryStream();

                var data1 = "おほー！₍ ᐢ. ̫ .ᐢ ₎";
                var byteData1 = System.Text.Encoding.UTF8.GetBytes(data1);
                var data2 = "（・▽・💠）";
                var byteData2 = System.Text.Encoding.UTF8.GetBytes(data2);
                var spacer = new byte[] { 0 };

                using (var binaryWriter = new BinaryTools.BinaryWriter(memoryStream, true))
                {
                    await binaryWriter.WriteStringAsync(data1, System.Text.Encoding.UTF8);
                    await binaryWriter.WriteStringAsync(data2, System.Text.Encoding.UTF8);

                    var length = memoryStream.Position;
                    var byteData = new byte[length];
                    memoryStream.Position = 0;
                    memoryStream.Read(byteData, 0, (int)length);

                    var correntByteData = byteData1.Concat(spacer).Concat(byteData2).Concat(spacer).ToArray();
                    Assert.IsTrue(correntByteData.SequenceEqual(byteData));
                }
            }).Wait();
        }
    }
}
