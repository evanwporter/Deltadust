using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Deltadust.Tiled
{
    public class TiledObject
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

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

        // Use a list of key-value pairs for serialization
        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<KeyValuePair> PropertyList { get; set; }

        // Use a dictionary internally for convenience
        [XmlIgnore]
        public Dictionary<string, string> Properties { get; set; }

        // Position and size
        [XmlIgnore]
        public Vector2 Position => new Vector2(X, Y);

        [XmlIgnore]
        public Vector2 Size => new Vector2(Width, Height);

        public TiledObject()
        {
            PropertyList = new List<KeyValuePair>();
            Properties = new Dictionary<string, string>();
        }

        public void Load()
        {
            // Convert PropertyList to the internal Properties dictionary
            foreach (var prop in PropertyList)
            {
                Properties[prop.Key] = prop.Value;
            }
        }

        public void SaveProperties()
        {
            // Convert the internal Properties dictionary back to the PropertyList
            PropertyList.Clear();
            foreach (var kvp in Properties)
            {
                PropertyList.Add(new KeyValuePair { Key = kvp.Key, Value = kvp.Value });
            }
        }

        public string GetProperty(string key)
        {
            Properties.TryGetValue(key, out string value);
            return value;
        }
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