using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.Datas
{
    public class CellLine
    {
        public int Row { get; set; }

        public List<CellContent> cells = new List<CellContent>();
    }
}
