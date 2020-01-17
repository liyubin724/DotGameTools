using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Utils;
using ExtractInject;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Validations
{
    public class DicKeyValidation : IValidation
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
                return ValidationResultCode.Success;
            }

            if(content[0]!='{' || content[content.Length-1]!='}')
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_DIC_FORMAT_ERROR,cell.ToString());
                return ValidationResultCode.ContentDicFormatError;
            }

            string[] values = ContentUtil.SplitContent(content, new char[] { ',', ';' });
            if(values!=null && values.Length%2!=0)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_DIC_KV_COUNT_ERROR, cell.ToString());
                return ValidationResultCode.ContentDicKeyValueCountError;
            }

            List<string> keys = new List<string>();
            for(int  i=0;i<values.Length;i+=2)
            {
                if(keys.Contains(values[i]))
                {
                    logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_DIC_KEY_REPEAT_ERROR, cell.ToString());
                    return ValidationResultCode.ContentDicKeyRepeatError;
                }else
                {
                    keys.Add(values[i]);
                }
            }

            return ValidationResultCode.Success;
        }


    }
}
