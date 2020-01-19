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















        public static void WriteTo(Workbook book,string outputDir,ETDWriterTarget target)
        {
            if(book == null)
            {
                return;
            }
            FieldPlatform platform = FieldPlatform.None;
            if(target == ETDWriterTarget.Client)
            {
                platform = FieldPlatform.Client;
            }else if(target == ETDWriterTarget.Server)
            {
                platform = FieldPlatform.Server;
            }

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
        }

        private void WriteSheet(string filePath,Sheet sheet)
        {

        }



        private Workbook workbook = null;

        public JsonWriter(Workbook book)
        {
            workbook = book;
        }

        public void WriteTo(string outputDir,ETDWriterTarget target)
        {
            if(workbook == null)
            {
                return;
            }

            string configDir = $"{outputDir}/{Path.GetFileNameWithoutExtension(workbook.bookPath)}";
            if(Directory.Exists(configDir))
            {
                Directory.Delete(configDir, true);
            }
            Directory.CreateDirectory(configDir);

            for(int i =0;i<workbook.SheetCount;++i)
            {
                Sheet sheet = workbook.GetSheetByIndex(i);

                string filePath = $"{configDir}/{sheet.name}{IOConst.JSON_EXTERSION}";
                
            }
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
