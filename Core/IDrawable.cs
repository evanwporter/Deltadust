using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust {
    public interface IDrawable {
        public float Y { get; }
        public void Draw(SpriteBatch spriteBatch, Matrix viewMatrix);
    }
}