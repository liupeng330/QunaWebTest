using System.Xml.Serialization;

namespace Selenium.Tools.xml
{
    [XmlRoot]
    public class Map
    {
        [XmlArray]
        public UIMap[] UIMaps
        { get; set; }
    }
}
