using AsepriteDotNet.Aseprite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using System.Collections.Generic;

namespace Deltadust.Core
{
    public static class ResourceManager
    {
        private static ContentManager _content;
        private static GraphicsDevice _graphicsDevice;

        private static readonly Dictionary<string, (Texture2D texture, int refCount)> _textures = new();
        private static readonly Dictionary<string, (SpriteFont font, int refCount)> _fonts = new();
        private static readonly Dictionary<string, (SpriteSheet sprite, int refCount)> _sprites = new();

        private static readonly Dictionary<Texture2D, string> _textureLookup = [];
        private static readonly Dictionary<SpriteFont, string> _fontLookup = [];
        private static readonly Dictionary<SpriteSheet, string> _spriteLookup = [];

        public static void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
        }

        #region Texture2D
        public static Texture2D LoadTexture(string assetName)
        {
            if (!_textures.TryGetValue(assetName, out var textureData))
            {
                var texture = _content.Load<Texture2D>(assetName);
                _textures[assetName] = (texture, 1);  // First load, refCount = 1
                _textureLookup[texture] = assetName;  // Add reverse lookup
            }
            else
            {
                _textures[assetName] = (textureData.texture, textureData.refCount + 1);  // Increment refCount
            }
            return _textures[assetName].texture;
        }

        public static void UnloadTexture(string assetName)
        {
            if (_textures.TryGetValue(assetName, out var textureData))
            {
                if (textureData.refCount > 1)
                {
                    _textures[assetName] = (textureData.texture, textureData.refCount - 1);  // Decrement refCount
                }
                else
                {
                    _textures.Remove(assetName);  // RefCount is zero, so remove and unload
                    _textureLookup.Remove(textureData.texture);  // Remove reverse lookup
                    _content.UnloadAsset(assetName);
                }
            }
        }

        public static void UnloadTexture(Texture2D texture)
        {
            if (_textureLookup.TryGetValue(texture, out var assetName))
            {
                UnloadTexture(assetName);
            }
        }
        #endregion

        #region Font
        public static SpriteFont LoadFont(string assetName)
        {
            if (!_fonts.TryGetValue(assetName, out var fontData))
            {
                var font = _content.Load<SpriteFont>(assetName);
                _fonts[assetName] = (font, 1);  // First load, refCount = 1
                _fontLookup[font] = assetName;  // Add reverse lookup
            }
            else
            {
                _fonts[assetName] = (fontData.font, fontData.refCount + 1);  // Increment refCount
            }
            return _fonts[assetName].font;
        }

        public static void UnloadFont(string assetName)
        {
            if (_fonts.TryGetValue(assetName, out var fontData))
            {
                if (fontData.refCount > 1)
                {
                    _fonts[assetName] = (fontData.font, fontData.refCount - 1);  // Decrement refCount
                }
                else
                {
                    _fonts.Remove(assetName);  // RefCount is zero, so remove and unload
                    _fontLookup.Remove(fontData.font);  // Remove reverse lookup
                    _content.UnloadAsset(assetName);
                }
            }
        }

        public static void UnloadFont(SpriteFont font)
        {
            if (_fontLookup.TryGetValue(font, out var assetName))
            {
                UnloadFont(assetName);
            }
        }
        #endregion

        #region Sprite
        public static SpriteSheet LoadSprite(string assetName)
        {
            if (!_sprites.TryGetValue(assetName, out var spriteData))
            {
                var sprite = _content.Load<AsepriteFile>(assetName).CreateSpriteSheet(_graphicsDevice);
                _sprites[assetName] = (sprite, 1);  // First load, refCount = 1
                _spriteLookup[sprite] = assetName;  // Add reverse lookup
            }
            else
            {
                _sprites[assetName] = (spriteData.sprite, spriteData.refCount + 1);  // Increment refCount
            }
            return _sprites[assetName].sprite;
        }

        public static void UnloadSprite(string assetName)
        {
            if (_sprites.TryGetValue(assetName, out var spriteData))
            {
                if (spriteData.refCount > 1)
                {
                    _sprites[assetName] = (spriteData.sprite, spriteData.refCount - 1);  // Decrement refCount
                }
                else
                {
                    _sprites.Remove(assetName);  // RefCount is zero, so remove and unload
                    _spriteLookup.Remove(spriteData.sprite);  // Remove reverse lookup
                    _content.UnloadAsset(assetName);
                }
            }
        }

        public static void UnloadSprite(SpriteSheet sprite)
        {
            if (_spriteLookup.TryGetValue(sprite, out var assetName))
            {
                UnloadSprite(assetName);
            }
        }
        #endregion

        // Helper to unload assets when refCount is zero
        private static void UnloadAsset(string assetName)
        {
            _content.UnloadAsset(assetName);
        }

        public static void UnloadAll()
        {
            _textures.Clear();
            _fonts.Clear();
            _sprites.Clear();
            _textureLookup.Clear();
            _fontLookup.Clear();
            _spriteLookup.Clear();
            _content.Unload();
        }
    }
}
