using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust.Tiled
{
    [XmlRoot("map")]
    public class TileMap
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
        public List<TileLayer> TileLayerList { get; set; }
        [XmlElement("objectgroup")]
        public List<ObjectLayer> ObjectLayerList { get; set; }

        [XmlIgnore]
        public Dictionary<string, TileLayer> TileLayers { get; private set; }
        [XmlIgnore]
        public Dictionary<string, ObjectLayer> ObjectLayers { get; private set; }

        public void Load()
        {
            TileLayers = new Dictionary<string, TileLayer>();

            if (TileLayerList != null)
            {
                foreach (var layer in TileLayerList)
                {
                    if (!string.IsNullOrEmpty(layer.Name))
                    {
                        TileLayers[layer.Name] = layer;
                        System.Diagnostics.Debug.WriteLine($"Loaded tile layer: {layer.Name}");
                        layer.Load();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Warning: Tile layer with no name found.");
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error: No layers found in the TMX file.");
            }
        }

        public TileLayer GetTileLayer(string name)
        {
            if (TileLayers == null || !TileLayers.ContainsKey(name))
            {
                System.Diagnostics.Debug.WriteLine($"Error: Tile layer '{name}' not found.");
                return null; // Return null if the layer is missing
            }
            return TileLayers[name];
        }


        public ObjectLayer GetObjectLayer(string name)
        {
            ObjectLayers.TryGetValue(name, out var layer);
            return layer;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            foreach (var layer in TileLayers.Values)
            {
                foreach (var tileSet in TileSets)
                {
                    layer.Draw(spriteBatch, tileSet, viewMatrix);
                }
            }
        }

    }
}