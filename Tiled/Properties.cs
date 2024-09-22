using System.Collections.Generic;
using System.Xml.Serialization;

namespace Deltadust.Tiled
{

    [XmlRoot("properties")]
    public class Properties
    {
        [XmlElement("property")]
        public List<Property> PropertyList { get; set; } = [];
        
        public string GetPropertyValue(string name)
        {
            foreach (var prop in PropertyList)
            {
                if (prop.Name == name)
                    return prop.Value;
            }
            return null;
        }
    }

    public class Property
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public float ToFloat() {
            return float.Parse(Value);
        }
    }


}