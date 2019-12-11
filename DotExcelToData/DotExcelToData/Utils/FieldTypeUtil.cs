using Dot.Tools.ETD.Fields;
using System;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Utils
{
    public static class FieldTypeUtil
    {
        private const string FIELD_TYPE_REGEX = @"^(?<typename>[A-Za-z]+)";
        private const string ARRAY_INNER_REGEX = @"^array\[(?<valuetype>[a-zA-Z]+)";
        private const string DIC_INNER_REGEX = @"^dic\{(?<valuetype>[a-zA-Z]+[<]{0,1}\w*[>]{0,1}";
        public static FieldType GetFieldType(string typeStr)
        {
            if(string.IsNullOrEmpty(typeStr))
            {
                return FieldType.None;
            }

            Match match = new Regex(FIELD_TYPE_REGEX).Match(typeStr);
            if(match.Success)
            {
                string typeName = match.Groups["typename"].Value;
                if(string.IsNullOrEmpty(typeName))
                {
                    return FieldType.None;
                }
                if(Enum.TryParse(typeName,true,out FieldType fieldType))
                {
                    return fieldType;
                }else
                {
                    return FieldType.None;
                }
            }
            return FieldType.None;
        }

        public static FieldType GetInnerKeyType(string typeStr)
        {
            FieldType fieldType = GetFieldType(typeStr);
            if(fieldType!= FieldType.Array || fieldType!= FieldType.Dic)
            {
                return FieldType.None;
            }


            return FieldType.None;
        }

        public static FieldType GetInnerValueType(string typeStr)
        {
            return FieldType.None;
        }

        public static string GetInnerRefName(string typeStr)
        {
            return "";
        }
    }
}
