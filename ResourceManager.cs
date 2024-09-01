using System.Collections.Generic;
using AsepriteDotNet.Aseprite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Aseprite;


namespace MyGame {
    public class ResourceManager(ContentManager content, GraphicsDevice graphicsDevice) {
        private readonly ContentManager _content = content;
        private readonly GraphicsDevice _graphicsDevice = graphicsDevice;
        private readonly Dictionary<string, Texture2D> _textures = [];
        private readonly Dictionary<string, SpriteFont> _fonts = [];
        private readonly Dictionary<string, SpriteSheet> _sprites = [];

        #region Texture2D
        public Texture2D LoadTexture(string assetName) {
            if (!_textures.TryGetValue(assetName, out Texture2D value)) {
                value = _content.Load<Texture2D>(assetName);
                _textures[assetName] = value;
            }
            return value;
        }

        public void UnloadTexture(string assetName) {
            _textures.Remove(assetName);
            UnloadAsset(assetName);
        }
        #endregion

        #region Font
        public SpriteFont LoadFont(string assetName) {
            if (!_fonts.TryGetValue(assetName, out SpriteFont value)) {
                value = _content.Load<SpriteFont>(assetName);
                _fonts[assetName] = value;
            }
            return value;
        }

        public void UnloadFont(string assetName) {
            _fonts.Remove(assetName);
            UnloadAsset(assetName);
        }
        #endregion

        #region Sprite
        public SpriteSheet LoadSprite(string assetName) {
            if (!_sprites.TryGetValue(assetName, out SpriteSheet value)) {
                value = _content.Load<AsepriteFile>(assetName).CreateSpriteSheet(_graphicsDevice);
                _sprites[assetName] = value;
            }
            return value;
        }

        public void UnloadSprite(string assetName) {
            _sprites.Remove(assetName);
            UnloadAsset(assetName);
        }
        #endregion

        public T LoadAsset<T>(string assetName) {
            return _content.Load<T>(assetName);
        }

        private void UnloadAsset(string assetName) {
            _content.UnloadAsset(assetName);
        }

        public void UnloadAll() {
            _textures.Clear();
            _fonts.Clear();
            _sprites.Clear();
            _content.Unload();
        }
    }
}
