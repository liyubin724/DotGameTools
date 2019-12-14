using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Dot.Serialize.Lua
{
    /// <summary>
    /// 将C#的数据结构转成Lua脚本文件
    /// </summary>
    public static class LuaSerializeWriter
    {
        public static void WriteToLua(string filePath,object data)
        {
            if(data == null)
            {
                return;
            }

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            StringBuilder luaSB = new StringBuilder();
            luaSB.Append($"local {fileName} = ");
            WriteObjectToLua(data, luaSB);
            if(luaSB[luaSB.Length-1] == ',')
            {
                luaSB.Remove(luaSB.Length - 1, 1);
            }
            luaSB.Append($"return {fileName}");

            File.WriteAllText(filePath, FormatLua(luaSB.ToString()));
        }

        private static string FormatLua(string luaStr)
        {
            StringBuilder luaSB = new StringBuilder();
            int indent = 0;
            for(int i =0;i<luaStr.Length;i++)
            {
                char ch = luaStr[i];
                if(ch == '{')
                {
                    luaSB.Append(ch);
                    luaSB.Append('\n');

                    indent++;
                    luaSB.Append(GetIndentSpace(indent));
                }
                else if(ch == '}')
                {
                    indent--;
                    luaSB.Append('\n');
                    luaSB.Append(GetIndentSpace(indent));
                    luaSB.Append(ch);

                    if (i + 1 < luaStr.Length && luaStr[i + 1] != ',')
                    {
                        luaSB.Append('\n');
                        luaSB.Append(GetIndentSpace(indent));
                    }
                }
                else if(ch == ',')
                {
                    luaSB.Append(ch);
                    if(i+1<luaStr.Length && luaStr[i+1] != '}')
                    {
                        luaSB.Append('\n');
                        luaSB.Append(GetIndentSpace(indent));
                    }
                }else
                {
                    luaSB.Append(ch);
                }
            }
            return luaSB.ToString();
        }

        private static string GetIndentSpace(int indent)
        {
            string result = string.Empty;
            for(int i =0;i<indent;i++)
            {
                result += "    ";
            }
            return result;
        }

        private static void WriteListToLua(IList data,StringBuilder luaSB)
        {
            if(data == null)
            {
                return;
            }
            luaSB.Append("{");

            foreach(var d in data)
            {
                WriteObjectToLua(d, luaSB);
                luaSB.Append(",");
            }
            luaSB.Append("}");
        }

        private static void WriteDicToLua(IDictionary data,StringBuilder luaSB)
        {
            if(data == null)
            {
                return;
            }
            IDictionaryEnumerator enumerator = data.GetEnumerator();
            luaSB.Append("{");
            while(enumerator.MoveNext())
            {
                string numberRegex = @"^[0-9.+-]+$";
                string key = enumerator.Key.ToString();
                if(Regex.IsMatch(key,numberRegex))
                {
                    luaSB.Append($"[{key}]=");
                }
                else
                {
                    luaSB.Append($"{key}=");
                }
                WriteObjectToLua(enumerator.Value, luaSB);
                luaSB.Append(",");
            }
            luaSB.Append("}");
        }

        private static void WriteTableToLua(object data,StringBuilder luaSB)
        {
            if(data == null)
            {
                return;
            }
            Type type = data.GetType();
            luaSB.Append("{");
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfos.Length > 0)
            {
                foreach (var fieldInfo in fieldInfos)
                {
                    luaSB.Append($"{fieldInfo.Name}=");
                    WriteObjectToLua(fieldInfo.GetValue(data), luaSB);
                    luaSB.Append(",");
                }
            }

            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            if (propertyInfos.Length > 0)
            {   
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.GetGetMethod() == null)
                    {
                        continue;
                    }
                    luaSB.Append($"{propertyInfo.Name}=");
                    WriteObjectToLua(propertyInfo.GetValue(data), luaSB);
                    luaSB.Append(",");
                }
            }

            luaSB.Append("}");
        }

        private static void WriteObjectToLua(object data, StringBuilder luaSB)
        {
            Type type = data.GetType();
            if (typeof(IList).IsAssignableFrom(type))
            {
                WriteListToLua((IList)data, luaSB);
            } else if (typeof(IDictionary).IsAssignableFrom(type))
            {
                WriteDicToLua((IDictionary)data, luaSB);
            } else if (type.IsValueType && type.IsEnum)
            {
                luaSB.Append((int)data);
            } else if (type == typeof(string))
            {
                luaSB.Append($"[[{data}]]");
            } else if (type.IsValueType && type.IsPrimitive)
            {
                luaSB.Append(data.ToString().ToLower());
            }
            else
            {
                WriteTableToLua(data, luaSB);
            } 
        }
    }
}
