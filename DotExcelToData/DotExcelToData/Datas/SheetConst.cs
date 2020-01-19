using NPOI.SS.UserModel;

namespace Dot.Tools.ETD.Datas
{
    public static class SheetConst
    {
        public static int MIN_ROW_COUNT = 6;
        public static int MIN_COLUMN_COUNT = 2;
        public static string SHEET_NAME_REGEX = @"^[A-Z]\w{3,10}";
        public static string FIELD_NAME_REGEX = @"^[A-Za-z]{2,15}$";
        public static string ID_FIELD_NAME = "ID";
        public static string TEXT_BOOK_NAME = "Text";
        public static string ROW_START_FLAG = "start";
        public static string ROW_END_FLAG = "end";

        public static string GetCellStringValue(ICell cell)
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
