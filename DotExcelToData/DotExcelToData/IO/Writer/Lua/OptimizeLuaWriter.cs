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
                    SummarySheetData summarySheet = OptimizeLuaAnalyzer.OptimizeSheet(book.Name, outputDir,sheet, 300, platform);
                    WriteSummarySheet(summarySheet);
                }
            }
        }

        private static void WriteSummarySheet(SummarySheetData summarySheet)
        {
            using(StreamWriter writer = new StreamWriter(summarySheet.OutputFilePath, false, new UTF8Encoding(false)))
            {
                string name = summarySheet.OutputName;
                writer.WriteLine($"{string.Format(IOConst.LUA_REQUIRE_FORMAT, IOConst.LUA_SUMMARY_SHEET_META)}");
                writer.WriteLine($"{string.Format(IOConst.LUA_LOCAL_DEFINE_FORMAT,name)}");
                writer.WriteLine(string.Format(IOConst.LUA_SET_INDEX_FORMART, name, name));
                
                writer.WriteLine($"{summarySheet.OutputName}." +
                    $"{IOConst.LUA_SUMMARY_DEPEND_NAME}={{");
                if (summarySheet.IsNeedText)
                {
                    string textName = string.Format(IOConst.LUA_PATH_FORMAT, summarySheet.bookName);
                    writer.WriteLine($"{WriterUtil.GetIndent(1)}{IOConst.LUA_SUMMARY_TEXT_NAME} = {string.Format(IOConst.LUA_REQUIRE_FORMAT, textName)},");
                }

                int indent = 0;
                WriteSummaryNames(summarySheet, writer,ref indent);
                WriteSummaryString(summarySheet, writer,ref indent);

                WriteSummaryDefault(summarySheet, writer,ref indent);

                writer.WriteLine(IOConst.LUA_LOCAL_DEFINE_END);

                
                WriteSummarySubSheet(summarySheet, writer);

                writer.WriteLine(string.Format(IOConst.LUA_SET_METATABLE_FORMAT, name, IOConst.LUA_SUMMARY_SHEET_META_NAME));

                writer.WriteLine($"return {name}");
                writer.Flush();
                writer.Close();
            }

            for (int i = 0; i < summarySheet.subSheets.Count; ++i)
            {
                WriteSubSheet(summarySheet, i);
            }
        }

        private static void WriteSummaryNames(SummarySheetData summarySheet,StreamWriter writer,ref int indent)
        {
            ++indent;
            {
                if (summarySheet.strFieldNames.Count > 0)
                {
                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUMMARY_STR_FIELD_NAME} = {{");
                    foreach (var name in summarySheet.strFieldNames)
                    {
                        ++indent;
                        writer.WriteLine($"{WriterUtil.GetIndent(indent)}\"{name}\",");
                        --indent;
                    }
                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}}},");
                }
            }
            --indent;
            
            
        }

        private static void WriteSummaryDefault(SummarySheetData summarySheet,StreamWriter writer,ref int indent)
        {
            ++indent;
            {
                writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUMMARY_DEFAULT_NAME} = {{");

                foreach (var kvp in summarySheet.defalutDic)
                {
                    string keyName = kvp.Key.name;
                    FieldType fType = kvp.Key.Type;
                    string value = kvp.Value;

                    if (FieldTypeUtil.IsStringType(fType))
                    {
                        keyName = string.Format(IOConst.LUA_FIELD_INDEX_FORMAT, keyName);
                        fType = FieldType.Int;
                        value = "" + (summarySheet.strList.IndexOf(value) + 1);
                    }

                    LuaWriterUtil.WriteContent(FieldType.String, keyName, fType, value, writer, ref indent);
                    writer.WriteLine(",");
                }

                writer.WriteLine($"{WriterUtil.GetIndent(indent)}}},");
            }
            --indent;
        }

        private static void WriteSummaryString(SummarySheetData summarySheet,StreamWriter writer,ref int indent)
        {
            ++indent;
            {
                writer.WriteLine($"{WriterUtil.GetIndent(indent)}{IOConst.LUA_SUMMARY_STRING_NAME} = {{");

                foreach (var str in summarySheet.strList)
                {
                    LuaWriterUtil.WriteContent(FieldType.String, str, writer, ref indent);
                    writer.WriteLine(",");
                }

                writer.WriteLine($"{WriterUtil.GetIndent(indent)}}},");
            }
            --indent;
            
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

            using (StreamWriter writer = new StreamWriter(subSheet.OutputFilePath, false,  new UTF8Encoding(false)))
            {
                writer.WriteLine($"{string.Format(IOConst.LUA_REQUIRE_FORMAT, IOConst.LUA_SUB_SHEET_META)}");
                writer.WriteLine($"{string.Format(IOConst.LUA_REQUIRE_FORMAT, IOConst.LUA_SHEET_LINE_META)}");

                Dictionary<string,string> tableNameDic = WriteSubSheetComplexContent(subSheet, writer);

                writer.WriteLine($"{string.Format(IOConst.LUA_LOCAL_DEFINE_FORMAT, name)}");
                writer.WriteLine(string.Format(IOConst.LUA_SET_INDEX_FORMART, name, name));
                //writer.WriteLine(string.Format(IOConst.LUA_SET_METATABLE_FORMAT, name, IOConst.LUA_SUB_SHEET_META_NAME));

                writer.Write($"{name}.ids = {{");
                foreach (var id in subSheet.GetLineIDs())
                {
                    writer.Write($"{id},");
                }
                writer.WriteLine("}");

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
                for (int m = 0; m < subSheet.lines.Count; ++m)
                {
                    SheetLine line = subSheet.lines[m];
                    string idContent = sheet.GetLineIDByRow(line.row);

                    string lineName = $"{name}[{idContent}]";
                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}{lineName} = {{");
                    for (int j = 0; j < fields.Count; ++j)
                    {
                        AFieldData field = fields[j];
                        LineCell cell = line.GetCellByCol(field.col);
                        string content = cell.GetContent(field);
                        if(string.IsNullOrEmpty(content))
                        {
                            continue;
                        }

                        WriteCell(summarySheet, subSheet,tableNameDic, field, content, writer, ref indent);
                    }

                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}}}");

                    writer.WriteLine(string.Format(IOConst.LUA_SET_INDEX_FORMART, lineName, lineName));
                    //writer.WriteLine(string.Format(IOConst.LUA_SET_METATABLE_FORMAT, lineName, IOConst.LUA_SHEET_LINE_META_NAME));

                    writer.WriteLine();
                }

                writer.WriteLine($"return {name}");

                writer.Flush();
                writer.Close();
            }
        }

        private static Dictionary<string, string> WriteSubSheetComplexContent(SubSheetData subSheet,StreamWriter writer)
        {
            int index = 1;
            Dictionary<string, string> tableNameDic = new Dictionary<string, string>();
            foreach(var kvp in subSheet.complexContentDic)
            {
                AFieldData field = kvp.Key;
                List<string> contents = kvp.Value;
                for(int i =0;i<contents.Count;++i)
                {
                    string content = contents[i];
                    string tName = string.Format(IOConst.LUA_TEMP_TABLE_NAME_FORMAT, index);
                    tableNameDic.Add(content, tName);
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
                        writer.Write($"local {tName} =");
                        string[] splitStr = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if(splitStr!=null && splitStr.Length>0)
                        {
                            for(int m = 0;m<splitStr.Length;++m)
                            {
                                writer.WriteLine(splitStr[m]);
                            }
                        }
                    }
                }
            }
            return tableNameDic;
        }

        private static void WriteCell(SummarySheetData summarySheet,
            SubSheetData subSheet,
            Dictionary<string, string> tableNameDic, 
            AFieldData field,
            string content,
            StreamWriter writer,
            ref int indent)
        {
            if(summarySheet.defalutDic.TryGetValue(field,out string dContent))
            {
                if(dContent == content)
                {
                    return;
                }
            }
            if(tableNameDic.TryGetValue(content,out string tableName))
            {
                ++indent;
                {
                    writer.WriteLine($"{WriterUtil.GetIndent(indent)}{field.name} = {tableName},");
                }
                --indent;
                return;
            }

            string keyName = field.name;
            FieldType fType = field.Type;

            if (FieldTypeUtil.IsStringType(fType))
            {
                keyName = string.Format(IOConst.LUA_FIELD_INDEX_FORMAT, keyName);
                fType = FieldType.Int;
                content = "" + (summarySheet.strList.IndexOf(content) + 1);
            }

            LuaWriterUtil.WriteContent(FieldType.String, keyName, fType, content, writer, ref indent);
            writer.WriteLine(",");
        }
    }
}
