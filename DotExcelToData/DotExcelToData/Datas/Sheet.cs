using ExtractInject;
using NPOI.SS.UserModel;
using System.Text;

namespace Dot.Tools.ETD.Datas
{
    public class Sheet : IEIContextObject
    {
        public string Name { get; set; }
        public SheetField Field { get; set; }
        public SheetLine Line { get; set; }

        public bool LoadFromSheet(ISheet sheet,out string msg)
        {
            msg = string.Empty;

            bool result = true;

            StringBuilder msgSB = new StringBuilder();
            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            int firstCol = sheet.GetRow(firstRow).FirstCellNum;
            int lastCol = sheet.GetRow(firstRow).LastCellNum;

            int rowCount = lastRow - firstRow + 1;
            int colCount = lastCol - firstCol + 1;
            if (rowCount < SheetConst.MIN_ROW_COUNT)
            {
                msgSB.AppendLine($"ExcelReader::GetSheet->the number of row is less then {SheetConst.MIN_ROW_COUNT}.sheetName = {sheet.SheetName}");
                result = false;
            }
            if (colCount < SheetConst.MIN_COLUMN_COUNT)
            {
                msgSB.AppendLine($"ExcelReader::GetSheet->the number of col is less then {SheetConst.MIN_COLUMN_COUNT}.sheetName = {sheet.SheetName}");
                result = false;
            }
            if(result)
            {
                SheetField sheetField = new SheetField();
                sheetField.LoadFromSheet(sheet, firstRow, lastRow, firstCol, lastCol);

                SheetLine sheetLine = new SheetLine();
                sheetLine.LoadFromSheet(sheet, sheetField, firstRow, lastRow, firstCol, lastCol);

                Name = sheet.SheetName;
                Field = sheetField;
                Line = sheetLine;
            }
            msg = msgSB.ToString();

            return result;
        }

        public bool Verify(IEIContext context,out string msg)
        {
            msg = string.Empty;

            if(!Field.Verify(out string fieldMsg))
            {
                msg += fieldMsg;
            }

            if(!Line.Verify(context,Field,out string lineMsg))
            {
                msg += lineMsg;
            }
            
            if(string.IsNullOrEmpty(msg))
            {
                return true;
            }else
            {
                msg =  $"Sheet::Verify->SheetName = {Name}\n" + msg;
                return false;
            }
        }
    }
}
