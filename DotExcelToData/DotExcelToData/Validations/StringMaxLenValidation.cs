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
        public CellContent cell;

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

        public ResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "StringMaxLenValidatoin::Verify->Argument is null!";
                return ResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                return ResultCode.Pass;
            }

            if(content.Length > maxLen)
            {
                msg = $"StringMaxLenValidatoin::Verify->Out of maxLen.Row = {cell.Row},Col = {cell.Col},Content = {content},Compare={content.Length}>{maxLen}";
                return ResultCode.MaxLenError;
            }
            return ResultCode.Success;
        }
    }
}
