using Deltadust.Core;
using Deltadust.World;
using MonoGame.Aseprite;

namespace Deltadust.Entities.Animated.Components
{
    public class IdleComponent : Component
    {

        public AnimatedSprite standForward {get; private set;}
        public AnimatedSprite standBackward {get; private set;}
        public AnimatedSprite standRight {get; private set;}

        public IdleComponent(AnimatedEntity owner) : base(owner) {}

        
        public void LoadContent(string spriteSheetName) {
            SpriteSheet characterSheet = ResourceManager.LoadSprite(spriteSheetName);
            standForward = characterSheet.CreateAnimatedSprite("Stand Down");
            standBackward = characterSheet.CreateAnimatedSprite("Stand Up");
            standRight = characterSheet.CreateAnimatedSprite("Stand Right");

            Owner.SetCurrentAnimation(standForward);
        } 

        public void SwitchToIdleAnimation()
        {
            if (Owner.GetCurrentAnimation() == Owner.GetComponent<MovementComponent>().moveForwardCycle)
            {
                Owner.SetCurrentAnimation(standForward);
            }
            else if (Owner.GetCurrentAnimation() == Owner.GetComponent<MovementComponent>().moveBackwardCycle)
            {
                Owner.SetCurrentAnimation(standBackward);
            }
            else if (Owner.GetCurrentAnimation() == Owner.GetComponent<MovementComponent>().moveRightCycle)
            {
                if (Owner.GetCurrentAnimation().FlipHorizontally) {
                    Owner.SetCurrentAnimation(standRight);
                    Owner.GetCurrentAnimation().FlipHorizontally = true;

                }
                else {
                    Owner.SetCurrentAnimation(standRight);
                    Owner.GetCurrentAnimation().FlipHorizontally = false;
                }
            }
            #if DEBUG
            else {
                System.Diagnostics.Debug.WriteLine("Unknown Animation when trying to switch to Idle Animation.");
            }
            #endif
            Owner.GetCurrentAnimation().Play();
        }
    }
}