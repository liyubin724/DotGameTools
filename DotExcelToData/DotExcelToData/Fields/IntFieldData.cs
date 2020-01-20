using Dot.Tools.ETD.Validations;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class IntFieldData : AFieldData
    {
        public IntFieldData(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetOriginalDefault()
        {
            return "0";
        }

        protected override void AddExtraValidation(List<IValidation> validations)
        {
            validations.Add(new IntValidation());
        }
    }
}
