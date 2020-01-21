using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class LuaValidation : IValidation
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

        public ValidationResultCode Verify()
        {
            if (field == null || cell == null)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);
                return ValidationResultCode.ArgIsNull;
            }

            string content = cell.GetContent(field);
            if(string.IsNullOrEmpty(content))
            {
                return ValidationResultCode.Success;
            }

            if(content.StartsWith("function") && content.EndsWith("end"))
            {
                return ValidationResultCode.Success;
            }else
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_LUA_FORMAT_ERROR, cell.ToString());
                return ValidationResultCode.LuaFunctionError;
            }

        }
    }
}
