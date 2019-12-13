using Dot.Tools.ETD.Fields;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Text;

namespace Dot.Tools.ETD.Datas
{
    public class SheetLine
    {
        public List<CellLine> lines = new List<CellLine>();

        public void LoadFromSheet(ISheet sheet, SheetField sheetField,
            int firstRow, int lastRow, int firstCol, int lastCol)
        {
            int rowCount = lastRow - firstRow - SheetConst.MIN_ROW_COUNT + 1;

            bool isStart = false;
            for (int i = 0; i < rowCount; i++)
            {
                IRow row = sheet.GetRow(SheetConst.MIN_ROW_COUNT + i);
                if (!isStart)
                {
                    ICell cell = row.GetCell(firstCol);
                    if (cell == null)
                    {
                        continue;
                    }
                    else
                    {
                        string cellContent = SheetConst.GetCellStringValue(cell);
                        if (cellContent == SheetConst.ROW_START_FLAG)
                        {
                            isStart = true;
                        }
                        else if (cellContent == SheetConst.ROW_END_FLAG)
                        {
                            break;
                        }
                    }
                }
                if (isStart)
                {
                    CellLine line = new CellLine();
                    line.Row = firstRow + i;

                    for (int j = 0; j < sheetField.fields.Count; j++)
                    {
                        AField field = sheetField.fields[j];
                        ICell cell = row.GetCell(field.Col);
                        string content = SheetConst.GetCellStringValue(cell);
                        if (j == 0)
                        {
                            if (string.IsNullOrEmpty(content))
                            {
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
                        lines.Add(line);
                    }
                }
            }
        }

        public bool Verify(SheetField sheetField, out string msg)
        {
            msg = string.Empty;

            StringBuilder msgSB = new StringBuilder();
            foreach(var line in lines)
            {
                for(int i =0;i<sheetField.fields.Count;i++)
                {
                    AField field = sheetField.fields[i];
                    if(!field.VerifyContent(line.cells[i],out string cellMsg))
                    {
                        msgSB.AppendLine(cellMsg);
                    }
                }
            }
            if(msgSB.Length == 0)
            {
                return true;
            }else
            {
                msgSB.Insert(0, "SheetLine::Verify->line Error\n");
                msg = msgSB.ToString();
                return false;
            }
        }
    }
}
