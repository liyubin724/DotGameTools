using System.Collections.Generic;

namespace Dot.Tools.ETD.Datas
{
    public class CellLine
    {
        public int Row { get; set; }
        public List<CellContent> cells = new List<CellContent>();
    }
}
