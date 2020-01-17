using CommandLine;
using Dot.Tools.ETD;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Console = Colorful.Console;

namespace DotExcelToDataConsole
{
    public enum ETDFormat
    {
        ALL,
        Json,
        Lua,
    }

    public class ETDOption
    {
        [Option('i',"input",Required =true,HelpText ="Input dir which contains mulitable excel files(.xlsx|.xls)")]
        public string InputDir { get; set; }

        [Option('o',"output",Required =true,HelpText ="Output dir")]
        public string OutputDir { get; set; }
        
        [Option('f',"format",Required =true,HelpText ="Export Format(Json, Lua,All)")]
        public ETDFormat Format { get; set; }

        [Option('p',"platform",Required =true,HelpText ="Platform(Server,Client,All)")]
        public FieldPlatform Platform { get; set; }

        [Option("log-level", Required = false, HelpText = "Log Level(Verbose,Info,Warning,Error)")]
        public LogType LogLevel { get; set; } = LogType.Info;

        [Option("log-path",Required =false,HelpText ="Log file")]
        public string LogFilePath { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ETDOption>(args).WithParsed(Run);
        }

        static void Run(ETDOption option)
        {
            Program program = new Program(option);
            program.Start(option);

            Console.ReadKey();
        }

        private ETDOption option = null;
        private StreamWriter writer = null;
        public Program(ETDOption option)
        {
            this.option = option;
        }

        private void OnLogReceived(LogType logType, int logID, string message)
        {
            string formatMess = $"[{logType,-7}]    [{logID,5}]    {message}";

            if (logType>=option.LogLevel)
            {
                Color messColor = Color.White;
                if (logType == LogType.Verbose)
                {
                    messColor = Color.GreenYellow;
                }
                else if (logType == LogType.Info)
                {
                    messColor = Color.White;
                }
                else if (logType == LogType.Warning)
                {
                    messColor = Color.Yellow;
                }
                else if (logType == LogType.Error)
                {
                    messColor = Color.Red;
                }

                Console.WriteLine(formatMess, messColor);
            }

            if(writer!=null)
            {
                writer.WriteLine(formatMess);
            }
        }

        public void Start(ETDOption option)
        {
            if (!string.IsNullOrEmpty(this.option.LogFilePath))
            {
                writer = new StreamWriter(this.option.LogFilePath, false);
                writer.AutoFlush = true;
            }

            List<Workbook> books = new List<Workbook>();
            ETDProxy proxy = new ETDProxy(OnLogReceived);

            if (!Directory.Exists(option.InputDir))
            {
                return;
            }

            List<string> fileList = new List<string>();
            string[] files = Directory.GetFiles(option.InputDir, "*.xls", SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
            {
                fileList.AddRange(files);
            }
            files = Directory.GetFiles(option.InputDir, "*.xlsx", SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
            {
                fileList.AddRange(files);
            }

            foreach (var file in fileList)
            {
                Workbook book = proxy.ReadWorkbookFromExcel(file);
                books.Add(book);
            }

            foreach (var book in books)
            {
                proxy.VerifyWorkbook(book);
            }


            if(writer!=null)
            {
                writer.Flush();
                writer.Close();
                writer = null;
            }
        }
    }
}
