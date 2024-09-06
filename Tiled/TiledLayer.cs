using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace Deltadust.Tiled
{
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

        public int[,] Tiles { get; set; }

        public void ParseCsvData()
        {
            // Parse CSV data into a 2D array
            Tiles = new int[Width, Height];
            string[] rows = Data.Value.Split(',');
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tiles[x, y] = int.Parse(rows[x + y * Width]);
                }
            }
        }

        // Draw method for the layer
        public void Draw(SpriteBatch spriteBatch, TiledTileSet tileSet, Vector2 position)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int tileId = Tiles[x, y];
                    if (tileId > 0)  // Non-zero tile IDs represent a tile
                    {
                        Texture2D tileTexture = tileSet.GetTileTexture(tileId);
                        if (tileTexture != null)
                        {
                            spriteBatch.Draw(tileTexture, new Vector2(x * tileSet.TileWidth, y * tileSet.TileHeight) + position, Color.White);
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