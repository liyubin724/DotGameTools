using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.IO;
using System.Text;
using System.Web;

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
            if(sheet==null)
            {
                return;
            }
            string filePath = $"{outputDir}/{sheet.name}{JSON_EXTERSION}";
            writer = new StreamWriter(filePath, false, Encoding.UTF8);

            writer.WriteLine("{");

            for(int i =0;i<sheet.LineCount;++i)
            {
                SheetLine line = sheet.GetLineByIndex(i);

                AFieldData idField = sheet.GetFieldByIndex(0);
                LineCell idCell = line.GetCellByIndex(0);
                ++indent;
                {
                    writer.WriteLine($"{GetIndent()}\"{idCell.GetContent(idField)}\":{{");

                    for (int j = 0; j < sheet.FieldCount; ++j)
                    {
                        AFieldData field = sheet.GetFieldByIndex(j);

                        if(field.Platform != FieldPlatform.All && field.Platform == platform)
                        {
                            continue;
                        }

                        LineCell cell = line.GetCellByIndex(j);

                        WriteCell(field, cell);

                        if (j == sheet.FieldCount - 1)
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
                else if (fieldType == FieldType.Int || fieldType == FieldType.Float
                   || fieldType == FieldType.Long || fieldType == FieldType.Ref)
                {
                    writer.Write($"{GetIndent()}{value}");
                }
                else if (fieldType == FieldType.String || fieldType == FieldType.Res)
                {
                    writer.Write($"{GetIndent()}\"{value}\"");
                }
                else if (fieldType == FieldType.Stringt || fieldType == FieldType.Lua)
                {
                    writer.Write($"{GetIndent()}\"{HttpUtility.UrlEncode(value, Encoding.UTF8)}\"");
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
                }else if(fieldType == FieldType.Int || fieldType == FieldType.Float 
                    || fieldType == FieldType.Long || fieldType == FieldType.Ref)
                {
                    writer.Write($"{GetIndent()}\"{key}\":{value}");
                }else if(fieldType == FieldType.String || fieldType == FieldType.Res)
                {
                    writer.Write($"{GetIndent()}\"{key}\":\"{value}\"");
                }else if(fieldType == FieldType.Stringt || fieldType == FieldType.Lua)
                {
                    writer.Write($"{GetIndent()}\"{key}\":\"{HttpUtility.UrlEncode(value,Encoding.UTF8)}\"");
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
