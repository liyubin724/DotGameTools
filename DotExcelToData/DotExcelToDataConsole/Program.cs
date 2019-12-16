using CommandLine;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Exporter;
using Dot.Tools.ETD.Fields;
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

        [Option('o',"output",Required =false,HelpText ="out put dir")]
        public string OutputDir { get; set; }
        
        [Option("format",Required =false,HelpText ="Json or Lua")]
        public ETDFormat Format { get; set; }

        [Option("verify",Required =false,HelpText ="Is it need verify")]
        public bool IsVerify { get; set; }
        [Option("optimize",Required =false,HelpText ="Optimize")]
        public bool IsOptimize { get; set; }
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
            str.AppendLine($"IsVerify = {IsVerify}");
            str.AppendLine($"IsOptimize = {IsOptimize}");
            str.AppendLine($"Platform = {Platform}");
            return str.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ETDOption>(args).WithParsed(Run);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static void Run(ETDOption option)
        {
            List<Workbook> books = new List<Workbook>();

            IEnumerator<string> enumerator = option.Files.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine($"----Load Excel({enumerator.Current})");

                Workbook book = new Workbook();
                bool result = book.LoadExcel(enumerator.Current, out string msg);
                if(!result)
                {
                    Console.WriteLine(msg,Color.Red);
                    Console.WriteLine("----Load Error.",Color.Red);
                }else
                {
                    books.Add(book);
                    Console.WriteLine("----Finished");
                }
            }

            if(books.Count==0)
            {
                Console.WriteLine("----Workbok not found.",Color.Yellow);
                return;
            }

            bool verifyResult = true;
            if(option.IsVerify)
            {
                foreach(var book in books)
                {
                    Console.WriteLine($"----Begin verify the book{book.Name}");

                    bool result = book.Verify(out string msg);
                    if(!result)
                    {
                        Console.WriteLine(msg,Color.Red);

                        Console.WriteLine("----Verify Failed",Color.OrangeRed);

                        if (verifyResult)
                        {
                            verifyResult = !verifyResult;
                        }
                    }else
                    {
                        Console.WriteLine("----Verify success");
                    }
                }
            }

            if(verifyResult)
            {
                foreach(var book in books)
                {
                    Console.WriteLine($"----Begin export book({book.Name})");
                    foreach(var sheet in book.sheets)
                    {
                        Console.WriteLine($"--------Begin export Sheet({sheet.Name})");
                        if(option.Format == ETDFormat.Json)
                        {
                            JsonExporter.Export(option.OutputDir, sheet, option.Platform);
                        }else
                        {
                            if(option.IsOptimize)
                            {
                                LuaOptimizeExporter.Export(option.OutputDir, sheet, option.Platform);
                            }else
                            {
                                LuaExporter.Export(option.OutputDir, sheet, option.Platform);
                            }
                        }
                        Console.WriteLine("--------Export sheet Success");
                    }
                    Console.WriteLine($"----End export book");
                }
            }
        }
    }
}
