using System;
using System.Text;

namespace Dot.Core.Config
{
    public class SimpleLuaExporter
    {
        private StringBuilder luaSB = new StringBuilder();
        public string Export(ConfigSheetData sheetData)
        {
            luaSB.Clear();
            if(sheetData!=null)
            {
                luaSB.AppendLine($"local {sheetData.name} = {{");
                ConfigFieldData firstFieldData = sheetData.fields[0];
                foreach(var line in sheetData.lines)
                {
                    luaSB.AppendLine($"    [{line.contents[0].GetContent(firstFieldData)}] = {{");

                    for(int i =0;i<line.contents.Length;i++)
                    {
                        ConfigFieldData fieldData = sheetData.fields[i];
                        string content = line.contents[i].GetContent(fieldData);
                        if(content == null || fieldData.DefineType == ConfigFieldType.None)
                        {
                            continue;
                        }
                        luaSB.Append($"        {fieldData.DefineName} = ");
                        if(fieldData.DefineType == ConfigFieldType.Int || fieldData.DefineType == ConfigFieldType.Float)
                        {
                            luaSB.AppendLine($"{content},");
                        }else if( fieldData.DefineType == ConfigFieldType.String)
                        {
                            luaSB.AppendLine($"\"{content}\",");
                        }else if(fieldData.DefineType == ConfigFieldType.Stringt)
                        {
                            luaSB.AppendLine($"[[{content}]],");
                        }else if(fieldData.DefineType == ConfigFieldType.Lua)
                        {
                            luaSB.AppendLine($"function()\n{content}\n        end,");
                        }else if(fieldData.DefineType == ConfigFieldType.Array)
                        {
                            luaSB.AppendLine("{");
                            string[] values = content.Split(new string[] { "[", "]", "##" }, StringSplitOptions.RemoveEmptyEntries);
                            if(values!=null && values.Length>0)
                            {
                                foreach(var v in values)
                                {
                                    if(fieldData.InnerFieldType == ConfigFieldType.Float || fieldData.InnerFieldType == ConfigFieldType.Int)
                                    {
                                        luaSB.AppendLine($"            {v},");
                                    }else if(fieldData.InnerFieldType == ConfigFieldType.String)
                                    {
                                        luaSB.AppendLine($"            \"{v}\",");
                                    }else if(fieldData.InnerFieldType == ConfigFieldType.Stringt)
                                    {
                                        luaSB.AppendLine($"            [[{content}]],");
                                    }
                                }
                            }
                            luaSB.AppendLine("        },");
                        }else if(fieldData.DefineType == ConfigFieldType.Dic)
                        {
                            luaSB.AppendLine("{");
                            string[] contents = content.Substring(1,content.Length-1).Split(new string[] { "##"}, StringSplitOptions.RemoveEmptyEntries);

                            luaSB.AppendLine("        },");
                        }
                    }

                    luaSB.AppendLine("    },");
                }
                luaSB.AppendLine("}");
                luaSB.AppendLine($"return {sheetData.name}");
            }


            return luaSB.ToString();
        }
    }
}
