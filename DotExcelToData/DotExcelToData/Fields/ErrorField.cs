using Dot.Tools.ETD.Datas;
using System;

namespace Dot.Tools.ETD.Fields
{
    public class ErrorField : AField
    {
        public ErrorField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override object GetValue(CellContent cell)
        {
            throw new NotImplementedException();
        }
    }
}
