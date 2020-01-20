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

                string filePath = $"{outputDir}/{book.Name}_{sheet.name}{IOConst.LUA_EXTERSION}";

                Dictionary<AFieldData, string> defaultDic = FindSheetDefaultValue(sheet, platform);
                List<string> stringList = FindSheetString(sheet, platform);

            }
        }

        private static Dictionary<AFieldData,string> FindSheetDefaultValue(Sheet sheet,FieldPlatform platform)
        {
            Dictionary<AFieldData, string> defaultValueDic = new Dictionary<AFieldData, string>();

            List<AFieldData> fields = new List<AFieldData>();
            for (int f = 0; f < sheet.FieldCount; ++f)
            {
                AFieldData field = sheet.GetFieldByIndex(f);
                if (field.Platform == FieldPlatform.All || field.Platform == platform)
                {
                    fields.Add(field);
                }
            }
            for(int i =0;i<fields.Count;++i)
            {
                AFieldData field = fields[i];
                Dictionary<string, int> contentRepeatDic = new Dictionary<string, int>();
                for(int j = 0;j<sheet.LineCount;++j)
                {
                    SheetLine line = sheet.GetLineByIndex(j);
                    LineCell cell = line.GetCellByCol(field.col);
                    string content = cell.GetContent(field);

                    if(contentRepeatDic.ContainsKey(content))
                    {
                        contentRepeatDic[content] += 1;
                    }else
                    {
                        contentRepeatDic.Add(content, 1);
                    }
                }

                string rContent = string.Empty;
                int maxCount = 0;
                foreach(var kvp in contentRepeatDic)
                {
                    if(kvp.Value>maxCount)
                    {
                        maxCount = kvp.Value;
                        rContent = kvp.Key;
                    }
                }

                if(maxCount>1)
                {
                    defaultValueDic.Add(field, rContent);
                }
            }
            return defaultValueDic;
        }

        private static List<string> FindSheetString(Sheet sheet,FieldPlatform platform)
        {
            List<string> result = new List<string>();
            List<AFieldData> fields = new List<AFieldData>();
            for (int f = 0; f < sheet.FieldCount; ++f)
            {
                AFieldData field = sheet.GetFieldByIndex(f);
                if (field.Platform == FieldPlatform.All || field.Platform == platform)
                {
                    fields.Add(field);
                }
            }
            for (int i = 0; i < fields.Count; ++i)
            {
                AFieldData field = fields[i];
                if(IsStringField(field))
                {
                    for (int j = 0; j < sheet.LineCount; ++j)
                    {
                        SheetLine line = sheet.GetLineByIndex(j);
                        LineCell cell = line.GetCellByCol(field.col);

                        string[] values = GetStringCellContent(field, cell);
                        if(values!=null && values.Length>0)
                        {
                            result.AddRange(values);
                        }
                    }
                }
            }
            result = result.Distinct().ToList();
            return result;
        }

        private static bool IsStringField(AFieldData field)
        {
            if(field.Type == FieldType.String || field.Type == FieldType.Res || field.Type == FieldType.Stringt)
            {
                return true;
            }
            if(field.Type == FieldType.Array)
            {
                ArrayFieldData arrayField = (ArrayFieldData)field;

                if (arrayField.Type == FieldType.String || arrayField.Type == FieldType.Res || arrayField.Type == FieldType.Stringt)
                {
                    return true;
                }
            }
            if(field.Type == FieldType.Dic)
            {
                DicFieldData dicField = (DicFieldData)field;

                if (dicField.ValueType == FieldType.String || dicField.ValueType == FieldType.Res || dicField.ValueType == FieldType.Stringt)
                {
                    return true;
                }
            }
            return false;
        }

        private static string[] GetStringCellContent(AFieldData field,LineCell cell)
        {
            List<string> contents = new List<string>();
            string content = cell.GetContent(field);

            if (field.Type == FieldType.String || field.Type == FieldType.Res || field.Type == FieldType.Stringt)
            {
                contents.Add(content);
            }else if (field.Type == FieldType.Array){
                ArrayFieldData arrayField = (ArrayFieldData)field;
                if (arrayField.Type == FieldType.String || arrayField.Type == FieldType.Res || arrayField.Type == FieldType.Stringt)
                {
                    string[] splitStr = content.Split(new char[] { '[',',',']' }, StringSplitOptions.RemoveEmptyEntries);
                    contents.AddRange(splitStr);
                }
            }
            if (field.Type == FieldType.Dic)
            {
                DicFieldData dicField = (DicFieldData)field;
                if (dicField.ValueType == FieldType.String || dicField.ValueType == FieldType.Res || dicField.ValueType == FieldType.Stringt)
                {
                    string[] splitStr = content.Split(new char[] { '[', ';', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var str in splitStr)
                    {
                        string[] splitValue = str.Split(new char[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);
                        if(splitValue.Length == 2)
                        {
                            contents.Add(splitValue[1]);
                        }
                    }
                }
            }
            return contents.ToArray();
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
