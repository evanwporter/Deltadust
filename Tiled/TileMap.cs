using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust.Tiled
{
    [XmlRoot("map")]
    public class TiledMap
    {
        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }

        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }

        [XmlElement("tileset")]
        public List<TiledTileSet> TileSets { get; set; }

        [XmlElement("layer")]
        public List<TileLayer> Layers { get; set; }

        public void ParseTileLayers()
        {
            foreach (var layer in Layers)
            {
                layer.ParseCsvData();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            foreach (var layer in Layers)
            {
                foreach (var tileSet in TileSets)
                {
                    layer.Draw(spriteBatch, tileSet, position);
                }
            }
        }
    }
}