using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dot.Tools.ETD.Exporter
{
    public class InnerTableData
    {
        public string content;
        public string paramName;
        public string luaStr;
    }

    public static class LuaOptimizeExporter
    {
        public static void Export(string outputDirPath, Sheet sheet, FieldPlatform platform)
        {
            if (!Directory.Exists(outputDirPath))
            {
                if (!Directory.CreateDirectory(outputDirPath).Exists)
                {
                    return;
                }
            }

            ExportStringToLua(sheet, platform, out List<string> strList, out List<string> textList);

            StringBuilder luaSB = new StringBuilder();
            if(strList.Count>0)
            {
                luaSB.AppendLine("local ConfigInnerString = {");
                foreach(var str in strList)
                {
                    luaSB.AppendLine($"    [[{str}]],");
                }
                luaSB.AppendLine("}");
            }

            if(textList.Count>0)
            {
                luaSB.AppendLine("local ConfigInnerText = {");
                foreach (var text in textList)
                {
                    luaSB.AppendLine($"    [[{text}]],");
                }
                luaSB.AppendLine("}");
            }

            ExportTableToLua(sheet, platform, strList, textList, out Dictionary<string, InnerTableData> tableDataDic);
            if(tableDataDic.Count>0)
            {
                foreach(var kvp in tableDataDic)
                {
                    luaSB.AppendLine(kvp.Value.luaStr);
                }
            }

            luaSB.AppendLine($"local {sheet.Name} = {{");
            
            foreach (var line in sheet.Line.lines)
            {
                string key = sheet.Field.fields[0].GetValue(line.cells[0]).ToString();
                luaSB.AppendLine($"    [{key}] = {{");

                for(int i =0;i<sheet.Field.fields.Count;i++)
                {
                    AField field = sheet.Field.fields[i];
                    if (!CanFieldExport(field, platform))
                    {
                        continue;
                    }
                    CellContent cellContent = line.cells[i];
                    var content = field.GetContent(cellContent);
                    var value = field.GetValue(cellContent);
                    if (value == null)
                    {
                        continue;
                    }

                    if(field.Type == FieldType.String || field.Type == FieldType.Res)
                    {
                        luaSB.AppendLine($"       {field.Name} = ConfigInnerString[{strList.IndexOf(content) + 1}],");
                    }else if(field.Type == FieldType.Stringt)
                    {
                        luaSB.AppendLine($"       {field.Name} = ConfigInnerText[{textList.IndexOf(content) + 1}],");
                    }else if(field.Type == FieldType.Dic || field.Type == FieldType.Array)
                    {
                        InnerTableData tableData = tableDataDic[content];
                        luaSB.AppendLine($"       {field.Name} = {tableData.paramName},");
                    }else if(field.Type == FieldType.Bool)
                    {
                        luaSB.AppendLine($"       {field.Name} = {value.ToString().ToLower()},");
                    }
                    else
                    {
                        luaSB.AppendLine($"       {field.Name} = {value.ToString()},");
                    }
                }

                luaSB.AppendLine("    },");
            }
            luaSB.AppendLine("}");
            luaSB.AppendLine($"return {sheet.Name}");

            string filePath = $"{outputDirPath}/{sheet.Name}.txt";
            File.WriteAllText(filePath, luaSB.ToString());

        }

        public static void ExportStringToLua(Sheet sheet,FieldPlatform platform,
            out List<string> strList,out List<string> textList)
        {
            strList = new List<string>();
            textList = new List<string>();

            foreach(var line in sheet.Line.lines)
            {
                for(int i =0;i<sheet.Field.fields.Count;i++)
                {
                    AField field = sheet.Field.fields[i];
                    if(!CanFieldExport(field,platform))
                    {
                        continue;
                    }

                    CellContent cellContent = line.cells[i];
                    var value = field.GetValue(cellContent);
                    if(value == null)
                    {
                        continue;
                    }
                    if (field.Type == FieldType.String || field.Type == FieldType.Res)
                    {
                        strList.Add(value.ToString());
                    } else if (field.Type == FieldType.Stringt)
                    {
                        textList.Add(value.ToString());
                    } else if (field.Type == FieldType.Array)
                    {
                        FieldType valueType = ((ArrayField)field).ValueFieldType;
                        if (valueType == FieldType.String || valueType == FieldType.Res)
                        {
                            var list = (IList)field.GetValue(cellContent);
                            foreach (var d in list)
                            {
                                strList.Add(d.ToString());
                            }
                        }
                        else if (valueType == FieldType.Stringt)
                        {
                            var list = (IList)field.GetValue(cellContent);
                            foreach (var d in list)
                            {
                                textList.Add(d.ToString());
                            }
                        }
                    } else if (field.Type == FieldType.Dic)
                    {
                        FieldType valueType = ((DicField)field).ValueFieldType;
                        if (valueType == FieldType.String || valueType == FieldType.Stringt || valueType == FieldType.Res)
                        {
                            IDictionary dic = (IDictionary)field.GetValue(cellContent);
                            IDictionaryEnumerator enumerator = dic.GetEnumerator();
                            while(enumerator.MoveNext())
                            {
                                if (valueType == FieldType.Stringt)
                                {
                                    textList.Add(enumerator.Value.ToString());
                                }
                                else if (valueType == FieldType.String || valueType == FieldType.Res)
                                {
                                    strList.Add(enumerator.Value.ToString());
                                }
                            }
                        }
                    }

                }
            }

            strList = strList.Distinct().ToList();
            textList = textList.Distinct().ToList();
        }

        private static void ExportTableToLua(Sheet sheet, FieldPlatform platform,
            List<string> strList,List<string> textList,out Dictionary<string, InnerTableData> tableDic)
        {
            tableDic = new Dictionary<string, InnerTableData>();

            int index = 1;
            foreach (var line in sheet.Line.lines)
            {
                for (int i = 0; i < sheet.Field.fields.Count; i++)
                {
                    AField field = sheet.Field.fields[i];
                    if (!CanFieldExport(field, platform))
                    {
                        continue;
                    }
                    if(field.Type != FieldType.Array && field.Type != FieldType.Dic)
                    {
                        continue;
                    }

                    CellContent cellContent = line.cells[i];
                    var content = field.GetContent(cellContent);
                    var value = field.GetValue(cellContent);
                    if(value == null)
                    {
                        continue;
                    }
                    Dictionary<string, InnerTableData> innerTableDic = null;
                    if(field.Type == FieldType.Array)
                    {
                        innerTableDic = tableDic;
                    }else
                    {
                        innerTableDic = tableDic;
                    }
                    if(!innerTableDic.TryGetValue(content,out InnerTableData tableData))
                    {
                        tableData = new InnerTableData();
                        tableData.content = content;
                        tableData.paramName = "InnerTable_" + index;

                        innerTableDic.Add(content, tableData);
                        ++index;

                        StringBuilder luaSB = new StringBuilder();
                        luaSB.AppendLine($"local {tableData.paramName} = {{");
                        if(field.Type == FieldType.Array)
                        {
                            ArrayField arrayField = (ArrayField)field;
                            IList list = (IList)value;
                            foreach(var d in list)
                            {
                                FieldType valueType = arrayField.ValueFieldType;
                                if(valueType == FieldType.String || valueType == FieldType.Res)
                                {
                                    luaSB.AppendLine($"    ConfigInnerString[{strList.IndexOf(d.ToString()) + 1}],");
                                }
                                else if(valueType == FieldType.Stringt)
                                {
                                    luaSB.AppendLine($"    ConfigInnerString[{textList.IndexOf(d.ToString()) + 1}],");
                                }
                                else if(valueType == FieldType.Bool)
                                {
                                    luaSB.AppendLine($"    {d.ToString().ToLower()},");
                                }else
                                {
                                    luaSB.AppendLine($"    {d.ToString()},");
                                }
                            }
                        }else
                        {
                            DicField dicField = (DicField)field;
                            IDictionary dic = (IDictionary)value;
                            IDictionaryEnumerator enumerator = dic.GetEnumerator();
                            while(enumerator.MoveNext())
                            {
                                var keyObj = enumerator.Key;
                                Type keyType = keyObj.GetType();
                                if(keyType.IsValueType && keyType.IsPrimitive)
                                {
                                    luaSB.Append($"    [{keyObj.ToString()}]=");
                                }else if(keyType == typeof(string))
                                {
                                    luaSB.Append($"    {keyObj.ToString()} = ");
                                }

                                FieldType valueType = dicField.ValueFieldType;
                                object d = enumerator.Value;
                                if (valueType == FieldType.String || valueType == FieldType.Res)
                                {
                                    luaSB.Append($"ConfigInnerString[{strList.IndexOf(d.ToString()) + 1}],");
                                }
                                else if (valueType == FieldType.Stringt)
                                {
                                    luaSB.Append($"ConfigInnerString[{textList.IndexOf(d.ToString()) + 1}],");
                                }
                                else if (valueType == FieldType.Bool)
                                {
                                    luaSB.AppendLine($"    {d.ToString().ToLower()},");
                                }
                                {
                                    luaSB.Append($"{d.ToString()},");
                                }
                                luaSB.Append("\n");
                            }
                        }
                        luaSB.AppendLine("}");

                        tableData.luaStr = luaSB.ToString();
                    }
                }
            }
        }

        private static bool CanFieldExport(AField field,FieldPlatform platform)
        {
            if (platform != FieldPlatform.All)
            {
                if (platform == FieldPlatform.Client && field.Platform == FieldPlatform.Server)
                {
                    return false;
                }
                else if (platform == FieldPlatform.Server && field.Platform == FieldPlatform.Client)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
