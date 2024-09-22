using Microsoft.Xna.Framework;

namespace Deltadust.Entities.Animated.Actor.States
{
    public abstract class EntityState {
        protected AnimatedEntity _entity;

        protected EntityState() {}

        public EntityState(AnimatedEntity entity) {
            _entity = entity;
        }

        public abstract void Enter();
        public abstract void Update(GameTime gameTime);
        public abstract void Exit();
    }
}