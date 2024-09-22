using Deltadust.Core;
using Deltadust.Entities.Animated.Components;
using Deltadust.Entities.Animated.Monsters.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;

namespace Deltadust.Entities.Animated.Monsters
{
    public class Monster : NPC   
    {

        public float VisionRadius = 50f;
        private AnimatedSprite _moveAnimation;
        private AnimatedSprite _idleAnimation;

        public string Name = "Monsters/slime";
        
        public int ID_Name = 1;

        public Monster(Vector2 startPosition)
            : base(startPosition) 
        {
            SpriteSheet spriteSheet = ResourceManager.LoadSprite(Name);

            _moveAnimation = spriteSheet.CreateAnimatedSprite("Jump");  // Assuming "Jump" is the move animation
            _idleAnimation = spriteSheet.CreateAnimatedSprite("Idle"); 

            _currentAnimation = _idleAnimation;
            
            AddComponent<MovementComponent>(new MovementComponent(this, 50f));
            AddComponent<IdleComponent>(new IdleComponent(this));
            AddComponent<StateMachineComponent>(new StateMachineComponent(this, new MonsterIdleState(this, Game1.GetWorld().GetPlayer())));
        }

        public override void Update(GameTime gameTime)
        {
            // GetComponent<StateMachineComponent>().Update(gameTime);

            _currentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            spriteBatch.Draw(_currentAnimation, _position);

            #if DEBUG
            DrawHitbox(spriteBatch);
            #endif
        }

        public void SwitchToMoveAnimation()
        {
            _currentAnimation = _moveAnimation;
            _currentAnimation.Play();
        }

        public void SwitchToIdleAnimation()
        {
            _currentAnimation = _idleAnimation;
            _currentAnimation.Play();
        }
    }
}