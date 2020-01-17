using Dot.Tools.ETD.Validations;
using Dot.Tools.ETD.Validations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotExcelToDataTest.Factorys
{
    [TestFixture]
    public class ValidationFactoryTest
    {
        [Test]
        public void TestGetValidation()
        {
            string validationStr = "IntValue";
            IValidation validation = ValidationFactory.GetValidation(validationStr);
            Assert.AreEqual(typeof(IntValidation), validation.GetType(), $"ValidationFactoryTest::TestGetValidation->{validationStr}");

            validationStr = "IntRange{10,100}";
            validation = ValidationFactory.GetValidation(validationStr);
            Assert.AreEqual(typeof(IntRangeValidation), validation.GetType(), $"ValidationFactoryTest::TestGetValidation->{validationStr}");
            Assert.AreEqual(100, ((IntRangeValidation)validation).max, $"ValidationFactoryTest::TestGetValidation->{validationStr}");
        }
        [Test]
        public void TestGetValidations()
        {

        }
    }
}
