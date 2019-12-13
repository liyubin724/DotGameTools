using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Factorys;
using Dot.Tools.ETD.Fields;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD
{
    public class ExcelReader
    {
        private static int MIN_ROW_COUNT = 6;
        private static int MIN_COLUMN_COUNT = 2;
        private static string SHEET_NAME_REGEX = @"^[A-Z]\w{4,10}";
        private static string ROW_START_FLAG = "start";
        private static string ROW_END_FLAG = "end";

        public static Workbook ReadExcel(string excelPath,out string msg)
        {
            msg = null;

            if(string.IsNullOrEmpty(excelPath) || !File.Exists(excelPath))
            {
                msg = $"ExcelReader::ReadExcel->File Not Found.excelPath = {excelPath}";
                return null;
            }

            string ext = Path.GetExtension(excelPath);
            if (ext != ".xlsx" && ext != ".xls")
            {
                msg = $"ExcelReader::ReadExcel->File is not a excel.excelPath = {excelPath}";
                return null;
            }

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

                if(workbook == null || workbook.NumberOfSheets ==0)
                {
                    msg = $"ExcelReader::ReadExcel->Excel is empty.excelPath = {excelPath}";
                    return null;
                }

                Workbook dataWB = new Workbook();
                dataWB.Name = excelPath;

                StringBuilder msgSB = new StringBuilder();

                for(int i =0;i<workbook.NumberOfSheets;i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    string sheetName = sheet.SheetName;
                    if(string.IsNullOrEmpty(sheetName))
                    {
                        msgSB.AppendLine("ExcelReader::ReadExcel->SheetName is null");
                        continue;
                    }
                    if(sheetName.StartsWith("#"))
                    {
                        continue;
                    }
                    if(!Regex.IsMatch(sheetName,SHEET_NAME_REGEX))
                    {
                        msgSB.AppendLine($"ExcelReader::ReadExcel->SheetName is error.sheetName = {sheetName}");
                        continue;
                    }
                    
                    Sheet dataSheet = GetSheet(sheet,msgSB);
                    if(dataSheet!=null)
                    {
                        dataWB.sheets.Add(dataSheet);
                    }
                }
                if(msgSB.Length>0)
                {
                    msg = msgSB.ToString();
                }
                return dataWB;
            }
        }

        private static Sheet GetSheet(ISheet sheet,StringBuilder msgSB )
        {
            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            int firstCol = sheet.GetRow(firstRow).FirstCellNum;
            int lastCol = sheet.GetRow(firstRow).LastCellNum;

            int rowCount = lastRow - firstRow + 1;
            int colCount = lastCol - firstCol + 1;
            if(rowCount < MIN_ROW_COUNT)
            {
                msgSB.AppendLine($"ExcelReader::GetSheet->the number of row is less then {MIN_ROW_COUNT}.sheetName = {sheet.SheetName}");
                return null;
            }

            if (colCount < MIN_COLUMN_COUNT)
            {
                msgSB.AppendLine($"ExcelReader::GetSheet->the number of col is less then {MIN_COLUMN_COUNT}.sheetName = {sheet.SheetName}");
                return null;
            }

            Sheet dataSheet = new Sheet();
            dataSheet.Name = sheet.SheetName;
            dataSheet.Field = GetSheetField(sheet, firstRow,lastRow,firstCol,lastCol,msgSB);
            dataSheet.Line = GetSheetLine(sheet, dataSheet.Field, firstRow, lastRow, firstCol, lastCol, msgSB);
            return dataSheet;
        }

        private static SheetField GetSheetField(ISheet sheet, 
            int firstRow, int lastRow,
            int firstCol,int lastCol, StringBuilder msgSB)
        {
            SheetField sheetField = new SheetField();
            MethodInfo getFieldMI = typeof(FieldFactory).GetMethod("GetField", BindingFlags.Public | BindingFlags.Static);

            int colCount = lastCol - firstCol+1;
            for(int i =1;i<colCount;i++)
            {
                List<object> values = new List<object>();
                values.Add(firstCol + i);
                for(int j = 0;j<MIN_ROW_COUNT;j++)
                {
                    IRow row = sheet.GetRow(firstRow + j);
                    ICell cell = row.GetCell(i);
                    if(j== 0)
                    {
                        if(cell == null)
                        {
                            break;
                        }else
                        {
                            string content = GetCellStringValue(cell);
                            if(string.IsNullOrEmpty(content) || content.StartsWith("#"))
                            {
                                break;
                            }
                        }
                    }
                    values.Add(GetCellStringValue(cell));
                }
                if(values.Count == MIN_ROW_COUNT+1)
                {
                    AField field = (AField)getFieldMI.Invoke(null, values.ToArray());
                    sheetField.fields.Add(field);
                }
            }
            return sheetField;
        }

        private static SheetLine GetSheetLine(ISheet sheet, SheetField sheetField,
            int firstRow, int lastRow, int firstCol, int lastCol, StringBuilder msgSB)
        {
            int rowCount = lastRow - firstRow - MIN_ROW_COUNT+1;

            SheetLine sheetLine = new SheetLine();
            bool isStart = false;
            for(int i =0;i<rowCount;i++)
            {
                IRow row = sheet.GetRow(MIN_ROW_COUNT+i);
                if(!isStart)
                {
                    ICell cell = row.GetCell(firstCol);
                    if(cell == null)
                    {
                        continue;
                    }
                    else
                    {
                        string cellContent = GetCellStringValue(cell);
                        if (cellContent == ROW_START_FLAG)
                        {
                            isStart = true;
                        }
                        else if (cellContent == ROW_END_FLAG)
                        {
                            break;
                        }
                    }
                }
                if(isStart)
                {
                    CellLine line = new CellLine();
                    line.Row = firstRow + i;

                    foreach(var field in sheetField.fields)
                    {

                    }

                    for(int j =0;j<sheetField.fields.Count;j++)
                    {
                        AField field = sheetField.fields[j];
                        ICell cell = row.GetCell(field.Col);
                        string content = GetCellStringValue(cell);
                        if (j == 0)
                        {
                            if (string.IsNullOrEmpty(content))
                            {
                                msgSB.AppendLine($"ExcelReader::GetSheetLine->fist cell is null.row = {firstRow + i},col={field.Col}");
                                break;
                            }
                        }

                        CellContent cellContent = new CellContent();
                        cellContent.Row = firstRow + i;
                        cellContent.Col = field.Col;
                        cellContent.Content = content;
                        line.cells.Add(cellContent);
                    }
                    if (line.cells.Count == sheetField.fields.Count)
                    {
                        sheetLine.lines.Add(line);
                    }
                }
            }

            return sheetLine;
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
