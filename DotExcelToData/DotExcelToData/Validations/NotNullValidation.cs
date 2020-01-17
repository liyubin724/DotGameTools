using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class NotNullValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AFieldData field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        public void SetRule(string rule)
        {
        }

        public ValidationResultCode Verify(IEIContext context)
        {
            LogHandler logHandler = context.GetObject<LogHandler>();

            if (field == null || cell == null)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);

                return ValidationResultCode.ArgIsNull;
            }

            string content = cell.GetContent(field);
            if (string.IsNullOrEmpty(content))
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_NULL, cell.row, cell.col);
                return ValidationResultCode.ContentIsNull;
            }

            return ValidationResultCode.Success;
        }
    }
}
