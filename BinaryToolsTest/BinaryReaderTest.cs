using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryToolsTest
{
    [TestClass]
    public class BinaryReaderTest
    {
        [TestMethod]
        public void TestReadBigEndianUInt()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var data = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4 };
                var memoryStream = new System.IO.MemoryStream(data);

                using (var binaryReader = new BinaryTools.BinaryReader(memoryStream, false))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left64 = await binaryReader.ReadUInt64Async();
                    Assert.AreEqual(left64, 0x0001000200030004UL);

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left32 = await binaryReader.ReadUInt32Async();
                    Assert.AreEqual(left32, 0x00010002U);

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left16 = await binaryReader.ReadUInt16Async();
                    Assert.AreEqual(left16, (System.UInt16)0x0001);
                }
            }).Wait();
        }

        [TestMethod]
        public void TestReadLittleEndianUInt()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var data = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4 };
                var memoryStream = new System.IO.MemoryStream(data);

                using (var binaryReader = new BinaryTools.BinaryReader(memoryStream, true))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left64 = await binaryReader.ReadUInt64Async();
                    Assert.AreEqual(left64, 0x0400030002000100UL);

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left32 = await binaryReader.ReadUInt32Async();
                    Assert.AreEqual(left32, 0x02000100U);

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left16 = await binaryReader.ReadUInt16Async();
                    Assert.AreEqual(left16, (System.UInt16)0x0100);
                }
            }).Wait();
        }

        // Int
        [TestMethod]
        public void TestReadBigEndianInt()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var data = new byte[] { 0x81, 0, 0x82, 0, 0x83, 0, 0x84, 0 };
                var memoryStream = new System.IO.MemoryStream(data);

                using (var binaryReader = new BinaryTools.BinaryReader(memoryStream, false))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left64 = await binaryReader.ReadInt64Async();
                    Assert.AreEqual(left64, -0x7EFF7DFF7CFF7C00L);

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left32 = await binaryReader.ReadInt32Async();
                    Assert.AreEqual(left32, -0x7EFF7E00);

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left16 = await binaryReader.ReadInt16Async();
                    Assert.AreEqual(left16, (System.Int16)(-0x7F00));
                }
            }).Wait();
        }

        [TestMethod]
        public void TestReadLittleEndianInt()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var data = new byte[] { 0, 0x81, 0, 0x82, 0, 0x83, 0, 0x84 };
                var memoryStream = new System.IO.MemoryStream(data);

                using (var binaryReader = new BinaryTools.BinaryReader(memoryStream, true))
                {
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left64 = await binaryReader.ReadInt64Async();
                    Assert.AreEqual(left64, -0x7BFF7CFF7DFF7F00L);

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left32 = await binaryReader.ReadInt32Async();
                    Assert.AreEqual(left32, -0x7DFF7F00);

                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    var left16 = await binaryReader.ReadInt16Async();
                    Assert.AreEqual(left16, (System.Int16)(-0x7F00));
                }
            }).Wait();
        }

        [TestMethod]
        public void TestReadString()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var memoryStream = new System.IO.MemoryStream();

                var data1 = "おほー！₍ ᐢ. ̫ .ᐢ ₎";
                var byteData1 = System.Text.Encoding.UTF8.GetBytes(data1);
                var data2 = "（・▽・💠）";
                var byteData2 = System.Text.Encoding.UTF8.GetBytes(data2);
                var spacer = new byte[] { 0 };
                memoryStream.Write(byteData1, 0, byteData1.Length);
                memoryStream.Write(spacer, 0, spacer.Length);
                memoryStream.Write(byteData2, 0, byteData2.Length);

                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);

                using (var binaryReader = new BinaryTools.BinaryReader(memoryStream, true))
                {
                    var string1 = await binaryReader.ReadStringAsync(System.Text.Encoding.UTF8);
                    Assert.AreEqual(string1, data1);
                    var string2 = await binaryReader.ReadStringAsync(System.Text.Encoding.UTF8);
                    Assert.AreEqual(string2, data2);
                }
            }).Wait();
        }
    }
}
