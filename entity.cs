using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AsepriteDotNet.Aseprite;
using MonoGame.Aseprite;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace MyGame {
    public abstract class Entity
    {
        // // public Vector2 Position { get; set; }
        // public float Speed { get; set; }

        // protected Entity(Vector2 startPosition, float speed)
        // {
        //     Position = startPosition;
        //     Speed = speed;
        // }

        public abstract void LoadContent(AsepriteFile aseFile, GraphicsDevice graphicsDevice);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix);
    }
}
