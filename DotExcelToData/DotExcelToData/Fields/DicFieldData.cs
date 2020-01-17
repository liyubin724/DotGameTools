using Dot.Tools.ETD.Utils;
using Dot.Tools.ETD.Validations;
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
    }
}
