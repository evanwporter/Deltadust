using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Texture2D Texture { get; set; }

        public void LoadTexture(GraphicsDevice graphicsDevice, string texturePath)
        {
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