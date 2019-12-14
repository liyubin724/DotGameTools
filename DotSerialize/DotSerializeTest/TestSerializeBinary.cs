using Dot.Serialize.Binary;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotSerializeTest
{
    public enum BinaryEnum
    {
        None = 0,
        Max ,
        Min ,
    }
    [Serializable]
    public class BinarySerializeChildData
    {
        public string strValue = "Just";
        public List<int> intList = new List<int>() { 1, 2, 3, 4, 5 };
    }
    [Serializable]
    public class BinarySerializeData
    {
        public int intValue = 10;
        public float floatValue = 11;
        public string stringValue = "Just for test";
        public bool boolValue = true;

        public BinaryEnum binaryEnum = BinaryEnum.Max;

        public BinarySerializeChildData childData = new BinarySerializeChildData();
    }

    [TestFixture]
    public class TestSerializeBinary
    {
        [Test]
        public void TestWriter()
        {
            string filePath = "D:/binary.bytes";
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            BinarySerializeData data = new BinarySerializeData();
            BinarySerializeWriter.WriteToBinary<BinarySerializeData>(filePath, data);

            Assert.IsTrue(File.Exists(filePath));

        }

        [Test]
        public void TestReader()
        {
            TestWriter();

            string filePath = "D:/binary.bytes";
            Assert.IsTrue(File.Exists(filePath));

            var data = BinarySerializeReader.ReadFromBinary<BinarySerializeData>(filePath);
            Assert.IsNotNull(data);
            Assert.AreEqual(10, data.intValue);
            Assert.IsNotNull(data.childData);
            Assert.AreEqual("Just", data.childData.strValue);
            Assert.AreEqual(1, data.childData.intList[0]);
        }
    }
}
