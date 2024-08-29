using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MyGame {
    [XmlRoot("Inventory")]
    public class Inventory
    {
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<string> Items { get; set; }

        public Inventory()
        {
            Items = new List<string>();
        }

        public void AddItem(string item)
        {
            Items.Add(item);
        }

        public void RemoveItem(string item)
        {
            Items.Remove(item);
        }

        public void SaveToFile(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Inventory));
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }

        public static Inventory LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Inventory));
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    return (Inventory)serializer.Deserialize(stream);
                }
            }
            else
            {
                return new Inventory(); // Return an empty inventory if the file doesn't exist
            }
        }
    }
}