using System.Collections.Generic;

namespace Dot.Tools.ETD.Datas
{
    public class LineCell
    {
        public int row;

        private List<CellContent> cells = new List<CellContent>();

        public LineCell(int r)
        {
            row = r;
        }

        public void AddCell(int c,string v)
        {
            CellContent cell = new CellContent(row, c, v);
            cells.Add(cell);
        }

        public CellContent GetCell(int index)
        {
            if(index>=0&&index<cells.Count)
            {
                return cells[index];
            }
            return null;
        }

    }
}
