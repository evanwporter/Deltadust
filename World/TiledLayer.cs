using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Deltadust.Entities.Static {

    public class TiledLayer : IDrawable {
        private readonly TiledMapTileLayer _layer;
        private readonly TiledMapRenderer _renderer;
        private readonly float _y;

        public float Y => _y;


        public TiledLayer(TiledMapTileLayer layer, float y, TiledMapRenderer renderer) {
            _layer = layer;
            _y = 192f;
            _renderer = renderer;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix viewMatrix) {
            _renderer.Draw(_layer, viewMatrix);
        }

    }
}
