using Dot.Tools.ETD.Validations;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class IDFieldData : AFieldData
    {
        public IDFieldData(int c, string n, string d, string t, string p, string v, string r) : base()
        {
            n = "ID";
            t = "id";
            p = "cs";
            v = null;
            r = "NotNull@Unique";

            InitData(c, n, d, t, p, v, r);
        }

        public override string GetOriginalDefault()
        {
            throw new System.NotImplementedException();
        }

        protected override void AddExtraValidation(List<IValidation> validations)
        {
            validations.Add(new IntValidation());
        }
    }
}
