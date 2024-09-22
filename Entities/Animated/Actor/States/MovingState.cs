using Deltadust.Core.Input;
using Deltadust.Entities.Animated;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Entities.Animated.Actor.States
{
    public class MovingState : EntityState {
        public MovingState(AnimatedEntity entity) : base(entity) { }

        public override void Enter() {
            // Logic for entering the moving state
        }

        public override void Update(GameTime gameTime) {
            Vector2 input = InputManager.GetMovementInput();
            if (input == Vector2.Zero) {
                _entity.GetComponent<Components.StateMachineComponent>().SetState(new IdleState(_entity));
                return;
            }
            _entity.GetComponent<Components.MovementComponent>().Move(input, gameTime);

            if (InputManager.IsKeyDown(Keys.T)) {
                _entity.GetComponent<Components.StateMachineComponent>().SetState(new AttackingState(_entity));
            }
        }

        public override void Exit() {
            // Logic for exiting the moving state
        }
    }
}