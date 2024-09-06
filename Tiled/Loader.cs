using System.IO;
using System.Xml.Serialization;
<<<<<<< HEAD
=======
using Microsoft.Xna.Framework.Graphics;
>>>>>>> 9b0e8af1058dbd6e511eef64b9bdb6586ca463d9

namespace Deltadust.Tiled
{
    public class Loader
    {
<<<<<<< HEAD
        public static TiledMap LoadMap(string filePath)
        {
            XmlSerializer serializer = new(typeof(TiledMap));
            using FileStream fs = new(filePath, FileMode.Open);
            TiledMap map = (TiledMap)serializer.Deserialize(fs);
            map.ParseTileLayers();
            return map;
        }
    }
}
=======
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
>>>>>>> 9b0e8af1058dbd6e511eef64b9bdb6586ca463d9
