using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Deltadust.Tiled
{
    public class KeyValuePair
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    [XmlRoot("layer")]
    public class TileLayer
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("data")]
        public CsvData Data { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlAttribute("opacity")]
        public float Opacity { get; set; } = 1.0f;  // Default to fully visible
        public bool IsVisible => Opacity > 0.0f;

        public int[] Tiles { get; set; }

        // Replace Dictionary with a serializable List of key-value pairs
        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<KeyValuePair> PropertyList { get; set; }

        // Dictionary to hold properties internally
        [XmlIgnore]
        public Dictionary<string, string> Properties { get; set; }

        public TileLayer()
        {
            PropertyList = new List<KeyValuePair>();
            Properties = new Dictionary<string, string>();
        }

        public void Load()
        {
            // Convert the PropertyList to the internal Properties dictionary
            foreach (var prop in PropertyList)
            {
                Properties[prop.Key] = prop.Value;
            }

            Tiles = new int[Width * Height];
            string[] rows = Data.Value.Split(',');
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tiles[x + y * Width] = int.Parse(rows[x + y * Width]); // 1D array logic
                }
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

        // Helper to get a tile value from a 2D position
        public int GetTileValue(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Tiles[x + y * Width]; // Convert 2D to 1D index
            }
            return 0;
        }

        public Tile GetTile(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                int tileId = Tiles[x + y * Width]; // Use the 1D array
                return new Tile(tileId, x, y);
            }
            return null;
        }

        public void Draw(SpriteBatch spriteBatch, TiledTileSet tileSet, Matrix viewMatrix)
        {
            if (!IsVisible)
            {
                return;
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int tileId = GetTileValue(x, y);
                    if (tileId > 0) // Non-zero tile IDs represent a tile
                    {
                        Texture2D tileTexture = tileSet.GetTileTexture(tileId);
                        if (tileTexture != null)
                        {
                            Vector2 tilePosition = new Vector2(x * tileSet.TileWidth, y * tileSet.TileHeight);
                            spriteBatch.Draw(tileTexture, tilePosition, Color.White * Opacity);
                        }
                    }
                }
            }
        }
    }

    public class CsvData
    {
        [XmlText]
        public string Value { get; set; }
    }
}
