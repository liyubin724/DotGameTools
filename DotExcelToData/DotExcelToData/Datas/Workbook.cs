using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Verify;
using ExtractInject;
using System.Collections.Generic;
using System.IO;

namespace Dot.Tools.ETD.Datas
{
    public class Workbook : IVerify,IEIContextObject
    {
        public string bookPath;
        private List<Sheet> sheets = new List<Sheet>();

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(bookPath);
            }
        }

        public int SheetCount { get => sheets.Count; }

        public Workbook(string path)
        {
            this.bookPath = path;
        }

        public Sheet GetSheetByName(string sheetName)
        {
            foreach(var sheet in sheets)
            {
                if(sheet.name == sheetName)
                {
                    return sheet;
                }
            }
            return null;
        }

        public Sheet GetSheetByIndex(int index)
        {
            if(index>=0&&index<sheets.Count)
            {
                return sheets[index];
            }
            return null;
        }

        public void AddSheet(Sheet sheet)
        {
            sheets.Add(sheet);
        }

        public bool Verify(IEIContext context)
        {
            LogHandler logHandler = context.GetObject<LogHandler>();

            logHandler.Log(LogType.Info, LogConst.LOG_WORKBOOK_VERIFY_START, bookPath);

            bool result = true;

            context.AddObject<Workbook>(this);

            foreach(var sheet in sheets)
            {
                context.AddObject<Sheet>(sheet);

                bool isValid = sheet.Verify(context);
                if(result && !isValid)
                {
                    result = false;
                }

                context.DeleteObject<Sheet>();
            }
            context.DeleteObject<Workbook>();

            logHandler.Log(LogType.Info, LogConst.LOG_WORKBOOK_VERIFY_END, bookPath,result);
            return result;
        }
    }
}
