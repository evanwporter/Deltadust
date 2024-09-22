using Microsoft.Xna.Framework;

namespace Deltadust.Entities.Animated.Actor.States
{
    public class AttackingState : EntityState {
        public AttackingState(AnimatedEntity entity) : base(entity) { }

        public override void Enter() {
            _entity.SwitchToAttackAnimation();
        }

        public override void Update(GameTime gameTime) {
            if (!_entity.IsAttacking()) { 
                _entity.GetComponent<Components.StateMachineComponent>().SetState(new IdleState(_entity)); 
            }
        }

        public override void Exit() {
        }
    }
}