using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.Datas
{
    public class Sheet
    {
        public string Name { get; set; }
        public SheetField Field { get; set; }
        public SheetLine Line { get; set; }
    }
}
