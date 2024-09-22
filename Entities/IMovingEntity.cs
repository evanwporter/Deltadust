using Microsoft.Xna.Framework;

namespace Deltadust.Entities
{
    public interface IMovingEntity : IEntity
    {
        public float Speed {get; set;}

        public void Update(GameTime gameTime);
    }
}