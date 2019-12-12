using Dot.Tools.ETD.Fields;
using System;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Utils
{
    public static class ContentUtil
    {
        public static string[] SplitContent(string content,char[] splitChar)
        {
            if(string.IsNullOrEmpty(content))
            {
                return new string[0];
            }

            if(splitChar == null || splitChar.Length == 0)
            {
                return new string[] { content };
            }else
            {
                List<string> list = new List<string>();
                string value = string.Empty;
                for (int i = 0; i < content.Length; i++)
                {
                    char ch = content[i];
                    if (Array.IndexOf(splitChar,ch)>=0)
                    {
                        if(i-1<0)
                        {
                            if(!string.IsNullOrEmpty(value))
                            {
                                list.Add(value);
                                value = string.Empty;
                            }
                        }else
                        {
                            if(content[i-1] == '\\')
                            {
                                value = value.Substring(0, value.Length - 1) + ch;
                            }else
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                    list.Add(value);
                                    value = string.Empty;
                                }
                            }
                        }
                    }else
                    {
                        value += ch;
                    }
                }
                if (!string.IsNullOrEmpty(value))
                {
                    list.Add(value);
                }
                return list.ToArray();
            }
        }

        public static object GetValue(string content,FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Int:
                case FieldType.Ref:
                    {
                        if(!string.IsNullOrEmpty(content) && int.TryParse(content,out int result))
                        {
                            return result;
                        }
                        return 0;
                    }
                case FieldType.Long:
                    {
                        if (!string.IsNullOrEmpty(content) && long.TryParse(content, out long result))
                        {
                            return result;
                        }
                        return 0L;
                    }
                case FieldType.Float:
                    {
                        if (!string.IsNullOrEmpty(content) && float.TryParse(content, out float result))
                        {
                            return result;
                        }
                        return 0f;
                    }
                case FieldType.Bool:
                    {
                        if (!string.IsNullOrEmpty(content) && bool.TryParse(content,out bool result))
                        {
                            return result;
                        }
                        return false;
                    }
                case FieldType.String:
                case FieldType.Stringt:
                case FieldType.Res:
                case FieldType.Lua:
                    return content;
            }
            return null;
        }
    }
}
