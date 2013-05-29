using System.Xml.Serialization;

namespace Selenium.Tools.xml
{
    public class UIMap
    {
        [XmlAttribute]
        public string Id
        { get; set; }

        [XmlElement]
        public string By
        { get; set; }

        [XmlElement]
        public string ToFind
        { get; set; }
    }
}
