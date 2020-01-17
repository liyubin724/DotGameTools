﻿using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Verify;
using ExtractInject;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Datas
{
    public class Sheet : IEIContextObject,IVerify
    {
        public string name;

        private List<AFieldData> fields = new List<AFieldData>();
        private List<SheetLine> lines = new List<SheetLine>();

        public Sheet(string n)
        {
            name = n;
        }

        public int LineCount { get => lines.Count; }
        public int FieldCount { get => fields.Count; }

        public bool Verify(IEIContext context)
        {
            LogHandler logHandler = context.GetObject<LogHandler>();

            logHandler.Log(LogType.Info, LogConst.LOG_SHEET_VERIFY_START, name);

            bool result = true;

            foreach(var field in fields)
            {
                bool isValid = field.Verify(context);
                if(!isValid)
                {
                    result = false;
                }
            }

            foreach(var line in lines)
            {
                logHandler.Log(LogType.Info, LogConst.LOG_LINE_VERIFY_START,line.ToString());

                bool lineResult = true;

                if (line.CellCount != fields.Count)
                {
                    logHandler.Log(LogType.Error, LogConst.LOG_LINE_COUNT_NOT_EQUAL);
                    lineResult = false;
                }else
                {
                    foreach(var field in fields)
                    {

                    }
                }
                logHandler.Log(LogType.Info, LogConst.LOG_LINE_VERIFY_END, lineResult);

                if(!lineResult)
                {
                    result = false;
                }
            }

            logHandler.Log(LogType.Info, LogConst.LOG_SHEET_VERIFY_END, name,result);
            return result;
        }

        public void AddField(AFieldData field)
        {
            fields.Add(field);
        }

        public AFieldData GetFieldByCol(int col)
        {
            foreach(var field in fields)
            {
                if(field.col == col)
                {
                    return field;
                }
            }
            return null;
        }

        public AFieldData GetFieldByIndex(int index)
        {
            if(index>=0&&index<fields.Count)
            {
                return fields[index];
            }
            return null;
        }

        public void AddLine(SheetLine line)
        {
            lines.Add(line);
        }

        public SheetLine GetLineByRow(int row)
        {
            foreach(var line in lines)
            {
                if(line.row == row)
                {
                    return line;
                }
            }
            return null;
        }

        public SheetLine GetLineByIndex(int index)
        {
            if(index>=0&&index<lines.Count)
            {
                return lines[index];
            }
            return null;
        }
    }
}
