using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust.Core.GameStates
{
    public abstract class GameState
    {
        public abstract void Initialize();

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void UnloadContent();
    }
}