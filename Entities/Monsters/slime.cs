using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AsepriteDotNet.Aseprite;
using MonoGame.Aseprite;

namespace MyGame {
    public class Slime : Entity {
        private Vector2 _position;
        private readonly float _speed;
        private AnimatedSprite _moveAnimation;
        private AnimatedSprite _idleAnimation;
        private AnimatedSprite _currentAnimation;

        private readonly World _world;
        private Vector2 _movementDirection;
        private float _timeSinceLastDirectionChange;
        private readonly float _directionChangeInterval = 2.0f; // Change direction every 2 seconds
        private readonly Random _random;

        public Slime(Vector2 startPosition, float speed, World world) {
            _position = startPosition;
            _speed = speed;
            _world = world;
            _random = new Random();
            _movementDirection = GetRandomDirection();
        }

        public override void LoadContent(AsepriteFile aseFile, GraphicsDevice graphicsDevice) {
            SpriteSheet spriteSheet = aseFile.CreateSpriteSheet(graphicsDevice);

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
        }

        private Vector2 GetRandomDirection()
        {
            // Generate a random direction
            int direction = _random.Next(4);
            return direction switch
            {
                0 => new Vector2(1, 0), // Move right
                1 => new Vector2(-1, 0), // Move left
                2 => new Vector2(0, 1), // Move down
                _ => new Vector2(0, -1), // Move up
            };
        }

        private Rectangle GetHitbox(Vector2 position)
        {
            return new Rectangle(
                (int)(position.X + 10), // Adjust the hitbox position and size as needed
                (int)(position.Y + 10),
                20, // Width of the hitbox
                20  // Height of the hitbox
            );
        }

        public Vector2 Position => _position;
    }
}
