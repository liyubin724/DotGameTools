using Dot.Serialize.Lua;
using NUnit.Framework;
using System.Collections.Generic;

namespace DotSerializeTest
{
    public class TestLuaSerializeData
    {
        public List<int> intList = new List<int>()
        {
            1,2,4,5,6,7,
        };
        public Dictionary<string, int> dic = new Dictionary<string, int>()
        {
            {"ss",0 },
            {"FFFF",123 },
        };

        public float floatValue = 1.30f;
        public string testStr = "test";

        public TestLuaSerializeChildData childData = new TestLuaSerializeChildData();
    }

    public class TestLuaSerializeChildData
    {
        public Dictionary<int, string> dic = new Dictionary<int, string>()
        {
            {1,"test" },
            {2,"default" },
        };
        public bool boolValue = false;
    }

    [TestFixture]
    public class TestLuaWriter
    {
        [Test]
        public void TestListToLua()
        {
            List<int> intList = new List<int>();
            intList.AddRange(new int[] { 1, 2, 3, 4, 4 });

            LuaSerializeWriter.WriteToLua("D:/lua.txt", intList);
        }
        [Test]
        public void TestDicToLua()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("T", 1);
            dic.Add("D", 2);
            dic.Add("S", 3);
            dic.Add("B", 4);

            LuaSerializeWriter.WriteToLua("D:/lua.txt", dic);
        }

        [Test]
        public void TestClassToLua()
        {
            TestLuaSerializeData data = new TestLuaSerializeData();
            LuaSerializeWriter.WriteToLua("D:/lua.txt", data);
        }
    }
}
