using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Validations;
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
            if(FieldCount == 0)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_SHEET_FIELD_EMPTY);
                result = false;
            }

            if(result)
            {
                context.AddObject<Sheet>(this);

                foreach (var field in fields)
                {
                    bool isValid = field.Verify(context);
                    if (!isValid)
                    {
                        result = false;
                    }
                }

                foreach (var line in lines)
                {
                    logHandler.Log(LogType.Info, LogConst.LOG_LINE_VERIFY_START, line.row);

                    bool lineResult = true;

                    if (line.CellCount != fields.Count)
                    {
                        logHandler.Log(LogType.Error, LogConst.LOG_LINE_COUNT_NOT_EQUAL);
                        lineResult = false;
                    }
                    else
                    {
                        for (int i = 0; i < fields.Count; ++i)
                        {
                            AFieldData field = fields[i];
                            LineCell cell = line.GetCellByIndex(i);

                            context.AddObject<AFieldData>(field);
                            context.AddObject<LineCell>(cell);

                            IValidation[] validations = field.GetValidations();
                            foreach (var v in validations)
                            {
                                if (v.GetType() != typeof(ErrorValidation))
                                {
                                    EIUtil.Inject(context, v);

                                    ValidationResultCode resultCode = v.Verify();
                                    if (resultCode != ValidationResultCode.Success)
                                    {
                                        lineResult = false;
                                    }
                                }
                            }

                            context.DeleteObject<AFieldData>();
                            context.DeleteObject<LineCell>();
                        }
                    }
                    logHandler.Log(LogType.Info, LogConst.LOG_LINE_VERIFY_END, lineResult);

                    if (!lineResult)
                    {
                        result = false;
                    }
                }

                context.DeleteObject<Sheet>();
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

        public string GetLineIDByIndex(int index)
        {
            SheetLine line = GetLineByIndex(index);
            if(line!=null)
            {
                return line.GetCellByIndex(0).GetContent(fields[0]);
            }
            return null;
        }

        public string GetLineIDByRow(int row)
        {
            SheetLine line = GetLineByRow(row);
            if (line != null)
            {
                return line.GetCellByIndex(0).GetContent(fields[0]);
            }
            return null;
        }
    }
}
