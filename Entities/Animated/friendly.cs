using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Aseprite;
using Deltadust.Core;
using System;

namespace Deltadust.Entities.Animated {
    public class Friendly : NPC {
        private AnimatedSprite _idleAnimation;
        private string _dialogue;

        public Friendly(Vector2 startPosition, string dialogue)
            : base(startPosition)
        {
            _dialogue = dialogue;
        }

        public override void LoadContent() {
            SpriteSheet spriteSheet = ResourceManager.LoadSprite("Character/Lewis_Beach");
            _idleAnimation = spriteSheet.CreateAnimatedSprite("Stand Forward");
            _currentAnimation = _idleAnimation;
        }

        public override void UnloadContent() {
            ResourceManager.UnloadSprite("Character/Lewis_Beach");
        }

        public override void Update(GameTime gameTime) {
            _currentAnimation.Play();
            _currentAnimation.Update(gameTime);
        }

        // public void Interact(Player player) {
        //     Rectangle playerHitbox = player.GetHitbox(player.Position);
        //     Rectangle npcHitbox = GetHitbox(_position);

        //     if (npcHitbox.Intersects(playerHitbox)) {
        //         _isInteracting = true;
        //     } else {
        //         _isInteracting = false;
        //     }
        // }

        public override void Draw(SpriteBatch spriteBatch, Matrix viewMatrix) {
            spriteBatch.Draw(_currentAnimation, _position);

            // if (_isInteracting) {
            //     Vector2 dialoguePosition = _position + new Vector2(0, -50); // Position the dialogue above the NPC
            //     spriteBatch.DrawString(_font, _dialogue, dialoguePosition, Color.White);
            // }

            #if DEBUG
            DrawHitbox(spriteBatch);
            #endif
        }

        public override void Interact() {
            Vector2 directionToPlayer = Game1.GetWorld().GetPlayer().Position - _position;

            // if (Math.Abs(directionToPlayer.X) > Math.Abs(directionToPlayer.Y)) {
            //     if (directionToPlayer.X > 0) {
            //         _currentAnimation = _idleRightAnimation; // Face right
            //     } else {
            //         _currentAnimation = _idleLeftAnimation; // Face left (flip horizontally)
            //     }
            // } else {
            //     if (directionToPlayer.Y > 0) {
            //         _currentAnimation = _idleDownAnimation; // Face down
            //     } else {
            //         _currentAnimation = _idleUpAnimation; // Face up
            //     }
            // }
            
            Game1.GetEventManager().TriggerDialogue("dialogue.json");
        }

        public new Rectangle GetHitbox(Vector2 position) {
            return new Rectangle(
                (int)position.X,
                (int)position.Y,
                32, // Width of the hitbox
                32  // Height of the hitbox
            );
        }
    }
}
