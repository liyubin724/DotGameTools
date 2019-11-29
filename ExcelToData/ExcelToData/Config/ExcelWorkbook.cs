using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Dot.Core.Config
{
    public static class ExcelWorkbook
    {
        public static readonly int MIN_EXCEL_ROW_COUNT = 6;

        public static ConfigWorkbookData ParseWorkbook(string excelPath)
        {
             if(string.IsNullOrEmpty(excelPath))
            {
                DebugLogger.Error("");
                return null;
            }
             if(!File.Exists(excelPath))
            {
                DebugLogger.Error("");
                return null;
            }

            string extension = Path.GetExtension(excelPath).ToLower();
            if(string.IsNullOrEmpty(extension) || (extension!=".xlsx"&&extension!=".xls"))
            {
                DebugLogger.Error("");
                return null;
            }

            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            IWorkbook excelWorkbook = null;
            if(extension == ".xlsx")
            {
                excelWorkbook = new XSSFWorkbook(fs);
            }else if(extension == ".xls")
            {
                excelWorkbook = new HSSFWorkbook(fs);
            }
            if(excelWorkbook == null)
            {
                DebugLogger.Error("");
                return null;
            }
            int sheetCount = excelWorkbook.NumberOfSheets;
            if(sheetCount<=0)
            {
                DebugLogger.Error("");
                return null;
            }

            ConfigWorkbookData workbookData = new ConfigWorkbookData();
            workbookData.name = excelPath;
            workbookData.sheets = new ConfigSheetData[sheetCount];
            for(int i =0;i<sheetCount;i++)
            {
                ISheet excelSheet = excelWorkbook.GetSheetAt(i);
                if(excelSheet == null)
                {
                    DebugLogger.Error("");
                    workbookData.sheets[i] = null;
                    continue;
                }
                else
                {
                    string sheetName = excelSheet.SheetName;
                    if(sheetName.StartsWith("#"))
                    {
                        continue;
                    }
                    if (excelSheet.FirstRowNum !=0)
                    {
                        DebugLogger.Error("");
                        workbookData.sheets[i] = null;
                        continue;
                    }
                    int rowCount = excelSheet.LastRowNum - excelSheet.FirstRowNum;
                    if(rowCount<MIN_EXCEL_ROW_COUNT)
                    {
                        DebugLogger.Error("");
                        workbookData.sheets[i] = null;
                        continue;
                    }
                    IRow firstRow = excelSheet.GetRow(0);
                    if(firstRow == null || firstRow.FirstCellNum!=0 && firstRow.LastCellNum-firstRow.FirstCellNum<=1)
                    {
                        DebugLogger.Error("");
                        workbookData.sheets[i] = null;
                        continue;
                    }
                    
                    string pattern = @"[A-Za-z0-9]{3,10}";
                    if(Regex.IsMatch(sheetName,pattern))
                    {
                        ConfigSheetData sheet = new ConfigSheetData();
                        sheet.name = sheetName;

                        sheet.fields = ReadFieldFromWorkbook(excelSheet);
                        sheet.lines = ReadSheetLineFromWorkbook(excelSheet,sheet.GetColNum());

                        workbookData.sheets[i] = sheet.IsValid() ? sheet : null;
                    }
                    else
                    {
                        DebugLogger.Error("");
                        workbookData.sheets[i] = null;
                        continue;
                    }
                }
            }

            return workbookData;
        }

        public static ConfigFieldData[] ReadFieldFromWorkbook(ISheet excelSheet)
        {
            List<ConfigFieldData> fields = new List<ConfigFieldData>();

            IRow excelRow = excelSheet.GetRow(0);
            int colCount = excelRow.LastCellNum - excelRow.FirstCellNum;
            List<FieldInfo> fieldInfos = new List<FieldInfo>();
            for(int i =0;i<MIN_EXCEL_ROW_COUNT;i++)
            {
                string name = excelSheet.GetRow(i).GetCell(0).StringCellValue;
                if(string.IsNullOrEmpty(name))
                {
                    fieldInfos.Add(null);
                }
                else
                {
                    FieldInfo fInfo = typeof(ConfigFieldData).GetField(name, BindingFlags.Instance | BindingFlags.Public);
                    fieldInfos.Add(fInfo);
                }
            }

            for(int i =1;i<colCount;i++)
            {
                ConfigFieldData fieldData = new ConfigFieldData();
                fieldData.col = i;
                for (int j = 0; j < MIN_EXCEL_ROW_COUNT; j++)
                {
                    IRow targetRow = excelSheet.GetRow(j);
                    if(targetRow!=null && fieldInfos[j]!=null)
                    {
                        string cellValue = GetCellStringValue(excelSheet.GetRow(j).GetCell(i));
                        fieldInfos[j].SetValue(fieldData, cellValue);
                    }
                }
                fields.Add(fieldData);
            }
            
            return fields.ToArray();
        }

        public static ConfigSheetLineData[] ReadSheetLineFromWorkbook(ISheet excelSheet,int colNum)
        {
            List<ConfigSheetLineData> lines = new List<ConfigSheetLineData>();

            int rowCount = excelSheet.LastRowNum - excelSheet.FirstRowNum;

            bool isStart = false;
            for(int i = MIN_EXCEL_ROW_COUNT;i<rowCount;i++)
            {
                IRow excelRow = excelSheet.GetRow(i);
                if(excelRow == null)
                {
                    continue;
                }
                ICell firstCell = excelRow.GetCell(0);
                if(firstCell!=null)
                {
                    string flag = firstCell.StringCellValue;
                    if (!isStart)
                    {
                        if (flag == "start")
                        {
                            isStart = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (flag == "end")
                        {
                            break;
                        }
                    }
                }
                ConfigSheetLineData line = new ConfigSheetLineData();
                line.row = i;

                List<ConfigContentData> contents = new List<ConfigContentData>();
                for (int j = 1; j < colNum+1; j++)
                {
                    ConfigContentData content = new ConfigContentData();
                    content.row = i;
                    content.col = j;
                    content.content = GetCellStringValue(excelRow.GetCell(j));
                    contents.Add(content);
                }
                line.contents = contents.ToArray();

                lines.Add(line);
            }

            return lines.ToArray();
        }

        private static string GetCellStringValue(ICell cell)
        {
            if (cell == null)
                return null;

            CellType cType = cell.CellType;
            if (cType == CellType.String)
            {
                return cell.StringCellValue;
            }
            else if (cType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }
            else if (cType == CellType.Boolean)
            {
                return cell.BooleanCellValue.ToString();
            }
            else if (cType == CellType.Formula)
            {
                CellType fCellType = cell.CachedFormulaResultType;
                if (fCellType == CellType.Numeric)
                {
                    return cell.NumericCellValue.ToString();
                }
                else if (fCellType == CellType.String)
                {
                    return cell.StringCellValue;
                }
            }
            return null;
        }
    }
}
