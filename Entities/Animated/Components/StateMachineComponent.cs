using Deltadust.Entities.Animated.Actor.States;
using Microsoft.Xna.Framework;

namespace Deltadust.Entities.Animated.Components
{
    public class StateMachineComponent : Component
    {
        private EntityState _currentState;

        public StateMachineComponent(AnimatedEntity owner) : base(owner)
        {
            _currentState = new IdleState(owner);
        }

        public StateMachineComponent(AnimatedEntity owner, EntityState initialState) : base(owner)
        {
            _currentState = initialState;
            _currentState.Enter();
        }

        public void SetState(EntityState newState)
        {
            if (_currentState?.GetType() == newState.GetType())
                return; // Avoid re-entering the same state

            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public override void Update(GameTime gameTime)
        {
            _currentState.Update(gameTime);
        }
    }
}
