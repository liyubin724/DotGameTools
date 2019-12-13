using Dot.Tools.ETD.Datas;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotExcelToDataTest.Datas
{
    [TestFixture]
    public class TestWorkbook
    {
        [Test]
        public void TestReadWorkbook()
        {
            string excelPath = @"D:\WorkSpace\DotGameProject\DotGameTools\cofing.xlsx";
            Workbook book = new Workbook();
            bool result = book.LoadExcel(excelPath, out string msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(book);
            Assert.AreEqual(1, book.sheets.Count);
            Assert.AreEqual("Sheet1", book.sheets[0].Name);

            bool verifyResult = book.Verify(out msg);
            if(!verifyResult)
            {
                File.WriteAllText("D:/result.txt", msg);
            }
            Assert.IsTrue(verifyResult);
        }
    }
}
