using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class BoolValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        private LogHandler logHandler;
        [EIField(EIFieldUsage.In, false)]
        private AFieldData field;
        [EIField(EIFieldUsage.In, false)]
        private LineCell cell;

        public void SetRule(string rule)
        {
        }

        ValidationResultCode IValidation.Verify()
        {
            if (field == null || cell == null)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);

                return ValidationResultCode.ArgIsNull;
            }

            string content = cell.GetContent(field);
            if (!bool.TryParse(content, out bool value))
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_CONVERT_ERROR,"bool", cell.ToString());
                return ValidationResultCode.ParseContentFailed;
            }
            return ValidationResultCode.Success;
        }
    }
}
