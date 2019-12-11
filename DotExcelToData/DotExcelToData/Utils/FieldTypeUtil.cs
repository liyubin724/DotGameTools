using Dot.Tools.ETD.Fields;
using System;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Utils
{
    public static class FieldTypeUtil
    {
        private static FieldType StrToFieldType(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr))
            {
                return FieldType.None;
            }

            string typeName = typeStr.Trim();

            if (string.IsNullOrEmpty(typeName))
            {
                return FieldType.None;
            }

            if (Enum.TryParse(typeName, true, out FieldType fieldType))
            {
                return fieldType;
            }
            else
            {
                return FieldType.None;
            }
        }

        private const string FIELD_TYPE_REGEX = @"^(?<typename>[A-Za-z]+)";
        public static FieldType GetFieldType(string typeStr)
        {
            if(string.IsNullOrEmpty(typeStr))
            {
                return FieldType.None;
            }

            Match match = new Regex(FIELD_TYPE_REGEX).Match(typeStr);
            Group group = match.Groups["typename"];
            if(group!=null && group.Success)
            {
                return StrToFieldType(group.Value);
            }
            return FieldType.None;
        }

        private const string REF_NAME_REGEX = @"^ref<(?<refname>[a-zA-Z]+[0-9]+)>$";
        public static string GetRefName(string typeStr)
        {
            if(string.IsNullOrEmpty(typeStr))
            {
                return string.Empty;
            }
            Match match = new Regex(REF_NAME_REGEX).Match(typeStr);
            Group group = match.Groups["refname"];
            if(group!=null && group.Success)
            {
                return group.Value;
            }
            return string.Empty;
        }

        private const string ARRAY_INNER_REGEX = @"^array\[(?<innertype>[A-Za-z]+)[<]{0,1}(?<refname>[a-zA-Z]*[0-9]*)[>]{0,1}\]$";
        public static FieldType GetArrayInnerType(string typeStr,out string refName)
        {
            refName = string.Empty;

            if (string.IsNullOrEmpty(typeStr))
            {
                return FieldType.None;
            }
            Match match = new Regex(ARRAY_INNER_REGEX).Match(typeStr);
            Group innerTypeGroup = match.Groups["innertype"];
            if (innerTypeGroup !=null && innerTypeGroup.Success)
            {
                Group refNameGroup = match.Groups["refname"];
                if(refNameGroup !=null && refNameGroup.Success)
                {
                    refName = refNameGroup.Value;
                }
                return StrToFieldType(innerTypeGroup.Value);
            }
            return FieldType.None;
        }
        private const string DIC_INNER_REGEX = @"^dic\{(?<keytype>[A-Za-z]+)[<]{0,1}(?<keyrefname>[a-zA-Z]*[0-9]*)[>]{0,1},(?<valuetype>[A-Za-z]+)[<]{0,1}(?<valuerefname>[a-zA-Z]*[0-9]*)[>]{0,1}\}$";
        public static void GetDicInnerType(string typeStr,
            out FieldType keyType,out string keyRefName,
            out FieldType valueType,out string valueRefName)
        {
            keyType = valueType = FieldType.None;
            keyRefName = valueRefName = string.Empty;

            if(string.IsNullOrEmpty(typeStr))
            {
                return;
            }

            Match match = new Regex(DIC_INNER_REGEX).Match(typeStr);
            Group keyTypeGroup = match.Groups["keytype"];
            if (keyTypeGroup != null && keyTypeGroup.Success)
            {
                Group keyRefNameGroup = match.Groups["keyrefname"];
                if (keyRefNameGroup != null && keyRefNameGroup.Success)
                {
                    keyRefName = keyRefNameGroup.Value;
                }
                keyType = StrToFieldType(keyTypeGroup.Value);
            }
            Group valueTypeGroup = match.Groups["valuetype"];
            if (valueTypeGroup != null && valueTypeGroup.Success)
            {
                Group valueRefNameGroup = match.Groups["valuerefname"];
                if (valueRefNameGroup != null && valueRefNameGroup.Success)
                {
                    valueRefName = valueRefNameGroup.Value;
                }
                valueType = StrToFieldType(valueTypeGroup.Value);
            }
        }
    }
}
