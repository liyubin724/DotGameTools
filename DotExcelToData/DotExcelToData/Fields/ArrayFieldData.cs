using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Utils;
using Dot.Tools.ETD.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.Fields
{
    public class ArrayFieldData : AFieldData
    {
        public FieldType ValueType { get; private set; } = FieldType.None;
        public string ValueRefName { get; private set; } = string.Empty;

        public ArrayFieldData(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            string refName = string.Empty;
            ValueType = FieldTypeUtil.GetArrayInnerType(t, out refName);
            ValueRefName = refName;
        }

        protected override void AddExtraValidation(List<IValidation> validations)
        {
            
        }
    }
}
