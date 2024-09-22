using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust.ItemManagement {
    [XmlRoot("Inventory")]
    public class Inventory
    {
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        private List<Item> Items { get; set; }

        private SpriteFont _font;

        public Inventory()
        {
            Items = [];
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void RemoveItem(Item item)
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
                using FileStream stream = new(filePath, FileMode.Open);
                return (Inventory)serializer.Deserialize(stream);
            }
            else
            {
                return new Inventory();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            // Draw the inventory relative to the camera position (top-left corner of the screen)
            Vector2 position = Vector2.Transform(new Vector2(10, 10), Matrix.Invert(viewMatrix));

            spriteBatch.DrawString(_font, "Inventory:", position, Color.White);
            position.Y += 30;

            foreach (Item item in Items) {
                spriteBatch.DrawString(_font, item.ToString(), position, Color.White);
                position.Y += 30;
            }
        }

        internal void SetFont(SpriteFont spriteFont)
        {
            _font = spriteFont;
        }

    }
}