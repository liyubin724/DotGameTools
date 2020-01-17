using Dot.Tools.ETD.Validations;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class FloatFieldData : AFieldData
    {
        public FloatFieldData(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        protected override void AddExtraValidation(List<IValidation> validations)
        {
            validations.Add(new FloatValidation());
        }
    }
}
