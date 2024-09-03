using Deltadust.Core;
using Deltadust.World;
using Deltadust.Events;
using Microsoft.Xna.Framework;

using MonoGame.Aseprite;
using System;

namespace Deltadust.Entities {
    public delegate void DeathEventHandler(object sender, EventArgs e);

    public class NPC : Entity {

        public event DeathEventHandler OnKilled;

        private string _name = "Lewis_Beach";
        
        private readonly int hitboxWidth = 20;

        public NPC(Vector2 startPosition, WorldEngine world, ResourceManager resourceManager, CentralEventHandler eventHandler)
            : base(startPosition, world, resourceManager, eventHandler)
        {
        }

        public override void LoadContent() {
            SpriteSheet spriteSheet = _resourceManager.LoadSprite(_name);
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

        // public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix) {
        //     spriteBatch.Draw(_currentAnimation, _position);

        //     if (_isInteracting) {
        //         Vector2 dialoguePosition = _position + new Vector2(0, -50); // Position the dialogue above the NPC
        //         spriteBatch.DrawString(font, _dialogue, dialoguePosition, Color.White);
        //     }
        // }

        protected Vector2 GetRandomDirection()
        {
            // Generate a random direction (e.g., for NPC or monsters)
            Random random = new Random();
            int direction = random.Next(4);
            return direction switch
            {
                0 => new Vector2(1, 0), // Move right
                1 => new Vector2(-1, 0), // Move left
                2 => new Vector2(0, 1), // Move down
                _ => new Vector2(0, -1), // Move up
            };
        }

        public void Kill() {
            OnKilled?.Invoke(this, EventArgs.Empty);
        }

        public Rectangle GetHitbox(Vector2 position)
        {
            return new Rectangle(
                (int)(position.X + ((32 - hitboxWidth) / 2)),
                (int)(position.Y + 32 + 32 - 20),
                hitboxWidth,
                20
            );
        }
    }
}
