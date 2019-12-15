using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Factorys;
using Dot.Tools.ETD.Utils;
using Dot.Tools.ETD.Validations;
using ExtractInject;
using System.Collections.Generic;
using System.Text;

namespace Dot.Tools.ETD.Fields
{
    public abstract class AField : IEIContextObject
    {
        protected int col;
        public int Col { get => col; }

        public string Name { get; set; }
        public string Desc { get; set; }
        protected string type;
        public FieldType Type
        {
            get
            {
                return FieldTypeUtil.GetFieldType(type);
            }
        }
        protected string platform;
        public FieldPlatform Platform
        {
            get
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
        }
        public string DefaultValue { get; set; }
        protected string validationRule;

        private IValidation[] validations = null;
        
        protected AField(int c,string n,string d,string t,string p,string dv,string vr)
        {
            col = c;
            Name = n;
            Desc = d;
            type = t == null ? "" : t.ToLower();
            platform = p == null ? "cs" : p.ToLower();
            DefaultValue = dv;
            validationRule = vr;
        }

        private IValidation[] GetValidations(IEIContext context)
        {
            if(validations == null)
            {
                List<IValidation> validationList = ValidationFactory.GetValidations(validationRule,context);
                AddExtraValidation(validationList);
                validations = validationList.ToArray();
            }
            return validations;
        }

        protected virtual void AddExtraValidation(List<IValidation> validationList)
        {

        }

        public bool VerifyContent(IEIContext context,CellContent cell,out string msg)
        {
            StringBuilder msgSB = new StringBuilder();
            bool result = true;

            IValidation[] validtionArr = GetValidations(context);

            foreach (var validation in validtionArr)
            {
                EIUtil.Inject(context, validation);

                ResultCode resultCode = validation.Verify(out string tMsg);
                if((int)resultCode<0)
                {
                    if(result)
                    {
                        result = false;
                    }
                    msgSB.AppendLine(tMsg);
                }
            }
            msg = msgSB.ToString();
            return result;
        }

        public virtual string GetContent(CellContent cell)
        {
            if (cell == null)
                return null;

            string content = cell.Content;
            if(string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(DefaultValue))
            {
                content = DefaultValue;
            }
            return content;
        }

        public abstract object GetValue(CellContent cell);
        
    }
}
