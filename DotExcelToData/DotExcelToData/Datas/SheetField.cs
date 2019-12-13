using Dot.Tools.ETD.Factorys;
using Dot.Tools.ETD.Fields;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Dot.Tools.ETD.Datas
{
    public class SheetField
    {
        public List<AField> fields = new List<AField>();

        public void LoadFromSheet(ISheet sheet,
            int firstRow, int lastRow,
            int firstCol, int lastCol)
        {
            MethodInfo getFieldMI = typeof(FieldFactory).GetMethod("GetField", BindingFlags.Public | BindingFlags.Static);

            int colCount = lastCol - firstCol + 1;
            for (int i = 1; i < colCount; i++)
            {
                List<object> values = new List<object>();
                values.Add(firstCol + i);
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
                    AField field = (AField)getFieldMI.Invoke(null, values.ToArray());
                    fields.Add(field);
                }
            }
        }

        public bool Verify(out string msg)
        {
            msg = string.Empty;

            StringBuilder msgSB = new StringBuilder();
            foreach(var field in fields)
            {
                if(field.GetType() == typeof(ErrorField))
                {
                    msgSB.AppendLine(((ErrorField)field).ErrorMsg);
                }
            }

            msg = msgSB.ToString();
            if(msgSB.Length == 0)
            {
                return true;
            }else
            {
                msgSB.Insert(0, @"SheetField::Verify->Field Error\n");
                msg = msgSB.ToString();
                return false;
            }
        }

    }
}
