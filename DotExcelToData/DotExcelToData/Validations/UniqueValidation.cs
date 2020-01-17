using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class UniqueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public Sheet sheet;
        [EIField(EIFieldUsage.In, false)]
        public AFieldData field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        public void SetRule(string rule)
        {
        }

        public ValidationResultCode Verify(EIContext context)
        {
            LogHandler logHandler = context.GetObject<LogHandler>();

            if (field == null || cell == null)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);

                return ValidationResultCode.ArgIsNull;
            }

            string content = cell.GetContent(field);

            bool isRepeat = false;
            for(int i =0;i<sheet.LineCount;++i)
            {
                SheetLine line = sheet.GetLineByIndex(i);
                if(line.row!=cell.row)
                {
                    LineCell tempCell = line.GetCellByCol(field.col);
                    string tempContent = tempCell.GetContent(field);
                    if(tempContent == content)
                    {
                        isRepeat = true;
                        logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_CONTENT_REPEAT_ERROR, cell.ToString(), tempCell.ToString());
                    }
                }
            }
            if(isRepeat)
            {
                return ValidationResultCode.ContentRepeatError;
            }
            return ValidationResultCode.Success;
        }


        public ValidationResultCode Verify(out string msg)
        {
            msg = null;
            if (field == null || cell == null || sheet == null)
            {
                msg = "UniqueValueValidation::Verify->Argument is null!";
                return ValidationResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if(string.IsNullOrEmpty(content))
            {
                msg = "UniqueValueValidation::Verify->Content is null";
                return ValidationResultCode.ContentIsNull;
            }

            //int index = sheet.Field.fields.IndexOf(field);

            //foreach(var line in sheet.Line.lines)
            //{
            //    LineCell cc = line.cells[index];
            //    if(cc.row == cell.row)
            //    {
            //        continue;
            //    }
            //    string c = field.GetContent(cc);
            //    if(c == content)
            //    {
            //        msg = $"UniqueValueValidation::Verify->Content is Repeat.content = {content},Row={cell.row}--{cc.row},Col = {cell.col}";
            //        return ValidationResultCode.ContentRepeatError;
            //    }
            //}

            return ValidationResultCode.Success;
        }
    }
}
