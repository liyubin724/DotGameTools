using Dot.Tools.ETD.Validations;
using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Validations;
using Dot.Tools.ETD.Verify;
using ExtractInject;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public abstract class AFieldData : IVerify,IEIContextObject
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
            type = t ==null ?"":t.Trim().ToLower();
            platform = p==null?"cs":p.Trim().ToLower();
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
            bool result = true;
            LogHandler logHandler = context.GetObject<LogHandler>();

            logHandler.Log(LogType.Verbose, LogConst.LOG_FIELD_VERIFY_START,ToString());

            if (string.IsNullOrEmpty(name))
            {
                logHandler.Log(LogType.Error, LogConst.LOG_FIELD_VERIFY_NAME_NULL);
                result = false;
            }
            if(Type == FieldType.None)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_FIELD_VERIFY_TYPE_ERROR);
                result = false;
            }

            if(Platform == FieldPlatform.None)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_FIELD_VERIFY_PLATFORM_ERROR);
                result = false;
            }

            foreach(var v in GetValidations())
            {
                if(v.GetType() == typeof(ErrorValidation))
                {
                    logHandler.Log(LogType.Error, LogConst.LOG_FIELD_VERIFY_VALIDATION_ERROR,((ErrorValidation)v).Rule);
                    result = false;
                }
            }

            logHandler.Log(LogType.Verbose, LogConst.LOG_FIELD_VERIFY_END, result);

            return result;
        }

        protected virtual bool InnerVerify(IEIContext context)
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
