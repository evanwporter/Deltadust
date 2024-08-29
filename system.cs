using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections;
using System.Collections.Generic;

    namespace MyGame {
    public abstract class System
    {
        public abstract void Update(GameTime gameTime, IEnumerable<Entity> entities);
    }

    public class MovementSystem : System
    {
        private EntityManager _entityManager;

        public MovementSystem(EntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var transform = _entityManager.GetComponent<TransformComponent>(entity);

                if (transform != null)
                {
                    transform.Position += transform.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
    }

    public class RenderSystem : System
    {
        private SpriteBatch _spriteBatch;
        private EntityManager _entityManager;

        public RenderSystem(SpriteBatch spriteBatch, EntityManager entityManager)
        {
            _spriteBatch = spriteBatch;
            _entityManager = entityManager;
        }

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var transform = _entityManager.GetComponent<TransformComponent>(entity);
                var render = _entityManager.GetComponent<RenderComponent>(entity);

                if (transform != null && render != null)
                {
                    // Update the animation
                    render.Sprite.Update(gameTime);

                    // Draw the animated sprite at the entity's position
                    render.Sprite.Draw(_spriteBatch, transform.Position);
                }
            }
        }
    }

}