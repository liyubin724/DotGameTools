using Dot.Tools.ETD.Validations;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class ResFieldData : AFieldData
    {
        public ResFieldData(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetOriginalDefault()
        {
            return string.Empty;
        }

        protected override void AddExtraValidation(List<IValidation> validations)
        {
            
        }
    }
}
