using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust {
    [XmlRoot("Inventory")]
    public class Inventory
    {
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        private List<string> Items { get; set; }

        public Inventory()
        {
            Items = [];
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
            XmlSerializer serializer = new(typeof(Inventory));
            using FileStream stream = new(filePath, FileMode.Create);
            serializer.Serialize(stream, this);
        }

        public static Inventory LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new(typeof(Inventory));
                using (FileStream stream = new(filePath, FileMode.Open))
                {
                    return (Inventory)serializer.Deserialize(stream);
                }
            }
            else
            {
                return new Inventory(); // Return an empty inventory if the file doesn't exist
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix)
        {
            // Draw the inventory relative to the camera position (top-left corner of the screen)
            Vector2 position = Vector2.Transform(new Vector2(10, 10), Matrix.Invert(viewMatrix));

            spriteBatch.DrawString(font, "Inventory:", position, Color.White);
            position.Y += 30;

            foreach (string item in Items)
            {
                spriteBatch.DrawString(font, item, position, Color.White);
                position.Y += 30;
            }
        }
    }
}