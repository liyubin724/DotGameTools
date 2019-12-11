using Dot.Tools.ETD.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.Fields
{
    public class IntField : AField
    {
        public IntField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override object GetValue(CellContent cell)
        {
            throw new NotImplementedException();
        }
    }
}
