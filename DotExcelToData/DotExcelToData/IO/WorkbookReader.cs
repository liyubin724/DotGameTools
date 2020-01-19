using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;
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
    internal class WorkbookReader
    {
        private static EIContext context = null;
        internal static void InitReader(EIContext context)
        {
            WorkbookReader.context = context;
        }

        internal static void DestroyReader()
        {
            context = null;
        }

        private static void Log(LogType type, int logID, params object[] datas)
        {
            context.GetObject<LogHandler>()?.Log(type, logID, datas);
        }

        internal static Workbook ReadExcelToWorkbook(string excelPath)
        {
            string ext = Path.GetExtension(excelPath).ToLower();
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
                    Log(LogType.Warning, LogConst.LOG_WORKBOOK_EMPTY, excelPath);
                    return null;
                }

                Log(LogType.Info, LogConst.LOG_WORKBOOK_START, excelPath);

                ETDWorkbook bookData = new ETDWorkbook(excelPath);
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    Log(LogType.Info, LogConst.LOG_WARP_LINE);

                    ISheet sheet = workbook.GetSheetAt(i);
                    string sheetName = sheet.SheetName;
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        Log(LogType.Warning, LogConst.LOG_SHEET_NAME_NULL, i);

                        continue;
                    }
                    if (sheetName.StartsWith("#"))
                    {
                        Log(LogType.Info, LogConst.LOG_IGNORE_SHEET, sheetName);
                        continue;
                    }
                    if (!Regex.IsMatch(sheetName, SheetConst.SHEET_NAME_REGEX))
                    {
                        Log(LogType.Error, LogConst.LOG_SHEET_NAME_NOT_MATCH, sheetName, SheetConst.SHEET_NAME_REGEX);
                        continue;
                    }

                    Sheet dataSheet = ReadFromSheet(sheet);
                    if(dataSheet!=null)
                    {
                        bookData.AddSheet(dataSheet);
                    }

                    Log(LogType.Info, LogConst.LOG_WARP_LINE);
                }

                Log(LogType.Info, LogConst.LOG_WORKBOOK_END, excelPath);

                return bookData;
            }
        }

        public static ETDSheet ReadFromSheet(ISheet sheet)
        {
            Log(LogType.Info, LogConst.LOG_SHEET_START, sheet.SheetName);

            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            int firstCol = sheet.GetRow(firstRow).FirstCellNum;
            int lastCol = sheet.GetRow(firstRow).LastCellNum;

            int rowCount = lastRow - firstRow + 1;
            int colCount = lastCol - firstCol + 1;
            if (rowCount < SheetConst.MIN_ROW_COUNT)
            {
                Log(LogType.Info, LogConst.LOG_SHEET_ROW_LESS, rowCount, SheetConst.MIN_ROW_COUNT);
                return null;
            }
            if (colCount < SheetConst.MIN_COLUMN_COUNT)
            {
                Log(LogType.Info, LogConst.LOG_SHEET_COL_LESS, colCount, SheetConst.MIN_COLUMN_COUNT);
                return null;
            }

            ETDSheet sheetData = new ETDSheet(sheet.SheetName);
            ReadFieldFromSheet(sheetData, sheet);
            ReadLineFromSheet(sheetData, sheet);

            Log(LogType.Info, LogConst.LOG_SHEET_END, sheet.SheetName);

            return sheetData;
        }

        private static void ReadFieldFromSheet(ETDSheet sheetData,ISheet sheet)
        {
            MethodInfo getFieldMI = typeof(FieldFactory).GetMethod("GetField", BindingFlags.Public | BindingFlags.Static);

            Log(LogType.Info, LogConst.LOG_SHEET_FIELD_START);

            int firstRow = sheet.FirstRowNum;
            int firstCol = sheet.GetRow(firstRow).FirstCellNum;
            int lastCol = sheet.GetRow(firstRow).LastCellNum;

            bool isFoundIDCol = false;
            for(int i = firstCol; i<=lastCol;++i)
            {
                IRow nameRow = sheet.GetRow(firstRow);
                string nameContent = SheetConst.GetCellStringValue(nameRow.GetCell(i));
                if(string.IsNullOrEmpty(nameContent))
                {
                    Log(LogType.Warning, LogConst.LOG_SHEET_FIELD_NAME_NULL);
                    continue;
                }
                if(nameContent.StartsWith("#") || nameContent.StartsWith("_"))
                {
                    Log(LogType.Info, LogConst.LOG_SHEET_FIELD_IGNORE, nameContent);
                    continue;
                }
                if(!isFoundIDCol)
                {
                    if(nameContent == SheetConst.ID_FIELD_NAME)
                    {
                        isFoundIDCol = true;
                    }else
                    {
                        continue;
                    }
                }

                List<string> values = new List<string>();
                for (int j = 0; j < SheetConst.MIN_ROW_COUNT; j++)
                {
                    IRow row = sheet.GetRow(firstRow + j);
                    values.Add(SheetConst.GetCellStringValue(row.GetCell(i)));
                }
                if (values.Count == SheetConst.MIN_ROW_COUNT)
                {
                    object[] datas = new object[values.Count + 1];
                    datas[0] = i;
                    Array.Copy(values.ToArray(), 0, datas, 1, values.Count);

                    Log(LogType.Info, LogConst.LOG_SHEET_FIELD_CREATE, i);

                    AFieldData field = (AFieldData)getFieldMI.Invoke(null, datas);
                    sheetData.AddField(field);

                    Log(LogType.Verbose, LogConst.LOG_SHEET_FIELD_DETAIL, field.ToString());
                }
            }
            Log(LogType.Info, LogConst.LOG_SHEET_FIELD_END);
        }

        private static void ReadLineFromSheet(ETDSheet sheetData,ISheet sheet)
        {
            Log(LogType.Info, LogConst.LOG_SHEET_LINE_START);

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

                Log(LogType.Info, LogConst.LOG_SHEET_LINE_CREATE,i);

                SheetLine line = new SheetLine(i);
                int fieldCount = sheetData.FieldCount;
                for (int j = 0; j < fieldCount; ++j)
                {
                    AFieldData fieldData = sheetData.GetFieldByIndex(j);
                    ICell valueCell = row.GetCell(fieldData.col);
                    line.AddCell(fieldData.col, SheetConst.GetCellStringValue(valueCell));
                }
                sheetData.AddLine(line);

                Log(LogType.Verbose, LogConst.LOG_SHEET_LINE_DETAIL,line.ToString());
            }

            Log(LogType.Info, LogConst.LOG_SHEET_LINE_END);
        }

    }
}
