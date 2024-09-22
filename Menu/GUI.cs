using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Deltadust.Menu
{
    public class GUI
    {
        private TiledMap _guiTiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private GraphicsDevice _graphicsDevice;

        private int _borderSize;
        private Rectangle _guiArea;

        public GUI(GraphicsDevice graphicsDevice, ContentManager content, string mapPath, int borderSize)
        {
            _graphicsDevice = graphicsDevice;
            _guiTiledMap = content.Load<TiledMap>(mapPath);
            _tiledMapRenderer = new TiledMapRenderer(_graphicsDevice, _guiTiledMap);
            _borderSize = borderSize;

            CalculateGuiArea(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        private void CalculateGuiArea(int screenWidth, int screenHeight)
        {
            _guiArea = new Rectangle(
                _borderSize, // X: left border
                _borderSize, // Y: top border
                screenWidth - 2 * _borderSize, // Width: screen width minus left and right border
                screenHeight - 2 * _borderSize // Height: screen height minus top and bottom border
            );
        }

        public void OnResize(int newWidth, int newHeight)
        {
            CalculateGuiArea(newWidth, newHeight);
        }

        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Matrix viewMatrix = Matrix.CreateScale(
                _guiArea.Width / (float)_guiTiledMap.WidthInPixels,
                _guiArea.Height / (float)_guiTiledMap.HeightInPixels, 1.0f
            );

            _tiledMapRenderer.Draw(viewMatrix);
        }
    }
}