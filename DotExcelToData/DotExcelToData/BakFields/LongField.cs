using System.Collections.Generic;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Validations;

namespace Dot.Tools.ETD.Fields
{
    public class LongField : AField
    {
        public LongField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        protected override void AddExtraValidation(List<IValidation> validationList)
        {
            validationList.Add(new LongValidation());
        }

        public override object GetValue(LineCell cell)
        {
            string content = GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                return 0L;
            }
            if (long.TryParse(content, out long result))
            {
                return result;
            }
            return 0L;
        }
    }
}
