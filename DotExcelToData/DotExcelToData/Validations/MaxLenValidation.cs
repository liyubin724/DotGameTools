using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Validations
{
    public class MaxLenValidation: IValidation
    {
        private const string MAX_LEN_REGEX = @"StringMaxLen{(?<len>[0-9]+)}";

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

            int maxLen = 0;
            Match match = new Regex(MAX_LEN_REGEX).Match(rule);
            Group group = match.Groups["len"];
            if (group.Success)
            {
                if (!int.TryParse(group.Value, out maxLen))
                {
                    maxLen = 0;
                }
            }
            if(maxLen<=0)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_FORMAT_ERROR, cell.col,rule);
                return ValidationResultCode.ValidationFormatError;
            }

            string content = cell.GetContent(field);
            if (string.IsNullOrEmpty(content))
            {
                return ValidationResultCode.Success;
            }

            if(content.Length>maxLen)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_LEN_ERROR,maxLen, cell.row, cell.col,content);
                return ValidationResultCode.MaxLenError;
            }
            return ValidationResultCode.Success;
        }
    }
}
