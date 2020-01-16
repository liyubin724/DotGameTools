using Dot.Tools.ETD.Verify;
using ExtractInject;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Datas
{
    public class Workbook : IVerify
    {
        public string bookPath;
        private List<Sheet> sheets = new List<Sheet>();

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(bookPath);
            }
        }

        public int SheetCount { get => sheets.Count; }

        public Workbook(string path)
        {
            this.bookPath = path;
        }

        public Sheet GetSheetByName(string sheetName)
        {
            foreach(var sheet in sheets)
            {
                if(sheet.name == sheetName)
                {
                    return sheet;
                }
            }
            return null;
        }

        public Sheet GetSheetByIndex(int index)
        {
            if(index>=0&&index<sheets.Count)
            {
                return sheets[index];
            }
            return null;
        }

        public void AddSheet(Sheet sheet)
        {
            sheets.Add(sheet);
        }

        public bool Verify()
        {
            throw new System.NotImplementedException();
        }


        //----------------------------------
        public bool LoadExcel(string excelPath,out string msg)
        {
            msg = string.Empty ;

            if (string.IsNullOrEmpty(excelPath) || !File.Exists(excelPath))
            {
                msg = $"ExcelReader::ReadExcel->File Not Found.excelPath = {excelPath}";
                return false;
            }

            string ext = Path.GetExtension(excelPath);
            if (ext != ".xlsx" && ext != ".xls")
            {
                msg = $"ExcelReader::ReadExcel->File is not a excel.excelPath = {excelPath}";
                return false;
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

                if (workbook == null || workbook.NumberOfSheets == 0)
                {
                    msg = $"ExcelReader::ReadExcel->Excel is empty.excelPath = {excelPath}";
                    return false;
                }

                Name = excelPath;

                StringBuilder msgSB = new StringBuilder();
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    string sheetName = sheet.SheetName;
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        msgSB.AppendLine("ExcelReader::ReadExcel->SheetName is null");
                        continue;
                    }
                    if (sheetName.StartsWith("#"))
                    {
                        continue;
                    }
                    if (!Regex.IsMatch(sheetName, SheetConst.SHEET_NAME_REGEX))
                    {
                        msgSB.AppendLine($"ExcelReader::ReadExcel->SheetName is error.sheetName = {sheetName}");
                        continue;
                    }

                    Sheet dataSheet = new Sheet();
                    if (dataSheet.LoadFromSheet(sheet,out string sheetMsg))
                    {
                        sheets.Add(dataSheet);
                    }else
                    {
                        msgSB.AppendLine(sheetMsg);
                    }
                }
                if (msgSB.Length > 0)
                {
                    msg = msgSB.ToString();
                    return false;
                }else
                {
                    return true;
                }
            }
        }

        public bool Verify(out string msg)
        {
            msg = string.Empty;

            EIContext context = new EIContext();

            foreach(var sheet in sheets)
            {
                context.AddObject(sheet);
                if(!sheet.Verify(context,out string sMsg))
                {
                    msg += sMsg;
                }
                context.DeleteObject(sheet);
            }

            if(string.IsNullOrEmpty(msg))
            {
                return true;
            }else
            {
                msg = $"Workbook::Verify->ExcelPath = {Name}\n" + msg;
                return false;
            }
        }

        
    }
}
