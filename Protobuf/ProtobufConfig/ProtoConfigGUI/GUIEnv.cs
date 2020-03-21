using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Dot.Tool.ProtoGUI
{
    public static class GUIEnv
    {
        public static readonly string GUICONFIG_DEFAULT_PATH = "./gui-config.xml";

        private static GUIConfig guiConfig = null;

        public static GUIConfig GetGUIConfig()
        {
            return guiConfig;
        }

        public static void ReadGUIConfig(string path)
        {
            guiConfig = null;
            if(File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(GUIConfig));
                guiConfig = xmlSerializer.Deserialize(File.OpenRead(path)) as GUIConfig;
            }
            if(guiConfig == null)
            {
                guiConfig = new GUIConfig();
            }
        }

        public static void WriterGUIConfig(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GUIConfig));
            StreamWriter writer = new StreamWriter(path);
            xmlSerializer.Serialize(writer, guiConfig);
            writer.Flush();
            writer.Close();
        }
    }
}
