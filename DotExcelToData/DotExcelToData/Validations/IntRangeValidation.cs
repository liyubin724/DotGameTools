using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Validations
{
    public class IntRangeValidation : IValidation
    {
        private const string RANGE_REGEX = @"IntRange\{(?<min>[-]{0,1}[0-9]+),(?<max>[-]{0,1}[0-9]+)\}";

        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public CellContent cell;

        public int min;
        public int max;

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
                msg = $"IntRangeValidation::Verify->Cell Content is null. Row = {cell.Row},Col = {cell.Col}.";
                return ValidationResultCode.ContentIsNull;
            }

            if (!int.TryParse(content, out int value))
            {
                msg = $"IntRangeValidation::Verify->Parse content error.Row = {cell.Row},Col = {cell.Col},Content = {content}";
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
                    msg = $"IntRangeValidation::Verify->Compare error.Row = {cell.Row},Col = {cell.Col},Compare={min}--{value}--{max}";
                    return ValidationResultCode.NumberRangeError;
                }
            }
        }
    }
}
