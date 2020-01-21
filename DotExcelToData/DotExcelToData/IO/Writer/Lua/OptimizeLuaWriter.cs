using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            FieldPlatform platform = GetPlatform(target);

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
                    SummarySheetData summarySheet = OptimizeLuaAnalyzer.OptimizeSheet(sheet, 3, platform);
                    summarySheet.bookName = book.Name;
                    WriteSummarySheet(sheet,summarySheet, outputDir,platform);
                }
            }
        }

        private static void WriteSummarySheet(Sheet sheet,SummarySheetData summarySheet,string outputDir,FieldPlatform platform)
        {
            string name = $"{summarySheet.bookName}_{summarySheet.sheetName}";

            string filePath = $"{outputDir}/{name}{IOConst.LUA_EXTERSION}";
            using(StreamWriter writer = new StreamWriter(filePath,false,Encoding.UTF8))
            {
                writer.WriteLine($"require(\"{IOConst.LUA_SUMMARY_SHEET_META}\")");
                writer.WriteLine($"local {name} = {{}}");
                writer.WriteLine(string.Format(IOConst.LUA_SET_INDEX_FORMART, name, name));
                writer.WriteLine(string.Format(IOConst.LUA_SET_METATABLE_FORMAT, name, IOConst.LUA_SUMMARY_SHEET_META_NAME));

                if(summarySheet.isNeedText)
                {
                    writer.WriteLine($"{name}.Text = require (\"{string.Format(IOConst.LUA_PATH_FORMAT,summarySheet.bookName)}\")");
                }

                WriteSummaryDefault(summarySheet, writer);
                WriteSummaryString(summarySheet, writer);
                WriteSummarySubSheet(summarySheet, writer);

                for(int i = 0;i<summarySheet.subSheets.Count;++i)
                {
                    WriteSubSheet(sheet,summarySheet, i, outputDir, platform);
                }

                writer.WriteLine($"return {name}");
                writer.Flush();
                writer.Close();
            }
        }

        private static void WriteSummaryDefault(SummarySheetData summarySheet,StreamWriter writer)
        {
            string name = $"{summarySheet.bookName}_{summarySheet.sheetName}";
            writer.WriteLine($"{name}.{IOConst.LUA_SUMMARY_DEFAULT_NAME} = {{");

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
            string name = $"{summarySheet.bookName}_{summarySheet.sheetName}";
            writer.WriteLine($"{name}.{IOConst.LUA_SUMMARY_STRING_NAME} = {{");

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
            string name = $"{summarySheet.bookName}_{summarySheet.sheetName}";
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
                        writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUBMARY_SUBSHEET_STARTID} = {subSheet.startID},");
                        writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUBMARY_SUBSHEET_ENDID} = {subSheet.endID},");
                        writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUBMARY_SUBSHEET_PATH} = \"{string.Format(IOConst.LUA_SUBMARY_SUBSHEET_PATH_FORMAT,name,i+1)}\",");
                    }
                    --indent;

                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}}},");
                }
                --indent;
            }

            writer.WriteLine("}");
        }

        private static void WriteSubSheet(Sheet sheet,SummarySheetData summarySheet,int index,string outputDir, FieldPlatform platform)
        {
            string name = $"{summarySheet.bookName}_{summarySheet.sheetName}_{index}";
            string filePath = $"{outputDir}/{name}{IOConst.LUA_EXTERSION}";
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                SubSheetData subSheet = summarySheet.subSheets[index];

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

                    writer.WriteLine($"{GetIndent(indent)}{name}[{idContent}] = {{");
                    for (int j = 0; j < fields.Count; ++j)
                    {
                        AFieldData field = fields[j];

                        LineCell cell = line.GetCellByCol(field.col);
                        LuaWriterUtil.WriteCell(field, cell, writer, ref indent);
                        writer.WriteLine(",");
                    }

                    writer.Write($"{GetIndent(indent)}}}");
                    writer.WriteLine();
                }

                writer.WriteLine($"return {name}");

                writer.Flush();
                writer.Close();
            }
        }

        private static void WriteCell(AFieldData field,string content,StreamWriter writer,ref int indent)
        {

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
            for (int i = 0; i < indent; ++i)
            {
                indentStr += "    ";
            }
            return indentStr;
        }
    }
}
