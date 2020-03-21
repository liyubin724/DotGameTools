using Dot.Tool.Proto.Writer;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Dot.Tool.Proto
{
    public enum ProtoWriterType
    {
        CSharp,
        Lua,
        CPlusPlus,
    }

    public enum ProtoPlatformType
    {
        None = 0,
        Client,
        Server,
    }

    public static class ProtoConfigUtil
    {
        public static ProtoConfig ReadConfig(string path, bool createIfNot = false)
        {
            ProtoConfig config = null;
            if(File.Exists(path))
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProtoConfig));
                    config = xmlSerializer.Deserialize(File.OpenRead(path)) as ProtoConfig;
                }catch
                {

                }
            }

            if(config == null && createIfNot)
            {
                config = new ProtoConfig();
            }
            return config;
        }

        public static bool WriterConfig(string path,ProtoConfig config)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProtoConfig));
                StreamWriter writer = new StreamWriter(path, false,Encoding.UTF8);
                xmlSerializer.Serialize(writer, config);
                return true;
            }catch
            {
                
            }
            return false;
        }

        public static void RecognizeMessage(ProtoConfig config,string rootDir, ProtoWriterType writerType)
        {
            if(writerType == ProtoWriterType.CSharp)
            {
                CSharpMessageRecognizer.CreateRecognizerScript(rootDir, config);
            }
        }

        public static void ParseMessage(ProtoConfig config,string rootDir,ProtoWriterType writerType, ProtoPlatformType platformType)
        {
            if(writerType == ProtoWriterType.CSharp)
            {
                if(platformType == ProtoPlatformType.Server)
                {
                    CSharpServerMessageParser.CreateParserScript(rootDir, config);
                }
                else
                {
                    CSharpClientMessageParser.CreateParserScript(rootDir, config);
                }
            }
        }
    }
}
