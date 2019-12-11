using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Tools.ETD.Datas;

namespace Dot.Tools.ETD.Fields
{
    public class ArrayField : AField
    {
        private static readonly string ARRAY_INNERVALUE_TYPE_REGEX = @"^array\[(?<typename>[A-Za-z]+)";
        public ArrayField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override FieldType Type => FieldType.Array;

        public override FieldType InnerValueType
        {
            get
            {
                return FieldType.None;
            }
        }

        public override FieldType InnerKeyType => throw new NotImplementedException();

        public override string RefTargetName => throw new NotImplementedException();

        public override object GetValue(CellContent cell)
        {
            throw new NotImplementedException();
        }
    }
}
