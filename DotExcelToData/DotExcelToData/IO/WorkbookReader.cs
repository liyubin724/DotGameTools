using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using ETDSheet = Dot.Tools.ETD.Datas.Sheet;
using ETDWorkbook = Dot.Tools.ETD.Datas.Workbook;

namespace Dot.Tools.ETD.IO
{
    public class WorkbookReader
    {
        public static Workbook ReadExcelToWorkbook(string excelPath)
        {
            string ext = Path.GetExtension(excelPath);
            using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                IWorkbook workbook = null;
                if (ext == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else
                {
                    workbook = new HSSFWorkbook(fs);
                }

                if (workbook == null || workbook.NumberOfSheets == 0)
                {
                    return null;
                }

                ETDWorkbook bookData = new ETDWorkbook(excelPath);
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    string sheetName = sheet.SheetName;
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        continue;
                    }
                    if (sheetName.StartsWith("#"))
                    {
                        continue;
                    }
                    if (!Regex.IsMatch(sheetName, SheetConst.SHEET_NAME_REGEX))
                    {
                        continue;
                    }

                    Sheet dataSheet = ReadFromSheet(sheet);
                    if(dataSheet!=null)
                    {
                        bookData.AddSheet(dataSheet);
                    }
                }
                return bookData;
            }
        }

        public static ETDSheet ReadFromSheet(ISheet sheet)
        {
            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            int firstCol = sheet.GetRow(firstRow).FirstCellNum;
            int lastCol = sheet.GetRow(firstRow).LastCellNum;

            int rowCount = lastRow - firstRow + 1;
            int colCount = lastCol - firstCol + 1;
            if (rowCount < SheetConst.MIN_ROW_COUNT)
            {
                return null;
            }
            if (colCount < SheetConst.MIN_COLUMN_COUNT)
            {
                return null;
            }
            ETDSheet sheetData = new ETDSheet(sheet.SheetName);
            ReadFieldFromSheet(sheetData, sheet);
            ReadLineFromSheet(sheetData, sheet);
            return sheetData;
        }

        private static void ReadFieldFromSheet(ETDSheet sheetData,ISheet sheet)
        {
            MethodInfo getFieldMI = typeof(FieldFactory).GetMethod("GetField", BindingFlags.Public | BindingFlags.Static);

            int firstRow = sheet.FirstRowNum;
            int firstCol = sheet.GetRow(firstRow).FirstCellNum;
            int lastCol = sheet.GetRow(firstRow).LastCellNum;

            int colCount = lastCol - firstCol + 1;
            for (int i = 1; i < colCount; i++)
            {
                List<string> values = new List<string>();
                for (int j = 0; j < SheetConst.MIN_ROW_COUNT; j++)
                {
                    IRow row = sheet.GetRow(firstRow + j);
                    ICell cell = row.GetCell(i);
                    if (j == 0)
                    {
                        if (cell == null)
                        {
                            break;
                        }
                        else
                        {
                            string content = SheetConst.GetCellStringValue(cell);
                            if (string.IsNullOrEmpty(content) || content.StartsWith("#"))
                            {
                                break;
                            }
                        }
                    }
                    values.Add(SheetConst.GetCellStringValue(cell));
                }
                if (values.Count == SheetConst.MIN_ROW_COUNT + 1)
                {
                    object[] datas = new object[values.Count + 1];
                    datas[0] = firstCol + i;
                    Array.Copy(values.ToArray(), 0, datas, 1, values.Count);

                    AFieldData field = (AFieldData)getFieldMI.Invoke(null, datas);
                    sheetData.AddField(field);
                }
            }
        }

        private static void ReadLineFromSheet(ETDSheet sheetData,ISheet sheet)
        {
            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            int firstCol = sheet.GetRow(firstRow).FirstCellNum;

            bool isStart = false;
            for (int i = SheetConst.MIN_ROW_COUNT; i < lastRow; i++)
            {
                IRow row = sheet.GetRow(i);
                ICell cell = row.GetCell(firstCol);
                if(!isStart)
                {
                    if(cell == null)
                    {
                        continue;
                    }else
                    {
                        string cellContent = SheetConst.GetCellStringValue(cell);
                        if (cellContent == SheetConst.ROW_START_FLAG)
                        {
                            isStart = true;
                        }else
                        {
                            continue;
                        }
                    }
                }else
                {
                    if(cell!=null)
                    {
                        string cellContent = SheetConst.GetCellStringValue(cell);
                        if (cellContent == SheetConst.ROW_END_FLAG)
                        {
                            break;
                        }
                    }
                }

                LineCell line = new LineCell(i);
                int fieldCount = sheetData.FieldCount;
                for (int j = 0; j < fieldCount; ++j)
                {
                    AFieldData fieldData = sheetData.GetFieldByIndex(j);
                    ICell valueCell = row.GetCell(fieldData.col);
                    line.AddCell(fieldData.col, SheetConst.GetCellStringValue(valueCell));
                }
                sheetData.AddLine(line);
            }
        }

    }
}
