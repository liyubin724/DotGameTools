using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.IO;
using Dot.Tools.ETD.Log;
using ExtractInject;
using System.IO;

namespace Dot.Tools.ETD
{
    public class ETDProxy
    { 
        private EIContext context = new EIContext();

        public ETDProxy(OnHandlerLog logCallback)
        {
            LogHandler handler = new LogHandler(logCallback);
            handler.AddToContext(context);
        }

        private void Log(LogType type, int logID, params object[] datas)
        {
            context.GetObject<LogHandler>()?.Log(type, logID, datas);
        }

        public Workbook ReadWorkbookFromExcel(string excelPath)
        {
            if(string.IsNullOrEmpty(excelPath))
            {
                Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);
                return null;
            }

            if(!File.Exists(excelPath))
            {
                Log(LogType.Error, LogConst.LOG_FILE_NOT_EXIST, excelPath);
                return null;
            }

            string ext = Path.GetExtension(excelPath).ToLower();
            if(ext != ".xlsx" && ext!=".xls")
            {
                Log(LogType.Error, LogConst.LOG_FILE_NOT_EXCEL, excelPath);
                return null;
            }

            WorkbookReader.InitReader(context);

            Workbook book = WorkbookReader.ReadExcelToWorkbook(excelPath);

            WorkbookReader.DestroyReader();

            return book;
        }

        public bool VerifyWorkbook(Workbook workbook)
        {
            if(workbook!=null)
            {
                return workbook.Verify(context);
            }

            return false;
        }

        public void WriteWorkbook(Workbook workbook)
        {

        }
    }
}
