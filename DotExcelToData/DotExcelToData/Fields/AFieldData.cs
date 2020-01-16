using Dot.Tools.ETD.Factorys;
using Dot.Tools.ETD.Utils;
using Dot.Tools.ETD.Validations;
using Dot.Tools.ETD.Verify;
using ExtractInject;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public abstract class AFieldData : IVerify
    {
        public int col;
        public string name;
        public string desc;
        public string type;
        public string platform;
        public string defaultValue;
        public string validationRule;

        protected AFieldData(int c,string n,string d,string t,string p,string v,string r)
        {
            col = c;
            name = n;
            desc = d;
            type = t ==null ?"":t.ToLower();
            platform = p==null?"cs":p.ToLower();
            defaultValue = v;
            validationRule = r;

            Type = FieldTypeUtil.GetFieldType(type);
            Platform = GetPlatform(platform);
        }

        public FieldType Type { get; private set; } = FieldType.None;
        public FieldPlatform Platform { get; private set; } = FieldPlatform.None;

        private IValidation[] validations = null;
        public IValidation[] GetValidations()
        {
            if(validations !=null)
            {
                return validations;
            }

            List<IValidation> validationList = ValidationFactory.GetValidations(validationRule, null);
            AddExtraValidation(validationList);
            validations = validationList.ToArray();

            return validations;
        }

        protected abstract void AddExtraValidation(List<IValidation> validations);

        public bool Verify(IEIContext context)
        {

            return VerifyField();
        }

        protected virtual bool VerifyField()
        {
            return true;
        }

        private FieldPlatform GetPlatform(string platform)
        {
            if (platform == "c")
            {
                return FieldPlatform.Client;
            }
            else if (platform == "s")
            {
                return FieldPlatform.Server;
            }
            else if (platform == "cs")
            {
                return FieldPlatform.All;
            }

            return FieldPlatform.None;
        }

        public override string ToString()
        {
            return $"<col = {col},name = {name},desc={desc},type={type},platform={platform},defaultValue={defaultValue},validationRule={validationRule}>";
        }
    }
}
