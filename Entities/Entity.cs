using Microsoft.Xna.Framework;

namespace Deltadust.Entities
{
    public interface IEntity : IDrawable
    {
        public Vector2 Position { get; }
    }
}