using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Dot.Tool.ProtoGUI
{
    [Serializable]
    [XmlRoot("config")]
    public class GUIConfig
    {
        [XmlElement("last_config_path")]
        public string LastConfigPath;
    }
}
