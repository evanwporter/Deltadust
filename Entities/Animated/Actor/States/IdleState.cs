using Deltadust.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Entities.Animated.Actor.States
{
    public class IdleState : EntityState {
        public IdleState(AnimatedEntity entity) : base(entity) { }

        public override void Enter() {
            _entity.GetComponent<Components.IdleComponent>().SwitchToIdleAnimation();
        }

        public override void Update(GameTime gameTime) {
            var input = InputManager.GetMovementInput();
            if (input != Vector2.Zero) {
                _entity.GetComponent<Components.StateMachineComponent>().SetState(new MovingState(_entity));
            } else if (InputManager.IsKeyDown(Keys.T)) {
                _entity.GetComponent<Components.StateMachineComponent>().SetState(new AttackingState(_entity));
            }
        }

        public override void Exit() {
            // Logic for when the entity exits the idle state
        }
    }
}