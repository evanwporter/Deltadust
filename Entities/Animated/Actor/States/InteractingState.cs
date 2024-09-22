using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Deltadust.Entities.Animated.Actor.States
{
    public class InteractingState : EntityState {
        public InteractingState(AnimatedEntity entity) : base(entity) { }

        public override void Enter() {
            // Trigger the interaction event
            // _entity.EventManager.TriggerInteraction();
        }

        public override void Update(GameTime gameTime) {
            // Interacting state logic (e.g., display dialogue)
        }

        public override void Exit() {
            // Logic for exiting the interaction state
        }
    }
}