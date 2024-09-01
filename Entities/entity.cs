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
        protected Vector2 _position;
        protected float _speed = 50f;
        protected AnimatedSprite CurrentAnimation;
        protected World _world;
        protected ResourceManager _resourceManager;

        public Entity(Vector2 startPosition, World world, ResourceManager resourceManager)
        {
            _position = startPosition;
            _world = world;
            _resourceManager = resourceManager;
        }

        public abstract void LoadContent();

        public virtual void Update(GameTime gameTime)
        {
            CurrentAnimation.Play();
            CurrentAnimation.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix)
        {
            spriteBatch.Draw(CurrentAnimation, _position);
        }

        public Vector2 Position => _position;

    }
}
