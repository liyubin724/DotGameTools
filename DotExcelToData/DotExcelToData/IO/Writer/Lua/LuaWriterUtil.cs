﻿using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.IO;

namespace Dot.Tools.ETD.IO
{
    public static class LuaWriterUtil
    {
        internal static void WriteContent(AFieldData field, string content, StreamWriter writer, ref int indent)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            if (field.Type == FieldType.Array)
            {
                WriteArrayContent(field, content, writer, ref indent);
            }
            else if (field.Type == FieldType.Dic)
            {
                WriteDicContent(field, content, writer, ref indent);
            }
            else
            {
                WriteContent(FieldType.String, field.name, field.Type, content, writer, ref indent);
            }
        }

        internal static void WriteContent(FieldType fieldType, string value, StreamWriter writer, ref int indent)
        {
            ++indent;
            {
                if (fieldType == FieldType.Bool)
                {
                    writer.Write($"{WriterUtil.GetIndent(indent)}{value.ToLower()}");
                }
                else if (FieldTypeUtil.IsNumberType(fieldType))
                {
                    writer.Write($"{WriterUtil.GetIndent(indent)}{value}");
                }
                else if (FieldTypeUtil.IsStringType(fieldType))
                {
                    writer.Write($"{WriterUtil.GetIndent(indent)}{string.Format(IOConst.LUA_STRING_FORMAT,value)}");
                }
            }
            --indent;
        }

        internal static void WriteContent(FieldType keyType, string key, FieldType valueType, string value, StreamWriter writer, ref int indent)
        {
            ++indent;
            {
                writer.Write(WriterUtil.GetIndent(indent));
                if (FieldTypeUtil.IsNumberType(keyType))
                {
                    writer.Write($"[{key.ToLower()}]");
                }
                else if (FieldTypeUtil.IsStringType(keyType))
                {
                    writer.Write($"{key}");
                }

                writer.Write(" = ");

                if (FieldTypeUtil.IsNumberType(valueType))
                {
                    writer.Write($"{value.ToLower()}");
                }
                else if (FieldTypeUtil.IsStringType(valueType))
                {
                    writer.Write($"{string.Format(IOConst.LUA_STRING_FORMAT, value)}");
                }
            }
            --indent;
        }

        internal static void WriteArrayContent(AFieldData field, string content, StreamWriter writer, ref int indent)
        {
            ArrayFieldData arrayFieldData = (ArrayFieldData)field;

            ++indent;
            {
                writer.WriteLine($"{WriterUtil.GetIndent(indent)}{field.name} = {{");

                string[] values = content.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (values != null && values.Length > 0)
                {
                    for (int i = 0; i < values.Length; ++i)
                    {
                        WriteContent(arrayFieldData.ValueType, values[i], writer, ref indent);
                        writer.WriteLine(",");
                    }
                }

                writer.Write($"{WriterUtil.GetIndent(indent)}}}");
            }
            --indent;
        }

        internal static void WriteDicContent(AFieldData field, string content, StreamWriter writer, ref int indent)
        {
            DicFieldData dicFieldData = (DicFieldData)field;

            ++indent;
            {
                writer.WriteLine($"{WriterUtil.GetIndent(indent)}{field.name} = {{");

                string[] values = content.Split(new char[] { '{', '}', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (values != null && values.Length > 0)
                {
                    for (int i = 0; i < values.Length; ++i)
                    {
                        string kvValue = values[i];
                        string[] tempArr = kvValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if(tempArr.Length == 2)
                        {
                            WriteContent(dicFieldData.KeyType, tempArr[0], dicFieldData.ValueType, tempArr[1], writer, ref indent);
                            writer.WriteLine(",");
                        }
                    }
                }

                writer.Write($"{WriterUtil.GetIndent(indent)}}}");
            }
            --indent;
        }
    }
}
