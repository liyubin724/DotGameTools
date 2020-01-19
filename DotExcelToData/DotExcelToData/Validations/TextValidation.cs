using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class TextValidation : IValidation
    {
        [EIField(EIFieldUsage.In,false)]
        private LogHandler logHandler;

        [EIField(EIFieldUsage.In, false)]
        public AFieldData field;

        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        [EIField(EIFieldUsage.In,false)]
        public Workbook book;

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
                return ValidationResultCode.Success;
            }

            Sheet sheet = book.GetSheetByName(SheetConst.TEXT_BOOK_NAME);
            if(sheet == null)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_TEXT_NOT_FOUND_ERROR);
                return ValidationResultCode.TextNotFoundError;
            }

            for(int i =0;i<sheet.LineCount; ++i)
            {
                string id = sheet.GetLineIDByIndex(i);
                if(id == content)
                {
                    return ValidationResultCode.Success;
                }
            }

            logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_TEXT_ID_NOT_FOUND_ERROR, content);

            return ValidationResultCode.TextIDNotFoundError;
        }
    }
}
