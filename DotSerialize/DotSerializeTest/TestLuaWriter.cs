using Dot.Serialize.Lua;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

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
        public int[] intArray = new int[] { 5, 6, 7 };
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

            string luaStr = LuaSerializeWriter.WriteToLua(intList);
            File.WriteAllText("D:/list-lua.txt", luaStr);
        }
        [Test]
        public void TestDicToLua()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("T", 1);
            dic.Add("D", 2);
            dic.Add("S", 3);
            dic.Add("B", 4);

            string luaStr = LuaSerializeWriter.WriteToLua(dic);
            File.WriteAllText("D:/dic-lua.txt", luaStr);
        }

        [Test]
        public void TestClassToLua()
        {
            TestLuaSerializeData data = new TestLuaSerializeData();
            string luaStr = LuaSerializeWriter.WriteToLua(data,"test",true,false);
            File.WriteAllText("D:/obj-lua.txt", luaStr);
        }
    }
}
