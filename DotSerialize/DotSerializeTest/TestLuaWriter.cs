using Dot.Serialize.Lua;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotSerializeTest
{
    public class TestData
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

        public InnerData innerData = new InnerData();
    }

    public class InnerData
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

            LuaWriter.WriteToLua("D:/lua.txt", intList);
        }
        [Test]
        public void TestDicToLua()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("T", 1);
            dic.Add("D", 2);
            dic.Add("S", 3);
            dic.Add("B", 4);

            LuaWriter.WriteToLua("D:/lua.txt", dic);
        }

        [Test]
        public void TestClassToLua()
        {
            TestData data = new TestData();
           
            LuaWriter.WriteToLua("D:/lua.txt", data);
        }
    }
}
