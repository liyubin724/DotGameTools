using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dot.Tools.ETD.IO
{
    public class JsonWriter
    {
        private static readonly string JSON_EXTERSION = ".json";

        private Sheet sheet = null;

        public JsonWriter(Sheet sheet)
        {
            this.sheet = sheet;
        }

        private StreamWriter writer = null;
        private int indent = 0;
        public void WriteTo(string outputDir,FieldPlatform platform)
        {
            if (sheet == null)
            {
                return;
            }
            string filePath = $"{outputDir}/{sheet.name}{JSON_EXTERSION}";
            writer = new StreamWriter(filePath, false, Encoding.UTF8);

            writer.WriteLine("{");

            List<AFieldData> fields = new List<AFieldData>();
            for(int i = 0;i<sheet.FieldCount;++i)
            {
                AFieldData field = sheet.GetFieldByIndex(i);
                if (field.Platform == FieldPlatform.All || field.Platform == platform)
                {
                    fields.Add(field);
                }
            }

            for (int i =0;i<sheet.LineCount;++i)
            {
                SheetLine line = sheet.GetLineByIndex(i);
                string idContent = sheet.GetLineIDByIndex(i);

                ++indent;
                {
                    writer.WriteLine($"{GetIndent()}\"{idContent}\":{{");

                    for (int j = 0; j < fields.Count; ++j)
                    {
                        AFieldData field = fields[j];

                        LineCell cell = line.GetCellByCol(field.col);
                        WriteCell(field, cell);
                        if (j == fields.Count - 1)
                        {
                            writer.WriteLine();
                        }
                        else
                        {
                            writer.WriteLine(",");
                        }
                    }

                    writer.Write($"{GetIndent()}}}");
                    if (i == sheet.LineCount - 1)
                    {
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.WriteLine(",");
                    }
                }
                --indent;
            }

            writer.WriteLine("}");

            writer.Flush();
            writer.Close();
            writer = null;
        }

        private void WriteCell(AFieldData field,LineCell cell)
        {
            string content = cell.GetContent(field);
            if(string.IsNullOrEmpty(content))
            {
                return;
            }

            if(field.Type == FieldType.Array)
            {
                WriteArrayContent(field.name, field, content);
            }else if(field.Type ==FieldType.Dic)
            {
                WriteDicContent(field.name, field, content);
            }else
            {
                WriteContent(field.name, field.Type, content);
            }
        }

        private void WriteContent(FieldType fieldType,string value)
        {
            ++indent;
            {
                if (fieldType == FieldType.Bool)
                {
                    writer.Write($"{GetIndent()}{value.ToLower()}");
                }
                else if (FieldTypeUtil.IsNumberType(fieldType))
                {
                    writer.Write($"{GetIndent()}{value}");
                }
                else if (FieldTypeUtil.IsStringType(fieldType))
                {
                    writer.Write($"{GetIndent()}\"{value}\"");
                }
            }
            --indent;
        }

        private void WriteContent(string key,FieldType fieldType,string value)
        {
            ++indent;
            {
                if(fieldType == FieldType.Bool)
                {
                    writer.Write($"{GetIndent()}\"{key}\":{value.ToLower()}");
                }else if(FieldTypeUtil.IsNumberType(fieldType))
                {
                    writer.Write($"{GetIndent()}\"{key}\":{value}");
                }else if (FieldTypeUtil.IsStringType(fieldType))
                {
                    writer.Write($"{GetIndent()}\"{key}\":\"{value}\"");
                }
            }
            --indent;
        }

        private void WriteArrayContent(string key,AFieldData field,string value)
        {
            ArrayFieldData arrayFieldData = (ArrayFieldData)field;

            ++indent;
            {
                writer.WriteLine($"{GetIndent()}\"{field.name}\":[");

                string[] values = value.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (values != null && values.Length > 0)
                {
                    for (int i = 0; i < values.Length; ++i)
                    {
                        WriteContent(arrayFieldData.ValueType, values[i]);
                        if (i != values.Length - 1)
                        {
                            writer.WriteLine(",");
                        }
                        else
                        {
                            writer.WriteLine();
                        }
                    }
                }

                writer.Write($"{GetIndent()}]");
            }
            --indent;
        }

        private void WriteDicContent(string key,AFieldData field,string value)
        {
            DicFieldData dicFieldData = (DicFieldData)field;

            ++indent;
            {
                writer.WriteLine($"{GetIndent()}\"{field.name}\":{{");

                string[] values = value.Split(new char[] { '{', '}', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (values != null && values.Length > 0)
                {
                    for (int i = 0; i < values.Length; ++i)
                    {
                        string kvValue = values[i];
                        string[] tempArr = kvValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        WriteContent(tempArr[0], dicFieldData.ValueType, tempArr[1]);
                        if (i != values.Length - 1)
                        {
                            writer.WriteLine(",");
                        }
                        else
                        {
                            writer.WriteLine();
                        }
                    }
                }

                writer.Write($"{GetIndent()}}}");
            }
            --indent;
        }

        private string GetIndent()
        {
            string indentStr = "";
            for(int i =0;i<indent;++i)
            {
                indentStr += "    ";
            }
            return indentStr;
        }
    }
}
