using Deltadust.Core;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;

namespace Deltadust.Entities.Animated.Components
{
    public class MovementComponent : Component
    {
        public AnimatedSprite moveForwardCycle;
        public AnimatedSprite moveBackwardCycle;
        public AnimatedSprite moveRightCycle;
        private readonly float _speed;

        public MovementComponent(AnimatedEntity owner, float speed) : base(owner)
        {
            _speed = speed;
        }

        public void LoadContent(string spriteSheetName)
        {
            SpriteSheet spriteSheet = ResourceManager.LoadSprite(spriteSheetName);
            moveForwardCycle = spriteSheet.CreateAnimatedSprite("Walk Down");
            moveBackwardCycle = spriteSheet.CreateAnimatedSprite("Walk Up");
            moveRightCycle = spriteSheet.CreateAnimatedSprite("Walk Right");
        }

        public void Move(Vector2 direction, GameTime gameTime) {
            Vector2 movement = direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 newPosition = Owner.Position + movement;

            if (!Game1.GetWorld().GetCurrentMap().IsColliding(Owner.GetHitbox(newPosition))) {
                Owner.SetPosition(newPosition); 
            }

            UpdateAnimation(direction);
        }

        private void UpdateAnimation(Vector2 direction) {
            if (direction.Y < 0) {
                Owner.SetCurrentAnimation(moveBackwardCycle);
                Owner.SetFacingVector(new Vector2(0, -1));  
            } else if (direction.Y > 0) {
                Owner.SetCurrentAnimation(moveForwardCycle);
                Owner.SetFacingVector(new Vector2(0, 1));  
            }

            if (direction.X < 0) {
                Owner.SetCurrentAnimation(moveRightCycle);
                Owner.GetCurrentAnimation().FlipHorizontally = true;
                Owner.SetFacingVector(new Vector2(-1, 0));  
            } else if (direction.X > 0) {
                Owner.SetCurrentAnimation(moveRightCycle);
                Owner.GetCurrentAnimation().FlipHorizontally = false;
                Owner.SetFacingVector(new Vector2(1, 0));  
            }

            Owner.GetCurrentAnimation().Play();
        }

        public override void Update(GameTime gameTime)
        {
            Owner.GetCurrentAnimation()?.Update(gameTime);
        }
    }
}
