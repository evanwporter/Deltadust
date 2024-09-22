using Deltadust.Entities.Animated.Actor.States;

namespace Deltadust.Entities.Animated.Monsters.States
{
    public abstract class MonsterState : EntityState
    {

        protected new Monster _entity;

        public MonsterState(AnimatedEntity entity) : base() {
            _entity = (Monster) entity;
        }
    }
}