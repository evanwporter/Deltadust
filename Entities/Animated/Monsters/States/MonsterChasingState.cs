using Deltadust.Entities.Animated.Actor;
using Deltadust.Entities.Animated.Components;
using Microsoft.Xna.Framework;

namespace Deltadust.Entities.Animated.Monsters.States {
    public class MonsterChasingState : MonsterState {
        private readonly Player _player;

        public MonsterChasingState(AnimatedEntity monster, Player player) : base(monster) {
            _player = player;
        }

        public override void Enter() {
        }

        public override void Update(GameTime gameTime) {
            Vector2 directionToPlayer = Vector2.Normalize(_player.Position - _entity.Position);
            _entity.GetComponent<MovementComponent>().Move(directionToPlayer, gameTime);

            if (Vector2.Distance(_entity.Position, _player.Position) > _entity.VisionRadius) {
                _entity.GetComponent<StateMachineComponent>().SetState(new MonsterIdleState(_entity, _player));
            }
        }

        public override void Exit() {
        }
    }
}
