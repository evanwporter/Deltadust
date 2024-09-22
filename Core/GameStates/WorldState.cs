using Deltadust.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Core.GameStates
{
    public class WorldState : GameState
    {
        private WorldEngine _world;
        private KeyboardState _previousKeyboardState;


        public WorldState() : base() { }

        public override void Initialize() {
            _world = new WorldEngine(Game1.GetGraphicsDevice(), Game1.GetContent(), Game1.GetEventManager());
            _world.Initialize();
            Game1.SetWorld(_world);
        }

        public override void LoadContent() {
            _world.LoadContent();
            _previousKeyboardState = Keyboard.GetState(); // Initialize the previous keyboard state
        }

        public override void Update(GameTime gameTime) {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.I) && !_previousKeyboardState.IsKeyDown(Keys.I)) {
                Game1.PushState(new InventoryState(_world.GetPlayer().GetInventory()));
            }
            else {
                _world.Update(gameTime);
            }
            _previousKeyboardState = currentKeyboardState;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            _world.Draw(gameTime, spriteBatch, Game1.GetContent().Load<SpriteFont>("Fonts/ArialFont"));
        }

        public override void UnloadContent() {
            _world = null;
        }
    }
}