using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Validations;
using ExtractInject;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class DicFieldData : AFieldData
    {
        public FieldType KeyType { get; private set; } = FieldType.None;
        public string KeyRefName { get; private set; } = string.Empty;
        public FieldType ValueType { get; private set; } = FieldType.None;
        public string ValueRefName { get; private set; } = string.Empty;

        public DicFieldData(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            FieldType keyType = FieldType.None;
            string keyRefName = string.Empty;
            FieldType valueType = FieldType.None;
            string valueRefName = string.Empty;

            FieldTypeUtil.GetDicInnerType(t, out keyType, out keyRefName, out valueType, out valueRefName);

            KeyType = keyType;
            ValueType = valueType;
            KeyRefName = keyRefName;
            ValueRefName = valueRefName;
        }

        protected override void AddExtraValidation(List<IValidation> validations)
        {
            validations.Add(new DicKeyValidation());
        }

        protected override bool InnerVerify(IEIContext context)
        {
            LogHandler logHandler = context.GetObject<LogHandler>();

            bool result = true;
            if(KeyType == FieldType.Text || KeyType == FieldType.Array 
                || KeyType == FieldType.Dic || KeyType == FieldType.Stringt || KeyType == FieldType.None)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_FIELD_VERIFY_DIC_KEY_ERROR, KeyType);
                result = false;
            }

            if(ValueType == FieldType.Array || ValueType == FieldType.Dic || ValueType == FieldType.None)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_FIELD_VERIFY_DIC_VALUE_ERROR, ValueType);
                result = false;
            }

            return result;
        }

        public override string GetOriginalDefault()
        {
            return "nil";
        }
    }
}
