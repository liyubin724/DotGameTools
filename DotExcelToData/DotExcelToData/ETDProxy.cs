using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.IO;
using Dot.Tools.ETD.Log;
using ExtractInject;

namespace Dot.Tools.ETD
{
    public class ETDProxy
    { 
        private EIContext context = new EIContext();
        public ETDProxy(OnHandlerLog logCallback)
        {
            LogHandler handler = new LogHandler(logCallback);
            handler.AddToContext(context);

            WorkbookAssembly bookAssembly = new WorkbookAssembly(context);
            bookAssembly.AddToContext(context);
        }

        public void ReadWorkbookForDir(string excelDir)
        {
            WorkbookAssembly workbookAssembly = context.GetObject<WorkbookAssembly>();
            workbookAssembly.ReadBooksFromDir(excelDir);
        }

        public void WriteWorkbook(string outputDir,ETDWriterFormat format, ETDWriterTarget target,bool isOptimize,bool isVerify)
        {
            WorkbookAssembly workbookAssembly = context.GetObject<WorkbookAssembly>();
            workbookAssembly.WriteTo(outputDir, format, target, isOptimize, isVerify);
        }
    }
}
