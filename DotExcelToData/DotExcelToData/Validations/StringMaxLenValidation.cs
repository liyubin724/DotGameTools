using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Validations
{
    public class StringMaxLenValidation: IValidation
    {
        private const string MAX_LEN_REGEX = @"StringMaxLen{(?<len>[0-9]+)}";

        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        private bool isValid = true;
        public bool IsValid => isValid;

        public string ErrorMsg { get; set; }

        public int maxLen = int.MaxValue;
        public void SetData(string rule)
        {
            if(field.Type != FieldType.String && field.Type != FieldType.Stringt && field.Type != FieldType.Res)
            {
                ErrorMsg = "FileType is not <FieldType.String/FieldType.Stringt/FieldType.Res>";
                isValid =false;
                return;
            }

            Match match = new Regex(MAX_LEN_REGEX).Match(rule);
            Group group = match.Groups["len"];
            if (group.Success)
            {
                if (!int.TryParse(group.Value, out maxLen))
                {
                    ErrorMsg = "Parse Len value error";
                    isValid = false;
                }
            }
        }

        public ValidationResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "StringMaxLenValidatoin::Verify->Argument is null!";
                return ValidationResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                return ValidationResultCode.Pass;
            }

            if(content.Length > maxLen)
            {
                msg = $"StringMaxLenValidatoin::Verify->Out of maxLen.Row = {cell.row},Col = {cell.col},Content = {content},Compare={content.Length}>{maxLen}";
                return ValidationResultCode.MaxLenError;
            }
            return ValidationResultCode.Success;
        }
    }
}
