using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust.Tiled
{
    public class Loader
    {
        public static TileMap LoadMap(string filePath, GraphicsDevice graphicsDevice, string contentDirectory)
        {
            // Deserialize the TileMap from the TMX file
            XmlSerializer serializer = new XmlSerializer(typeof(TileMap));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                TileMap map = (TileMap)serializer.Deserialize(fs);
                
                // Load textures for each tileset
                foreach (var tileset in map.TileSets)
                {
                    if (tileset.Image != null && !string.IsNullOrEmpty(tileset.Image.Source))
                    {
                        string texturePath = Path.Combine(contentDirectory, tileset.Image.Source);
                        tileset.LoadTexture(graphicsDevice, texturePath);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"No image source for tileset with firstgid {tileset.FirstGid}");
                    }
                }

                return map;
            }
        }
    }
}
