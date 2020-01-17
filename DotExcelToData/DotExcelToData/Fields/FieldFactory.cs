using Dot.Tools.ETD.Fields;
using System;

namespace Dot.Tools.ETD.Fields
{
    public static class FieldFactory
    {
        public static AFieldData GetField(
            int col, 
            string name, 
            string type,
            string platform,
            string desc,
            string value, 
            string validation)
        {
            if(string.IsNullOrEmpty(type))
            {
                return null;
            }

            Type resultType = null;

            FieldType fieldType = FieldTypeUtil.GetFieldType(type);
            if(fieldType != FieldType.None)
            {
                string fieldName = fieldType.ToString() + "FieldData";
                resultType = AssemblyUtil.GetTypeByName(fieldName, true);
            }

            if(resultType == null)
            {
                return null;
            }

            return (AFieldData)Activator.CreateInstance(resultType, col, name, desc, type, platform, value, validation);
        }
    }
}
