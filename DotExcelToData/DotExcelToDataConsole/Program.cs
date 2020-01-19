using CommandLine;
using Dot.Tools.ETD;
using Dot.Tools.ETD.IO;
using Dot.Tools.ETD.Log;
using System.Drawing;
using System.IO;
using Console = Colorful.Console;

namespace DotExcelToDataConsole
{
    public class ETDOption
    {
        [Option('i',"input",Required =true,HelpText ="Input dir which contains mulitable excel files(.xlsx|.xls)")]
        public string InputDir { get; set; }

        [Option('o',"output",Required =true,HelpText ="Output dir")]
        public string OutputDir { get; set; }
        
        [Option('f',"format",Required =true,HelpText ="Export Format(Json, Lua,All)")]
        public ETDWriterFormat Format { get; set; }

        [Option('p',"target",Required =true,HelpText ="Target(Server,Client,All)")]
        public ETDWriterTarget Target { get; set; }

        [Option("optimize",Required =false,HelpText ="Is Need Optimize")]
        public bool IsOptimize { get; set; } = false;

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
            if(logID == 0)
            {
                Console.WriteLine();

                writer?.WriteLine();
                return;
            }

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

            writer?.WriteLine(formatMess);
        }

        public void Start(ETDOption option)
        {
            if (!string.IsNullOrEmpty(this.option.LogFilePath))
            {
                writer = new StreamWriter(this.option.LogFilePath, false);
                writer.AutoFlush = true;
            }

            if (Directory.Exists(option.InputDir))
            {
                ETDProxy proxy = new ETDProxy(OnLogReceived);
                proxy.ReadWorkbookForDir(option.InputDir);

                proxy.WriteWorkbook(option.OutputDir, option.Format, option.Target, option.IsOptimize, true);
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
