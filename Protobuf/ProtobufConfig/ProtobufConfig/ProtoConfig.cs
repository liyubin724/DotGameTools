using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Dot.Tool.Proto
{
    [Serializable]
    [XmlRoot("protos")]
    public class ProtoConfig
    {
        [XmlAttribute("namespace")]
        public string SpaceName { get; set; }

        [XmlElement("c2s")]
        public ProtoGroup C2SGroup { get; set; }

        [XmlElement("s2c")]
        public ProtoGroup S2CGroup { get; set; }
    }

    [Serializable]
    public class ProtoGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("message_group")]
        public List<ProtoMessageGroup> MessageGroups { get; set; }
    }

    [Serializable]
    public class ProtoMessageGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("message")]
        public List<ProtoMessage> Messages { get; set; }
    }

    [Serializable]
    public class ProtoMessage
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlAttribute("comment")]
        public string Comment { get; set; }

        [XmlAttribute("class")]
        public string ClassName { get; set; }

        [XmlAttribute("enable")]
        public bool Enable { get; set; }
    }
}
