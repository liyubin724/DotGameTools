using CommandLine;
using Dot.Tools.ETD;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Exporter;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Console = Colorful.Console;

namespace DotExcelToDataConsole
{
    public enum ETDFormat
    {
        Json,
        Lua,
    }

    public class ETDOption
    {
        [Option('f',"file",Required =true,HelpText = "input files which must be excel(.xlsx|.xls)")]
        public IEnumerable<string> Files { get; set; }

        [Option('o',"output",Required =false,HelpText ="output dir")]
        public string OutputDir { get; set; }
        
        [Option("format",Required =false,HelpText ="Json, Lua,All")]
        public ETDFormat Format { get; set; }

        [Option("platform",Required =false,HelpText ="Platform(Server,Client,All)")]
        public FieldPlatform Platform { get; set; }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Files = ");
            IEnumerator<string> enumerator = Files.GetEnumerator();
            while(enumerator.MoveNext())
            {
                str.Append(enumerator.Current+",");
            }
            str.Append("\n");
            str.AppendLine($"Output = {OutputDir}");
            str.AppendLine($"Format = {Format}");
            str.AppendLine($"Platform = {Platform}");
            return str.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ETDOption>(args).WithParsed(Run);
        }

        static void Log(LogType logType,int logID,string message)
        {
            string formatMess = $"[{logType,-7}]    [{logID,5}]    {message}";
            Color messColor = Color.White;

            if (logType == LogType.Verbose)
            {
                messColor = Color.GreenYellow;
            }
            else if(logType == LogType.Info)
            {
                messColor = Color.White;
            }
            else if(logType == LogType.Warning)
            {
                messColor = Color.Yellow;
            }
            else if(logType == LogType.Error)
            {
                messColor = Color.Red;
            }

            Console.WriteLine(formatMess, messColor);
        }

        static void Run(ETDOption option)
        {
            List<Workbook> books = new List<Workbook>();
            ETDProxy proxy = new ETDProxy(Log);

            IEnumerator<string> enumerator = option.Files.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string excelPath = enumerator.Current;

                Workbook book = proxy.ReadWorkbookFromExcel(excelPath);
                books.Add(book);
            }

            foreach (var book in books)
            {
                proxy.VerifyWorkbook(book);
            }

            Console.ReadKey();
        
        }
    }
}
