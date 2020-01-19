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
        public static void WriteBook(Workbook book,string outputDir,ETDWriterTarget target)
        {
            if(book == null)
            {
                return;
            }
            FieldPlatform platform = GetPlatform(target);

            string configDir = $"{outputDir}/{book.Name}";
            if (Directory.Exists(configDir))
            {
                Directory.Delete(configDir, true);
            }
            Directory.CreateDirectory(configDir);

            for (int i = 0; i < book.SheetCount; ++i)
            {
                Sheet sheet = book.GetSheetByIndex(i);

                string filePath = $"{configDir}/{sheet.name}{IOConst.JSON_EXTERSION}";
                WriteSheet(sheet, filePath, platform);
            }
        }

        private static void WriteSheet(Sheet sheet, string filePath, FieldPlatform platform)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("{");

                int indent = 0;

                List<AFieldData> fields = new List<AFieldData>();
                for (int f = 0; f < sheet.FieldCount; ++f)
                {
                    AFieldData field = sheet.GetFieldByIndex(f);
                    if (field.Platform == FieldPlatform.All || field.Platform == platform)
                    {
                        fields.Add(field);
                    }
                }

                for (int m = 0; m < sheet.LineCount; ++m)
                {
                    SheetLine line = sheet.GetLineByIndex(m);
                    string idContent = sheet.GetLineIDByIndex(m);

                    ++indent;
                    {
                        writer.WriteLine($"{GetIndent(indent)}\"{idContent}\":{{");

                        for (int j = 0; j < fields.Count; ++j)
                        {
                            AFieldData field = fields[j];

                            LineCell cell = line.GetCellByCol(field.col);
                            WriteCell(field, cell, writer, ref indent);
                            if (j == fields.Count - 1)
                            {
                                writer.WriteLine();
                            }
                            else
                            {
                                writer.WriteLine(",");
                            }
                        }

                        writer.Write($"{GetIndent(indent)}}}");
                        if (m == sheet.LineCount - 1)
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
            }
        }

        private static void WriteCell(AFieldData field,LineCell cell,StreamWriter writer,ref int indent)
        {
            string content = cell.GetContent(field);
            if(string.IsNullOrEmpty(content))
            {
                return;
            }

            if(field.Type == FieldType.Array)
            {
                WriteArrayContent(field.name, field, content,writer,ref indent);
            }else if(field.Type ==FieldType.Dic)
            {
                WriteDicContent(field.name, field, content, writer, ref indent);
            }
            else
            {
                WriteContent(field.name, field.Type, content, writer, ref indent);
            }
        }

        private static void WriteContent(FieldType fieldType,string value, StreamWriter writer, ref int indent)
        {
            ++indent;
            {
                if (fieldType == FieldType.Bool)
                {
                    writer.Write($"{GetIndent(indent)}{value.ToLower()}");
                }
                else if (FieldTypeUtil.IsNumberType(fieldType))
                {
                    writer.Write($"{GetIndent(indent)}{value}");
                }
                else if (FieldTypeUtil.IsStringType(fieldType))
                {
                    writer.Write($"{GetIndent(indent)}\"{value}\"");
                }
            }
            --indent;
        }

        private static void WriteContent(string key,FieldType fieldType,string value, StreamWriter writer, ref int indent)
        {
            ++indent;
            {
                if(fieldType == FieldType.Bool)
                {
                    writer.Write($"{GetIndent(indent)}\"{key}\":{value.ToLower()}");
                }else if(FieldTypeUtil.IsNumberType(fieldType))
                {
                    writer.Write($"{GetIndent(indent)}\"{key}\":{value}");
                }else if (FieldTypeUtil.IsStringType(fieldType))
                {
                    writer.Write($"{GetIndent(indent)}\"{key}\":\"{value}\"");
                }
            }
            --indent;
        }

        private static void WriteArrayContent(string key,AFieldData field,string value, StreamWriter writer, ref int indent)
        {
            ArrayFieldData arrayFieldData = (ArrayFieldData)field;

            ++indent;
            {
                writer.WriteLine($"{GetIndent(indent)}\"{field.name}\":[");

                string[] values = value.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (values != null && values.Length > 0)
                {
                    for (int i = 0; i < values.Length; ++i)
                    {
                        WriteContent(arrayFieldData.ValueType, values[i],writer,ref indent);
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

                writer.Write($"{GetIndent(indent)}]");
            }
            --indent;
        }

        private static void WriteDicContent(string key,AFieldData field,string value, StreamWriter writer, ref int indent)
        {
            DicFieldData dicFieldData = (DicFieldData)field;

            ++indent;
            {
                writer.WriteLine($"{GetIndent(indent)}\"{field.name}\":{{");

                string[] values = value.Split(new char[] { '{', '}', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (values != null && values.Length > 0)
                {
                    for (int i = 0; i < values.Length; ++i)
                    {
                        string kvValue = values[i];
                        string[] tempArr = kvValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        WriteContent(tempArr[0], dicFieldData.ValueType, tempArr[1],writer,ref indent);
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

                writer.Write($"{GetIndent(indent)}}}");
            }
            --indent;
        }

        private static FieldPlatform GetPlatform(ETDWriterTarget target)
        {
            FieldPlatform platform = FieldPlatform.None;
            if (target == ETDWriterTarget.Client)
            {
                platform = FieldPlatform.Client;
            }
            else if (target == ETDWriterTarget.Server)
            {
                platform = FieldPlatform.Server;
            }
            return platform;
        }

        private static string GetIndent(int indent)
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
