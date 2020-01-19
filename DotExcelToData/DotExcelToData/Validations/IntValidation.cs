using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class IntValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        private LogHandler logHandler;

        [EIField(EIFieldUsage.In, false)]
        public AFieldData field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        public void SetRule(string rule)
        {
        }

        public ValidationResultCode Verify()
        {
            if (field == null || cell == null)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);

                return ValidationResultCode.ArgIsNull;
            }

            string content = cell.GetContent(field);
            if (string.IsNullOrEmpty(content))
            {
                logHandler.Log(LogType.Warning, LogConst.LOG_VALIDATION_SET_DEFAULT,"0", cell.row, cell.col);
                content = cell.value = "0";
            }

            if (!int.TryParse(content, out int value))
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_CONVERT_ERROR, "int",cell.ToString());
                return ValidationResultCode.ParseContentFailed;
            }

            return ValidationResultCode.Success;
        }
    }
}
