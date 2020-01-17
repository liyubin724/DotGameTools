using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Utils;
using ExtractInject;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Validations
{
    public class RangeValidation : IValidation
    {
        private const string RANGE_REGEX = @"Range\{(?<min>[-]{0,1}[0-9.]+),(?<max>[-]{0,1}[0-9.]+)\}";

        [EIField(EIFieldUsage.In, false)]
        public AFieldData field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        private string rule = string.Empty;
        public void SetRule(string rule)
        {
            this.rule = rule;
        }

        public ValidationResultCode Verify(EIContext context)
        {
            LogHandler logHandler = context.GetObject<LogHandler>();

            if (FieldTypeUtil.IsNumberType(field.Type) ||
                field.Type == FieldType.Array && FieldTypeUtil.IsNumberType(((ArrayFieldData)field).ValueType))
            {
                if (field == null || cell == null)
                {
                    logHandler.Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);

                    return ValidationResultCode.ArgIsNull;
                }

                string content = cell.GetContent(field);
                if (string.IsNullOrEmpty(content))
                {
                    logHandler.Log(LogType.Warning, LogConst.LOG_VALIDATION_NULL, cell.row, cell.col);
                    return ValidationResultCode.ContentIsNull;
                }



            }
            else
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_TYPE_FOR_RANGE_ERROR,cell.row,cell.col,field.Type);
                return ValidationResultCode.NumberRangeError;
            }
        }


        //--------------------------------

        private bool isValid = true;
        public bool IsValid => isValid;

        public string ErrorMsg { get; set; }

        public void SetData(string rule)
        {
            if(field.Type != FieldType.Int && field.Type != FieldType.Ref)
            {
                ErrorMsg = "FileType is not <FieldType.Int/FieldType.Ref>";
                isValid = false;
                return;
            }

            Match match = new Regex(RANGE_REGEX).Match(rule);
            Group group = match.Groups["min"];
            if (group.Success)
            {
                if (!int.TryParse(group.Value, out min))
                {
                    ErrorMsg = "Parse Min value error";
                    isValid = false;
                }
            }
            group = match.Groups["max"];
            if (group.Success)
            {
                if (!int.TryParse(group.Value, out max))
                {
                    ErrorMsg = "Parse Max value error";
                    isValid = false;
                }
            }

            if(max<min)
            {
                ErrorMsg = "the value of max is less then min";
                isValid = false;
            }
        }

        public ValidationResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "IntRangeValidation::Verify->Argument is null!";
                return ValidationResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"IntRangeValidation::Verify->Cell Content is null. Row = {cell.row},Col = {cell.col}.";
                return ValidationResultCode.ContentIsNull;
            }

            if (!int.TryParse(content, out int value))
            {
                msg = $"IntRangeValidation::Verify->Parse content error.Row = {cell.row},Col = {cell.col},Content = {content}";
                return ValidationResultCode.ParseContentFailed;
            }
            else
            {
                if (value >= min && value <= max)
                {
                    return ValidationResultCode.Success;
                }
                else
                {
                    msg = $"IntRangeValidation::Verify->Compare error.Row = {cell.row},Col = {cell.col},Compare={min}--{value}--{max}";
                    return ValidationResultCode.NumberRangeError;
                }
            }
        }
    }
}
