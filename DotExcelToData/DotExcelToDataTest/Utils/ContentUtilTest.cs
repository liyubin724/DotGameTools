using Dot.Tools.ETD.Utils;
using NUnit.Framework;

namespace DotExcelToDataTest.Utils
{
    [TestFixture]
    public class ContentUtilTest
    {
        [Test]
        public void TestSplitContent()
        {
            string str = "a;b;c;";
            string[] strArr = ContentUtil.SplitContent(str, new char[] { ';' });
            Assert.IsNotNull(strArr, "ContentUtilTest::TestSplitContent->Array is null.str = " + str);
            Assert.AreEqual(3, strArr.Length);

            str = "a;b\\;c;";
            strArr = ContentUtil.SplitContent(str, new char[] { ';' });
            Assert.IsNotNull(strArr, "ContentUtilTest::TestSplitContent->Array is null.str = " + str);
            Assert.AreEqual(2, strArr.Length);
            Assert.AreEqual(@"b;c", strArr[1]);


            str = "a,1;b,2;c;3";
            strArr = ContentUtil.SplitContent(str, new char[] { ';' ,','});
            Assert.IsNotNull(strArr, "ContentUtilTest::TestSplitContent->Array is null.str = " + str);
            Assert.AreEqual(6, strArr.Length);
            Assert.AreEqual("a", strArr[0]);

            str = @"a,test\,11;b,2;c;3";
            strArr = ContentUtil.SplitContent(str, new char[] { ';', ',' });
            Assert.IsNotNull(strArr, "ContentUtilTest::TestSplitContent->Array is null.str = " + str);
            Assert.AreEqual(6, strArr.Length);
            Assert.AreEqual(@"test,11", strArr[1]);
        }
    }
}
