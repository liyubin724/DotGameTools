using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Factorys;
using Dot.Tools.ETD.Fields;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotExcelToDataTest.Fields
{
    [TestFixture]
    public class FieldTest
    {
        [Test]
        public void TestArrayField()
        {
            int col = 1;
            string name = "test";
            string desc = "test desc";
            string type = "array[int]";
            string platform = "c";
            string value = "11";
            string validation = "IntValue";

            AField field = FieldFactory.GetField(col, name, desc,  platform, type, value, validation);
            Assert.AreEqual(typeof(ArrayField), field.GetType());

            CellContent content = new CellContent()
            {
                col = 1,
                row = 1,
                value = "[1,2,3,4]",
            };

            var data = field.GetValue(content);
            Assert.IsNotNull(data);
            Assert.AreEqual(typeof(List<int>), data.GetType());
            Assert.AreEqual(1, ((List<int>)data)[0]);
            Assert.AreEqual(4, ((List<int>)data)[3]);
        }
    }
}
