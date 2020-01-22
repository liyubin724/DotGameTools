using Dot.Tools.ETD.IO;
using Dot.Tools.ETD.Log;
using ExtractInject;
using System.Collections.Generic;
using System.IO;

namespace Dot.Tools.ETD.Datas
{
    public class WorkbookAssembly : EIContextObject
    {
        private IEIContext context;
        private LogHandler logHandler;

        private List<Workbook> books = new List<Workbook>();

        public int BookCount { get => books.Count; }

        public WorkbookAssembly(IEIContext context)
        {
            this.context = context;
            logHandler = context.GetObject<LogHandler>();
        }

        public Workbook GetBookByIndex(int index)
        {
            if (index >= 0 && index<books.Count)
            {
                return books[index];
            }
            return null;
        }

        public Workbook GetBookByName(string bookName)
        {
            foreach(var book in books)
            {
                if(book.Name == bookName)
                {
                    return book;
                }
            }
            return null;
        }

        public Workbook[] ReadBooksFromDir(string excelDir)
        {
            if (string.IsNullOrEmpty(excelDir) || !Directory.Exists(excelDir))
            {
                return null;
            }

            List<Workbook> dirBooks = new List<Workbook>();

            List<string> fileList = new List<string>();
            string[] files = Directory.GetFiles(excelDir, "*.xls", SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
            {
                fileList.AddRange(files);
            }
            files = Directory.GetFiles(excelDir, "*.xlsx", SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
            {
                fileList.AddRange(files);
            }

            foreach (var file in fileList)
            {
                Workbook book = ReadBookFromFile(file);
                dirBooks.Add(book);
            }

            books.AddRange(dirBooks);

            return dirBooks.ToArray();
        }

        public Workbook ReadBookFromFile(string excelPath)
        {
            if (string.IsNullOrEmpty(excelPath))
            {
                logHandler.Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);
                return null;
            }

            if (!File.Exists(excelPath))
            {
                logHandler.Log(LogType.Error, LogConst.LOG_FILE_NOT_EXIST, excelPath);
                return null;
            }

            string ext = Path.GetExtension(excelPath).ToLower();
            if (ext != ".xlsx" && ext != ".xls")
            {
                logHandler.Log(LogType.Error, LogConst.LOG_FILE_NOT_EXCEL, excelPath);
                return null;
            }

            WorkbookReader.InitReader(context);
            Workbook book = WorkbookReader.ReadExcelToWorkbook(excelPath);
            WorkbookReader.DestroyReader();

            return book;
        }

        public void WriteTo(string outputDir, ETDWriterFormat format, ETDWriterTarget target,bool isOptimize = false,bool isVerify = true)
        {
            foreach(var book in books)
            {
                if(isVerify)
                {
                    if(book.Verify(context))
                    {
                        WriteBook(book,outputDir, format, target,isOptimize);
                    }else
                    {
                        logHandler.Log(LogType.Error, LogConst.LOG_WORKBOOK_VERIFY_FAILED);
                    }
                }else
                {
                    WriteBook(book,outputDir, format, target, isOptimize);
                }
            }
        }

        private void WriteBook(Workbook book,string outputDir,ETDWriterFormat format,ETDWriterTarget target, bool isOptimize)
        {
            if (format == ETDWriterFormat.All || format == ETDWriterFormat.Json)
            {
                JsonWriter.WriteBook(book, Path.Combine(outputDir,IOConst.JSON_DIR_NAME), target);
            }

            if (format == ETDWriterFormat.All || format == ETDWriterFormat.Lua)
            {
                string targetDir = outputDir;//Path.Combine(outputDir, IOConst.LUA_DIR_NAME);
                if (isOptimize)
                {

                    OptimizeLuaWriter.WriteBook(book, targetDir, target);
                }
                else
                {
                    LuaWriter.WriteBook(book, targetDir, target);
                }
            }
        }
    }
}
