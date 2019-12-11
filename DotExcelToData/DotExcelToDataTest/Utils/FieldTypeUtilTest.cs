using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Utils;
using NUnit.Framework;

namespace DotExcelToDataTest.Utils
{
    [TestFixture]
    public class FieldTypeUtilTest
    {
        [Test]
        public void TestGetFieldType()
        {
            string typeStr = "int";
            FieldType fieldType = FieldTypeUtil.GetFieldType(typeStr);

            Assert.AreEqual(FieldType.Int, fieldType, $"FieldTypeUtilTest::TestGetFieldType->{typeStr}");

            typeStr = "ref<refTable0>";
            fieldType = FieldTypeUtil.GetFieldType(typeStr);
            Assert.AreEqual(FieldType.Ref, fieldType, $"FieldTypeUtilTest::TestGetFieldType->{typeStr}");

            typeStr = "array[string]";
            fieldType = FieldTypeUtil.GetFieldType(typeStr);
            Assert.AreEqual(FieldType.Array, fieldType, $"FieldTypeUtilTest::TestGetFieldType->{typeStr}");

            typeStr = "array[ref<refTable0>]";
            fieldType = FieldTypeUtil.GetFieldType(typeStr);
            Assert.AreEqual(FieldType.Array, fieldType, $"FieldTypeUtilTest::TestGetFieldType->{typeStr}");

            typeStr = "dic{ref<refTable0>,ref<refTable1>}";
            fieldType = FieldTypeUtil.GetFieldType(typeStr);
            Assert.AreEqual(FieldType.Dic, fieldType, $"FieldTypeUtilTest::TestGetFieldType->{typeStr}");

            typeStr = "dic{string,int}";
            fieldType = FieldTypeUtil.GetFieldType(typeStr);
            Assert.AreEqual(FieldType.Dic, fieldType, $"FieldTypeUtilTest::TestGetFieldType->{typeStr}");
        }

        [Test]
        public void TestGetRefName()
        {
            string typeStr = "ref<refTable0>";
            string refName = FieldTypeUtil.GetRefName(typeStr);
            Assert.AreEqual("refTable0", refName, $"FieldTypeUtilTest::TestGetRefName->{typeStr}");
        }

        [Test]
        public void TestGetArrayInnerType()
        {
            string typeStr = "array[string]";
            FieldType innerType = FieldTypeUtil.GetArrayInnerType(typeStr,out string refName);
            Assert.IsTrue(string.IsNullOrEmpty(refName), $"FieldTypeUtilTest::TestGetArrayInnerType->{typeStr}");
            Assert.AreEqual(FieldType.String, innerType, $"FieldTypeUtilTest::TestGetArrayInnerType->{typeStr}");

            typeStr = "array[ref<refTable0>]";
            innerType = FieldTypeUtil.GetArrayInnerType(typeStr, out refName);
            Assert.AreEqual("refTable0", refName, $"FieldTypeUtilTest::TestGetArrayInnerType->{typeStr}");
            Assert.AreEqual(FieldType.Ref, innerType, $"FieldTypeUtilTest::TestGetArrayInnerType->{typeStr}");
        }

        [Test]
        public void TestGetDicInnerType()
        {
            string typeStr = "dic{string,int}";
            FieldTypeUtil.GetDicInnerType(typeStr,
                out FieldType keyType, out string keyRefName,
                out FieldType valueType, out string valueRefName);
            Assert.IsTrue(string.IsNullOrEmpty(keyRefName), $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.AreEqual(FieldType.String, keyType, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.IsTrue(string.IsNullOrEmpty(valueRefName), $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.AreEqual(FieldType.Int, valueType, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");

            typeStr = "dic{ref<refTable0>,ref<refTable1>}";
            FieldTypeUtil.GetDicInnerType(typeStr,
                out keyType, out keyRefName,
                out valueType, out valueRefName);
            Assert.AreEqual("refTable0",keyRefName, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.AreEqual(FieldType.Ref, keyType, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.AreEqual("refTable1", valueRefName, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.AreEqual(FieldType.Ref, valueType, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");

            typeStr = "dic{float,ref<refTable1>}";
            FieldTypeUtil.GetDicInnerType(typeStr,
                out keyType, out keyRefName,
                out valueType, out valueRefName);
            Assert.IsTrue(string.IsNullOrEmpty(keyRefName), $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.AreEqual(FieldType.Float, keyType, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.AreEqual("refTable1", valueRefName, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
            Assert.AreEqual(FieldType.Ref, valueType, $"FieldTypeUtilTest::TestGetDicInnerType->{typeStr}");
        }
    }
}
