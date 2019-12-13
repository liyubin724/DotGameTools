﻿using Dot.Tools.ETD;
using Dot.Tools.ETD.Datas;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotExcelToDataTest
{
    [TestFixture]
    public class TestExcelReader
    {
        [Test]
        public void TestReadExcel()
        {
            string excelPath = @"E:\DotGameProject\DotGameTools\cofing.xlsx";
            Workbook book = ExcelReader.ReadExcel(excelPath,out string msg);
            Assert.IsNotNull(book);
            Assert.AreEqual(1, book.sheets.Count);
            Assert.AreEqual("Sheet1", book.sheets[0].Name);

        }

        [Test]
        public void TestWorkbook()
        {
            string excelPath = @"E:\DotGameProject\DotGameTools\cofing.xlsx";
            Workbook book = new Workbook();
            bool result = book.LoadExcel(excelPath, out string msg);
            Assert.IsTrue(result);
            Assert.IsNotNull(book);
            Assert.AreEqual(1, book.sheets.Count);
            Assert.AreEqual("Sheet1", book.sheets[0].Name);

            bool verifyResult = book.Verify(out msg);
            Assert.IsTrue(verifyResult);
        }
    }
}