using Deltadust.Core;
using Deltadust.Events;
using Deltadust.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Aseprite;

namespace Deltadust.Entities {
    public abstract class Entity
    {
        protected Vector2 _position;
        protected readonly float _speed;
        protected AnimatedSprite CurrentAnimation;
        protected WorldEngine _world;
        protected ResourceManager _resourceManager;
        private readonly CentralEventHandler _eventHandler;

        public Entity(Vector2 startPosition, WorldEngine world, ResourceManager resourceManager, CentralEventHandler eventHandler)
        {
            _position = startPosition;
            _world = world;
            _resourceManager = resourceManager;
            _eventHandler = eventHandler;
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
