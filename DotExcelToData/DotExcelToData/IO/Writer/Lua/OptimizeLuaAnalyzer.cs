using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dot.Tools.ETD.IO
{
    public class SubSheetData
    {
        public int startID;
        public int endID;

        public Dictionary<AFieldData, List<string>> complexContentDic = new Dictionary<AFieldData, List<string>>();
        public List<SheetLine> lines = new List<SheetLine>();

        public int index;
        public SummarySheetData summarySheet;
        public SubSheetData(int index, SummarySheetData summarySheet)
        {
            this.index = index;
            this.summarySheet = summarySheet;
        }

        public string OutputName { get => $"{summarySheet.bookName}_{summarySheet.sheet.name}_{index+1}"; }
        public string OutputFilePath { get => $"{summarySheet.outputDir}/{summarySheet.OutputName}/{OutputName}{IOConst.LUA_EXTERSION}"; }

        public int[] GetLineIDs()
        {
            List<int> ids = new List<int>();
            foreach(var line in lines)
            {
                string strID = summarySheet.sheet.GetLineIDByRow(line.row);
                ids.Add(int.Parse(strID));
            }
            return ids.ToArray();
        }

        internal void FindSubSheetComplexContent()
        {
            Sheet sheet = summarySheet.sheet;
            FieldPlatform platform = summarySheet.platform;
            List<AFieldData> fields = new List<AFieldData>();
            for (int f = 0; f < sheet.FieldCount; ++f)
            {
                AFieldData field = sheet.GetFieldByIndex(f);
                if (field.Platform == FieldPlatform.All || field.Platform == platform)
                {
                    if (field.Type == FieldType.Array || field.Type == FieldType.Dic || field.Type == FieldType.Lua)
                    {
                        fields.Add(field);
                    }
                }
            }
            foreach (var field in fields)
            {
                List<string> contents = new List<string>();
                foreach (var line in lines)
                {
                    var content = line.GetCellByCol(field.col).GetContent(field);
                    if (content != null)
                    {
                        contents.Add(content);
                    }
                }

                contents = contents.Distinct().ToList();
                complexContentDic.Add(field, contents);
            }
        }
    }

    public class SummarySheetData
    {
        public string bookName;
        public string outputDir;
        public Sheet sheet;
        public FieldPlatform platform;

        public Dictionary<AFieldData, string> defalutDic = new Dictionary<AFieldData, string>();
        public List<string> strList = new List<string>();

        public List<string> strFieldNames = new List<string>();
             
        public List<SubSheetData> subSheets = new List<SubSheetData>();

        public SummarySheetData(string bookName,string outputDir,Sheet sheet,FieldPlatform platform)
        {
            this.bookName = bookName;
            this.outputDir = outputDir;
            this.sheet = sheet;
            this.platform = platform;
        }

        public string OutputName { get => $"{bookName}_{sheet.name}"; }
        public string OutputFilePath { get => $"{outputDir}/{OutputName}{IOConst.LUA_EXTERSION}"; }

        public bool IsNeedText
        {
            get
            {
                for (int f = 0; f < sheet.FieldCount; ++f)
                {
                    AFieldData field = sheet.GetFieldByIndex(f);
                    if (field.Platform == FieldPlatform.All || field.Platform == platform)
                    {
                        if (field.Type == FieldType.Text)
                        {
                            return true;
                        }
                        if (field.Type == FieldType.Array && ((ArrayFieldData)field).ValueType == FieldType.Text)
                        {
                            return true;
                        }
                        if (field.Type == FieldType.Dic && ((DicFieldData)field).ValueType == FieldType.Text)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private void SubSheet(int countForSub)
        {
            sheet.SortLineByID();

            int curIndex = 0;
            SubSheetData subSheet = null;
            while (curIndex < sheet.LineCount)
            {
                SheetLine line = sheet.GetLineByIndex(curIndex);
                string idStr = sheet.GetLineIDByIndex(curIndex);
                int id = int.Parse(idStr);
                if (curIndex % countForSub == 0)
                {
                    subSheet = new SubSheetData(subSheets.Count, this);
                    subSheets.Add(subSheet);

                    subSheet.startID = id;
                }

                subSheet.endID = id;

                subSheet.lines.Add(line);

                curIndex++;
            }
        }

        private void FindSheetDefault()
        {
            List<AFieldData> fields = new List<AFieldData>();
            for (int f = 0; f < sheet.FieldCount; ++f)
            {
                AFieldData field = sheet.GetFieldByIndex(f);
                if (field.Platform == FieldPlatform.All || field.Platform == platform)
                {
                    if (field.Type == FieldType.Array || field.Type == FieldType.Dic || field.Type == FieldType.Lua)
                    {
                        continue;
                    }
                    fields.Add(field);
                }
            }
            for (int i = 0; i < fields.Count; ++i)
            {
                AFieldData field = fields[i];
                Dictionary<string, int> contentRepeatDic = new Dictionary<string, int>();
                for (int j = 0; j < sheet.LineCount; ++j)
                {
                    SheetLine line = sheet.GetLineByIndex(j);
                    LineCell cell = line.GetCellByCol(field.col);
                    string content = cell.GetContent(field);

                    if (contentRepeatDic.ContainsKey(content))
                    {
                        contentRepeatDic[content] += 1;
                    }
                    else
                    {
                        contentRepeatDic.Add(content, 1);
                    }
                }

                string rContent = string.Empty;
                int maxCount = 0;
                foreach (var kvp in contentRepeatDic)
                {
                    if (kvp.Value > maxCount)
                    {
                        maxCount = kvp.Value;
                        rContent = kvp.Key;
                    }
                }

                if (maxCount > 1)
                {
                    defalutDic.Add(field, rContent);
                }
            }
        }

        private void FindSheetString()
        {
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
                if (FieldTypeUtil.IsStringType(field.Type))
                {
                    strFieldNames.Add(field.name);

                    for (int j = 0; j < sheet.LineCount; ++j)
                    {
                        SheetLine line = sheet.GetLineByIndex(j);
                        LineCell cell = line.GetCellByCol(field.col);
                        string content = cell.GetContent(field);
                        if (!string.IsNullOrEmpty(content))
                        {
                            strList.Add(content);
                        }
                    }
                }
            }
            strList = strList.Distinct().ToList();
        }

        internal void Analyzer(int countForSub)
        {
            SubSheet(countForSub);
            FindSheetDefault();
            FindSheetString();

            foreach(var subSheet in subSheets)
            {
                subSheet.FindSubSheetComplexContent();
            }
        }
    }

    public class OptimizeLuaAnalyzer
    {
        public static SummarySheetData OptimizeSheet(string bookName,string outputDir,Sheet sheet,int countInSub,FieldPlatform platform)
        {
            SummarySheetData summarySheet = new SummarySheetData(bookName,outputDir,sheet,platform);
            summarySheet.Analyzer(countInSub);
            return summarySheet;
        }
    }
}
