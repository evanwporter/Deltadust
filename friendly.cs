using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AsepriteDotNet.Aseprite;
using MonoGame.Aseprite;

namespace MyGame {
    public class Friendly : NPC {
        private AnimatedSprite _idleAnimation;
        private AnimatedSprite _currentAnimation;
        private string _dialogue;
        private bool _isInteracting;

        public Friendly(Vector2 startPosition, string dialogue, World world, ResourceManager resourceManager)
            : base(startPosition, world, resourceManager)
        {
            _dialogue = dialogue;
        }

        public override void LoadContent() {
            SpriteSheet spriteSheet = _resourceManager.LoadSprite("Character/Lewis_Beach");
            _idleAnimation = spriteSheet.CreateAnimatedSprite("Stand Forward");
            _currentAnimation = _idleAnimation;
        }

        public override void Update(GameTime gameTime) {
            _currentAnimation.Play();
            _currentAnimation.Update(gameTime);
        }

        public void Interact(Player player) {
            Rectangle playerHitbox = player.GetHitbox(player.Position);
            Rectangle npcHitbox = GetHitbox(_position);

            if (npcHitbox.Intersects(playerHitbox)) {
                _isInteracting = true;
            } else {
                _isInteracting = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix) {
            spriteBatch.Draw(_currentAnimation, _position);

            if (_isInteracting) {
                Vector2 dialoguePosition = _position + new Vector2(0, -50); // Position the dialogue above the NPC
                spriteBatch.DrawString(font, _dialogue, dialoguePosition, Color.White);
            }
        }

        private Rectangle GetHitbox(Vector2 position) {
            return new Rectangle(
                (int)position.X,
                (int)position.Y,
                32, // Width of the hitbox
                32  // Height of the hitbox
            );
        }
    }
}
