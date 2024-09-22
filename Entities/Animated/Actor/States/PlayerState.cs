using Microsoft.Xna.Framework;

namespace Deltadust.Entities.Animated.Actor.States
{
    public abstract class PlayerState {
        protected Player _player;

        public PlayerState(Player player) {
            _player = player;
        }

        public abstract void Enter();
        public abstract void Update(GameTime gameTime);
        public abstract void Exit();
    }
}