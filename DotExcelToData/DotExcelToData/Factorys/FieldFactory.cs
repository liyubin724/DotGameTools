using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Utils;
using System;

namespace Dot.Tools.ETD.Factorys
{
    public static class FieldFactory
    {
        public static AField GetField(int col, string name, string type,
            string platform,string desc,
            string value, string validation)
        {
            if(string.IsNullOrEmpty(type))
            {
                return null;
            }

            Type resultType = null;

            FieldType fieldType = FieldTypeUtil.GetFieldType(type);
            if(fieldType != FieldType.None)
            {
                string fieldName = fieldType.ToString() + "Field";
                resultType = AssemblyUtil.GetTypeByName(fieldName, true);
            }
            if(resultType == null)
            {
                resultType = typeof(ErrorField);
            }

            AField field = (AField)Activator.CreateInstance(resultType, col, name, desc, type, platform, value, validation);
            return field;
        }
    }
}
