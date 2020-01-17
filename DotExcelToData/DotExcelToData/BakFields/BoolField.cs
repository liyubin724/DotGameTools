using System.Collections.Generic;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Validations;

namespace Dot.Tools.ETD.Fields
{
    public class BoolField : AField
    {
        public BoolField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        protected override void AddExtraValidation(List<IValidation> validationList)
        {
            validationList.Add(new BoolValidation());
        }

        public override string GetContent(LineCell cell)
        {
            string content = base.GetContent(cell);
            if(string.IsNullOrEmpty(content))
            {
                return "false";
            }
            return content.ToLower();
        }

        public override object GetValue(LineCell cell)
        {
            string content = GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }
            content = content.ToLower();
            if (bool.TryParse(content.ToLower(), out bool result))
            {
                return result;
            }
            return false;
        }
    }
}
