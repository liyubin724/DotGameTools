using System.Collections.Generic;

namespace Dot.Tools.ETD.Datas
{
    public class SheetLine
    {
        public int row;

        private List<LineCell> cells = new List<LineCell>();

        public SheetLine(int r)
        {
            row = r;
        }

        public void AddCell(int c,string v)
        {
            LineCell cell = new LineCell(row, c, v);
            cells.Add(cell);
        }

        public LineCell GetCell(int index)
        {
            if(index>=0&&index<cells.Count)
            {
                return cells[index];
            }
            return null;
        }

    }
}
