using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Tools.ETD.Datas;

namespace Dot.Tools.ETD.Fields
{
    public class RefField : AField
    {
        public RefField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override object GetValue(CellContent cell)
        {
            throw new NotImplementedException();
        }
    }
}
