using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dot.Tools.ETD.IO
{
    public class OptimizeLuaWriter
    {
        public static void WriteBook(Workbook book, string outputDir, ETDWriterTarget target)
        {
            if (book == null)
            {
                return;
            }
            FieldPlatform platform = WriterUtil.GetPlatform(target);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            for (int i = 0; i < book.SheetCount; ++i)
            {
                Sheet sheet = book.GetSheetByIndex(i);
                if(sheet.name == SheetConst.TEXT_BOOK_NAME)
                {
                    LuaWriter.WriteSheet(book.Name, sheet, outputDir,platform);
                }else
                {
                    SummarySheetData summarySheet = OptimizeLuaAnalyzer.OptimizeSheet(book.Name, outputDir,sheet, 3, platform);
                    WriteSummarySheet(summarySheet);
                }
            }
        }

        private static void WriteSummarySheet(SummarySheetData summarySheet)
        {
            using(StreamWriter writer = new StreamWriter(summarySheet.OutputFilePath, false,Encoding.UTF8))
            {
                string name = summarySheet.OutputName;
                writer.WriteLine($"require(\"{IOConst.LUA_SUMMARY_SHEET_META}\")");
                writer.WriteLine($"local {name} = {{}}");
                writer.WriteLine(string.Format(IOConst.LUA_SET_INDEX_FORMART, name, name));
                writer.WriteLine(string.Format(IOConst.LUA_SET_METATABLE_FORMAT, name, IOConst.LUA_SUMMARY_SHEET_META_NAME));

                if(summarySheet.IsNeedText)
                {
                    writer.WriteLine($"{name}.{IOConst.LUA_SUMMARY_TEXT_NAME} = require (\"{string.Format(IOConst.LUA_PATH_FORMAT,summarySheet.bookName)}\")");
                }
                WriteSummaryNames(summarySheet, writer);
                WriteSummaryString(summarySheet, writer);
                WriteSummaryDefault(summarySheet, writer);
                WriteSummarySubSheet(summarySheet, writer);

                writer.WriteLine($"return {name}");
                writer.Flush();
                writer.Close();
            }

            for (int i = 0; i < summarySheet.subSheets.Count; ++i)
            {
                WriteSubSheet(summarySheet, i);
            }
        }

        private static void WriteSummaryNames(SummarySheetData summarySheet,StreamWriter writer)
        {
            if(summarySheet.luaFieldNames.Count>0)
            {
                writer.WriteLine($"{summarySheet.OutputName}.{IOConst.LUA_SUMMARY_LUA_FIELD_NAME} = {{");
                foreach(var name in summarySheet.luaFieldNames)
                {
                    writer.WriteLine($"{WriterUtil.GetIndent(1)}\"{name}\",");
                }
                writer.WriteLine("}");
            }
            if(summarySheet.strFieldNames.Count>0)
            {
                writer.WriteLine($"{summarySheet.OutputName}.{IOConst.LUA_SUMMARY_LUA_FIELD_NAME} = {{");
                foreach (var name in summarySheet.strFieldNames)
                {
                    writer.WriteLine($"{WriterUtil.GetIndent(1)}\"{name}\",");
                }
                writer.WriteLine("}");
            }
            if (summarySheet.textFieldNames.Count > 0)
            {
                writer.WriteLine($"{summarySheet.OutputName}.{IOConst.LUA_SUMMARY_LUA_FIELD_NAME} = {{");
                foreach (var name in summarySheet.textFieldNames)
                {
                    writer.WriteLine($"{WriterUtil.GetIndent(1)}\"{name}\",");
                }
                writer.WriteLine("}");
            }
        }

        private static void WriteSummaryDefault(SummarySheetData summarySheet,StreamWriter writer)
        {
            writer.WriteLine($"{summarySheet.OutputName}.{IOConst.LUA_SUMMARY_DEFAULT_NAME} = {{");

            int indent = 0;
            foreach(var kvp in summarySheet.defalutDic)
            {
                LuaWriterUtil.WriteContent(FieldType.String,kvp.Key.name, kvp.Key.Type, kvp.Value, writer, ref indent);
                writer.WriteLine(",");
            }

            writer.WriteLine("}");
        }

        private static void WriteSummaryString(SummarySheetData summarySheet,StreamWriter writer)
        {
            writer.WriteLine($"{summarySheet.OutputName}.{IOConst.LUA_SUMMARY_STRING_NAME} = {{");

            int indent = 0;
            foreach (var str in summarySheet.strList)
            {
                LuaWriterUtil.WriteContent(FieldType.String, str, writer, ref indent);
                writer.WriteLine(",");
            }

            writer.WriteLine("}");
        }

        private static void WriteSummarySubSheet(SummarySheetData summarySheet, StreamWriter writer)
        {
            string name = summarySheet.OutputName;
            writer.WriteLine($"{name}.{IOConst.LUA_SUMMARY_SUB_NAME} = {{");

            int indent = 0;
            for(int i =0;i<summarySheet.subSheets.Count;++i)
            {
                SubSheetData subSheet = summarySheet.subSheets[i];
                ++indent;
                {
                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}{{");

                    ++indent;
                    {
                        writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUMMARY_DATA_NAME} = {IOConst.LUA_NULL},");
                        writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUBMARY_SUBSHEET_STARTID} = {subSheet.startID},");
                        writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUBMARY_SUBSHEET_ENDID} = {subSheet.endID},");
                        writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUBMARY_SUBSHEET_PATH} = \"{string.Format(IOConst.LUA_SUBMARY_SUBSHEET_PATH_FORMAT, summarySheet.OutputName, subSheet.OutputName)}\",");
                    }
                    --indent;

                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}}},");
                }
                --indent;
            }

            writer.WriteLine("}");
        }

        private static void WriteSubSheet(SummarySheetData summarySheet,int index)
        {
            SubSheetData subSheet = summarySheet.subSheets[index];
            string name = subSheet.OutputName;
            Sheet sheet = summarySheet.sheet;
            FieldPlatform platform = summarySheet.platform;

            string dirPath = Path.GetDirectoryName(subSheet.OutputFilePath);
            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            using (StreamWriter writer = new StreamWriter(subSheet.OutputFilePath, false, Encoding.UTF8))
            {
                WriteSubSheetComplexContent(subSheet, writer);

                writer.WriteLine($"local {name} = {{}}");

                List<AFieldData> fields = new List<AFieldData>();
                for (int f = 0; f < sheet.FieldCount; ++f)
                {
                    AFieldData field = sheet.GetFieldByIndex(f);
                    if (field.Platform == FieldPlatform.All || field.Platform == platform)
                    {
                        fields.Add(field);
                    }
                }

                int indent = 0;

                for (int m = 0; m < sheet.LineCount; ++m)
                {
                    SheetLine line = sheet.GetLineByIndex(m);
                    string idContent = sheet.GetLineIDByIndex(m);

                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}{name}[{idContent}] = {{");
                    for (int j = 0; j < fields.Count; ++j)
                    {
                        AFieldData field = fields[j];

                        LineCell cell = line.GetCellByCol(field.col);
                        LuaWriterUtil.WriteCell(field, cell, writer, ref indent);
                        writer.WriteLine(",");
                    }

                    writer.Write($"{WriterUtil.GetIndent(indent)}}}");
                    writer.WriteLine();
                }

                writer.WriteLine($"return {name}");

                writer.Flush();
                writer.Close();
            }
        }

        private static void WriteSubSheetComplexContent(SubSheetData subSheet,StreamWriter writer)
        {
            int index = 1;
            Dictionary<string, string> contentToTableNameDic = new Dictionary<string, string>();
            foreach(var kvp in subSheet.complexContentDic)
            {
                AFieldData field = kvp.Key;
                List<string> contents = kvp.Value;
                for(int i =0;i<contents.Count;++i)
                {
                    string tName = string.Format(IOConst.LUA_TEMP_TABLE_NAME_FORMAT, index);
                    string content = contents[i];
                    contentToTableNameDic.Add(contents[i], tName);
                    index++;
                    if(field.Type == FieldType.Array)
                    {
                        ArrayFieldData arrayField = (ArrayFieldData)field;
                        writer.WriteLine($"local {tName} = {{");

                        string[] values = content.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values != null && values.Length > 0)
                        {
                            int indent = 0;
                            foreach(var value in values)
                            {
                                LuaWriterUtil.WriteContent(arrayField.ValueType, value, writer,ref indent);
                                writer.WriteLine(",");
                            }
                        }

                        writer.WriteLine("}");
                    }else if(field.Type == FieldType.Dic)
                    {
                        DicFieldData dicField = (DicFieldData)field;
                        writer.WriteLine($"local {tName} = {{");

                        string[] values = content.Split(new char[] { '{', '}', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values != null && values.Length > 0)
                        {
                            int indent = 0;
                            foreach (var value in values)
                            {
                                string[] tempArr = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if(tempArr.Length == 2)
                                {
                                    LuaWriterUtil.WriteContent(dicField.KeyType, tempArr[0], dicField.ValueType, tempArr[1], writer, ref indent);
                                    writer.WriteLine(",");
                                }
                            }
                        }

                        writer.WriteLine("}");
                    }
                    else if(field.Type == FieldType.Lua)
                    {
                        writer.WriteLine($"local {tName} = function()");
                        string[] splitStr = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if(splitStr!=null && splitStr.Length>0)
                        {
                            foreach(var str in splitStr)
                            {
                                writer.WriteLine($"{WriterUtil.GetIndent(1)}{str}");
                            }
                        }
                        writer.WriteLine("end");
                    }
                }
            }
        }
    }
}
