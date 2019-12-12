using Dot.Tools.ETD.Fields;

namespace Dot.Tools.ETD.Utils
{
    public static class ContentUtil
    {
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
