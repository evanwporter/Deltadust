using System.Xml.Serialization;
using System.Collections.Generic;

namespace Deltadust.Tiled
{

    public class TiledObject
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("gid")]
        public int Gid { get; set; }

        [XmlAttribute("x")]
        public float X { get; set; }

        [XmlAttribute("y")]
        public float Y { get; set; }

        [XmlAttribute("width")]
        public float Width { get; set; }

        [XmlAttribute("height")]
        public float Height { get; set; }

        [XmlElement("properties")]
        public Properties Properties { get; set; }
    }

    [XmlRoot("objectgroup")]
    public class ObjectLayer
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("object")]
        public List<TiledObject> Objects { get; set; } = new List<TiledObject>();

        public TiledObject GetObjectById(int id)
        {
            return Objects.Find(obj => obj.Id == id);
        }
    }


}