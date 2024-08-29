using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public Slime(Vector2 startPosition, float speed, World world) {
            _position = startPosition;
            _speed = speed;
            _world = world;
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
            Vector2 movement = Vector2.Zero;

            // Basic AI: Move randomly or towards the player
            // For simplicity, let's make the slime move randomly for now
            movement.X = (float)(gameTime.TotalGameTime.TotalSeconds % 2 == 0 ? _speed : -_speed) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 newPosition = _position + movement;
            Rectangle slimeHitbox = GetHitbox(newPosition);
            if (!_world.IsCollidingWithTile(slimeHitbox))
            {
                _position = newPosition;
            }

            _currentAnimation = movement != Vector2.Zero ? _moveAnimation : _idleAnimation;
            // _currentAnimation = _idleAnimation;
            _currentAnimation.Play();
            _currentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix)
        {
            spriteBatch.Draw(_currentAnimation, _position);
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
