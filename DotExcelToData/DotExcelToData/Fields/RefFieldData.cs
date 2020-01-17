using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Validations;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class RefFieldData : AFieldData
    {
        public string RefName { get; private set; } = string.Empty;

        public RefFieldData(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            RefName = FieldTypeUtil.GetRefName(t);
        }

        protected override void AddExtraValidation(List<IValidation> validations)
        {
            
        }
    }
}
