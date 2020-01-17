using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Validations;

namespace Dot.Tools.ETD.Fields
{
    public class FloatField : AField
    {
        public FloatField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        protected override void AddExtraValidation(List<IValidation> validationList)
        {
            validationList.Add(new FloatValidation());
        }

        public override object GetValue(LineCell cell)
        {
            string content = GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                return 0.0f;
            }
            if (float.TryParse(content, out float result))
            {
                return result;
            }
            return 0.0f;
        }
    }
}
