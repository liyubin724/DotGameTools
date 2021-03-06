﻿using Dot.Tools.ETD.Validations;
using Dot.Tools.ETD.Fields;
using NUnit.Framework;

namespace DotExcelToDataTest.Factorys
{
    [TestFixture]
    public class FieldFactroyTest
    {
        [Test]
        public void TestGetField()
        {
            int col = 1;
            string name = "test";
            string desc = "test desc";
            string type = "int";
            string platform = "c";
            string value = "11";
            string validation = "IntValue";

            AField field = FieldFactory.GetField(col, name,  type, platform, desc, value, validation);
            Assert.IsNotNull(field, "Field is Null");
            Assert.AreEqual(typeof(IntField), field.GetType());

            type = "array[ref<table0>]";

            field = FieldFactory.GetField(col, name,  type, platform, desc, value, validation);
            Assert.IsNotNull(field, "Field is Null");
            Assert.AreEqual(typeof(ArrayField), field.GetType());
        }
    }
}
