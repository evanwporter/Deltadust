using Deltadust.Core;
using Deltadust.Events;
using Deltadust.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Aseprite;

namespace Deltadust.Entities {
    public class Slime(Vector2 startPosition, WorldEngine world, ResourceManager resourceManager, CentralEventHandler eventHandler) 
        : NPC(startPosition, world, resourceManager, eventHandler) {
        protected new readonly float _speed = 50f;
        private AnimatedSprite _moveAnimation;
        private AnimatedSprite _idleAnimation;
        private AnimatedSprite _currentAnimation;

        private Vector2 _movementDirection;
        private float _timeSinceLastDirectionChange;
        private readonly float _directionChangeInterval = 2.0f; // Change direction every 2 seconds
        public string Name = "Monsters/slime";
        public int ID_Name = 1;

        public override void LoadContent() {
            SpriteSheet spriteSheet = _resourceManager.LoadSprite(Name);

            // Load animations
            _moveAnimation = spriteSheet.CreateAnimatedSprite("Jump");
            _idleAnimation = spriteSheet.CreateAnimatedSprite("Jump");

            _currentAnimation = _idleAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            _timeSinceLastDirectionChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeSinceLastDirectionChange >= _directionChangeInterval)
            {
                _movementDirection = GetRandomDirection();
                _timeSinceLastDirectionChange = 0f;
            }

            Vector2 movement = _movementDirection * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 newPosition = _position + movement;
            Rectangle slimeHitbox = GetHitbox(newPosition);
            if (!_world.IsCollidingWithTile(slimeHitbox))
            {
                _position = newPosition;
            }

            _currentAnimation = movement != Vector2.Zero ? _moveAnimation : _idleAnimation;
            _currentAnimation.Play();
            _currentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix)
        {
            spriteBatch.Draw(_currentAnimation, _position);

            #if DEBUG
            DrawHitbox(spriteBatch);
            #endif
        }

        // private Vector2 GetRandomDirection()
        // {
        //     // Generate a random direction
        //     int direction = _random.Next(4);
        //     return direction switch
        //     {
        //         0 => new Vector2(1, 0), // Move right
        //         1 => new Vector2(-1, 0), // Move left
        //         2 => new Vector2(0, 1), // Move down
        //         _ => new Vector2(0, -1), // Move up
        //     };
        // }

        public override Rectangle GetHitbox(Vector2 position)
        {
            return new Rectangle(
                (int)(position.X + ((32 - hitboxWidth) / 2)), 
                (int)(position.Y + 32 - hitboxWidth),
                hitboxWidth,
                hitboxWidth
            );
        }
    }
}
