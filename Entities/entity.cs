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
        protected WorldEngine _map;
        protected ResourceManager _resourceManager;
        private readonly CentralEventHandler _eventHandler;
        protected readonly int hitboxWidth = 20;

        public Entity(Vector2 startPosition, WorldEngine map, ResourceManager resourceManager, CentralEventHandler eventHandler)
        {
            _position = startPosition;
            _map = map;
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

        protected void DrawHitbox(SpriteBatch spriteBatch) {
            Texture2D hitboxTexture = new(spriteBatch.GraphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.Red });

            spriteBatch.Draw(hitboxTexture, GetHitbox(_position), Color.Red * 0.5f);
        }

        public virtual Rectangle GetHitbox(Vector2 position)
        {
            return new Rectangle(
                (int)(position.X + ((32 - hitboxWidth) / 2)), // Centers it on X axis
                (int)(position.Y + 32 + 32 - 20), // Move down two tiles then up 20 pixels
                hitboxWidth,
                20
            );
        }

        public Vector2 Position => _position;

    }
}
