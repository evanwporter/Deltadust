using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
<<<<<<< HEAD
=======
using System.IO;
>>>>>>> 9b0e8af1058dbd6e511eef64b9bdb6586ca463d9
using System.Xml.Serialization;

namespace Deltadust.Tiled
{

    [XmlRoot("tileset")]
    public class TiledTileSet
    {
        [XmlAttribute("firstgid")]
        public int FirstGid { get; set; }

        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }

        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }

        [XmlElement("image")]
        public TiledImage Image { get; set; }

<<<<<<< HEAD
=======
        [XmlIgnore]
>>>>>>> 9b0e8af1058dbd6e511eef64b9bdb6586ca463d9
        public Texture2D Texture { get; set; }

        public void LoadTexture(GraphicsDevice graphicsDevice, string texturePath)
        {
<<<<<<< HEAD
            // Load the tileset image into a Texture2D
            using (var stream = TitleContainer.OpenStream(texturePath))
            {
                Texture = Texture2D.FromStream(graphicsDevice, stream);
            }
        }

        // Get the correct tile by its ID
        public Texture2D GetTileTexture(int tileId)
        {
            if (tileId < FirstGid) return null;

            int tileIndex = tileId - FirstGid;
            int tilesPerRow = Texture.Width / TileWidth;

            int tileX = (tileIndex % tilesPerRow) * TileWidth;
            int tileY = (tileIndex / tilesPerRow) * TileHeight;

            return GetSubTexture(Texture, new Rectangle(tileX, tileY, TileWidth, TileHeight));
        }

=======
            if (File.Exists(texturePath))
            {
                // Load the tileset image into a Texture2D
                using (var stream = TitleContainer.OpenStream(texturePath))
                {
                    Texture = Texture2D.FromStream(graphicsDevice, stream);
                    System.Diagnostics.Debug.WriteLine($"Successfully loaded texture: {texturePath}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Error: Texture file not found: {texturePath}");
            }
        }

        public Texture2D GetTileTexture(int tileId)
        {
            // Check if the tileId is valid for this tileset
            if (tileId < FirstGid || tileId >= FirstGid + GetTotalTiles())
            {
                System.Diagnostics.Debug.WriteLine($"Texture for tileset not loaded. Tile ID: {tileId}");
                return null;
            }

            if (tileId < FirstGid || tileId >= FirstGid + GetTotalTiles())
            {
                System.Diagnostics.Debug.WriteLine($"Tile ID {tileId} is out of range for this tileset.");
                return null;
            }

            // Calculate the index of the tile relative to this tileset
            int localTileId = tileId - FirstGid;
            int tilesPerRow = Texture.Width / TileWidth;

            // Calculate the X and Y coordinates of the tile in the tileset
            int tileX = (localTileId % tilesPerRow) * TileWidth;
            int tileY = (localTileId / tilesPerRow) * TileHeight;

            // Get the subtexture for the tile
            return GetSubTexture(Texture, new Rectangle(tileX, tileY, TileWidth, TileHeight));
        }


        public int GetTotalTiles()
        {
            // Check if the Texture is loaded
            if (Texture == null)
            {
                return 0; // Return 0 or handle this error appropriately
            }

            int tilesPerRow = Texture.Width / TileWidth;
            int tilesPerColumn = Texture.Height / TileHeight;
            return tilesPerRow * tilesPerColumn;
        }

>>>>>>> 9b0e8af1058dbd6e511eef64b9bdb6586ca463d9
        private Texture2D GetSubTexture(Texture2D sourceTexture, Rectangle sourceRectangle)
        {
            Texture2D subTexture = new Texture2D(sourceTexture.GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
            Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
            sourceTexture.GetData(0, sourceRectangle, data, 0, data.Length);
            subTexture.SetData(data);
            return subTexture;
        }
    }

    public class TiledImage
    {
        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }
    }

}