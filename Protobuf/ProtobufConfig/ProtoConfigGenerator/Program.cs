using CommandLine;
using Dot.Tool.Proto;
using Colorful;
using System.Drawing;

namespace ProtoConfigGenerator
{
    public enum ProtoExportType
    {
        Recognizer,
        Parser,
    }

    public class GeneratorOption
    {
        [Option('i',"input",Required =true,HelpText ="")]
        public string Input { get; set; }
        
        [Option('o', "output", Required = true, HelpText = "")]
        public string OutputDir { get; set; }
       
        [Option('w', "writer-type", Required = true, HelpText = "")]
        public ProtoWriterType WriterType { get; set; }
        
        [Option('e', "export-type", Required = true, HelpText = "")]
        public ProtoExportType ExportType { get; set; }

        [Option('p', "platform-type", Required = false, HelpText = "")]
        public ProtoPlatformType PlatformType { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<GeneratorOption>(args).WithParsed((option)=>
            {
                ProtoConfig config = ProtoConfigUtil.ReadConfig(option.Input);
                if(config==null)
                {
                    Console.WriteLine("ProtoConfig is null!", Color.Red);
                    return;
                }

                if(option.ExportType == ProtoExportType.Parser)
                {
                    if(option.PlatformType == ProtoPlatformType.Client || option.PlatformType == ProtoPlatformType.None)
                    {
                        ProtoConfigUtil.ParseMessage(config, option.OutputDir, option.WriterType, ProtoPlatformType.Client);
                    }
                    if (option.PlatformType == ProtoPlatformType.Server || option.PlatformType == ProtoPlatformType.None)
                    {
                        ProtoConfigUtil.ParseMessage(config, option.OutputDir, option.WriterType, ProtoPlatformType.Server);
                    }
                }
                else if(option.ExportType == ProtoExportType.Recognizer)
                {
                    ProtoConfigUtil.RecognizeMessage(config, option.OutputDir, option.WriterType);
                }
            });
            if (result.Tag == ParserResultType.NotParsed)
            {
                Console.WriteLine("The parameter in option is error");
            }
        }
    }
}
