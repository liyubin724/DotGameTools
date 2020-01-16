using Dot.Serialize.Lua;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dot.Tools.ETD.Exporter
{
    public static class LuaExporter
    {
        private static void ExportLineToLua(StringBuilder luaSB, SheetLine line,List<AField> fields,int indent,FieldPlatform platform)
        {
            string indentStr = GetIndentStr(indent);

            string key = fields[0].GetContent(line.cells[0]);
            luaSB.AppendLine($"{indentStr}[{key}] = {{");

            indentStr = GetIndentStr(indent + 1);
            for (int i = 0; i < fields.Count; i++)
            {
                AField field = fields[i];
                if (platform != FieldPlatform.All)
                {
                    if (platform == FieldPlatform.Client && field.Platform == FieldPlatform.Server)
                    {
                        continue;
                    }
                    else if (platform == FieldPlatform.Server && field.Platform == FieldPlatform.Client)
                    {
                        continue;
                    }
                }

                LineCell cellContent = line.cells[i];
                var value = field.GetValue(cellContent);
                if(value!=null)
                {
                    Type type = value.GetType();
                    if(type.IsValueType && type.IsPrimitive)
                    {
                        luaSB.AppendLine($"{indentStr}{field.Name} = {value.ToString().ToLower()},");
                    }else if(type == typeof(string))
                    {
                        luaSB.AppendLine($"{indentStr}{field.Name} = [[{value.ToString()}]],");
                    }else if(typeof(IList).IsAssignableFrom(type) || typeof(IDictionary).IsAssignableFrom(type))
                    {
                        string luaStr = LuaSerializeWriter.WriteToLua(value);
                        string[] splitStr = luaStr.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        for(int m = 0;m<splitStr.Length;m++)
                        {
                            if(m == 0)
                            {
                                luaSB.AppendLine($"{indentStr}{field.Name} = {splitStr[m]}");
                            }else if(m == splitStr.Length -1)
                            {
                                luaSB.AppendLine($"{indentStr}{splitStr[m]},");
                            }
                            else
                            {
                                luaSB.AppendLine($"{indentStr}{splitStr[m]}");
                            }
                        }
                    }
                }
            }
            indentStr = GetIndentStr(indent);
            luaSB.AppendLine($"{indentStr}}},");
        }

        private static string GetIndentStr(int indent)
        {
            StringBuilder indentSB = new StringBuilder();
            for (int i = 0; i < indent; i++)
            {
                indentSB.Append("    ");
            }
            return indentSB.ToString();
        }

        public static void Export(string outputDirPath, Sheet sheet,FieldPlatform platform)
        {
            if (!Directory.Exists(outputDirPath))
            {
                if (!Directory.CreateDirectory(outputDirPath).Exists)
                {
                    return;
                }
            }

            StringBuilder luaSB = new StringBuilder();
            luaSB.AppendLine($"local {sheet.name} = {{");
            foreach(var line in sheet.Line.lines)
            {
                ExportLineToLua(luaSB, line, sheet.Field.fields, 1, platform);
            }
            luaSB.AppendLine("}");
            luaSB.AppendLine($"return {sheet.name}");

            string filePath = $"{outputDirPath}/{sheet.name}.txt";
            File.WriteAllText(filePath, luaSB.ToString());
        }
    }
}
