using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Validations;
using ExtractInject;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class ArrayFieldData : AFieldData
    {
        public FieldType ValueType { get; private set; } = FieldType.None;
        public string ValueRefName { get; private set; } = string.Empty;

        public ArrayFieldData(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            string refName = string.Empty;
            ValueType = FieldTypeUtil.GetArrayInnerType(t, out refName);
            ValueRefName = refName;
        }

        protected override void AddExtraValidation(List<IValidation> validations)
        {
            
        }

        protected override bool InnerVerify(IEIContext context)
        {
            LogHandler logHandler = context.GetObject<LogHandler>();

            bool result = true;
            if (ValueType == FieldType.Array || ValueType == FieldType.Dic || ValueType == FieldType.None)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_FIELD_VERIFY_ARRAY_VALUE_ERROR, ValueType);
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
