using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Tools.ETD.Datas;

namespace Dot.Tools.ETD.Fields
{
    public class FloatField : AField
    {
        public FloatField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override FieldType Type => FieldType.Float;

        public override FieldType InnerValueType => throw new NotImplementedException();

        public override FieldType InnerKeyType => throw new NotImplementedException();

        public override string RefTargetName => throw new NotImplementedException();

        public override object GetValue(CellContent cell)
        {
            throw new NotImplementedException();
        }
    }
}
