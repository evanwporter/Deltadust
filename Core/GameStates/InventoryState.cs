using Deltadust.ItemManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Core.GameStates
{
    public class InventoryState : GameState
    {
        private Inventory _inventory;
        private SpriteFont _font;
        private bool _inventoryOpen;
        private KeyboardState _previousKeyboardState;

        public InventoryState(Inventory inventory) : base()
        {
            _inventory = inventory;
        }

        // public InventoryState(Game1 game, Inventory inventory, KeyboardState previousKeyboardState) : base(game) {
        //     _previousKeyboardState = previousKeyboardState;
        //     _inventory = inventory;
        // }

        public InventoryState(bool inventoryOpen) : base() {
            _inventoryOpen = inventoryOpen;
        }

        public override void Initialize() { }

        public override void LoadContent() {
            _font = Game1.GetContent().Load<SpriteFont>("Fonts/ArialFont");
            _previousKeyboardState = Keyboard.GetState();

        }

        public override void Update(GameTime gameTime) {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.I) && !_previousKeyboardState.IsKeyDown(Keys.I)) {
                Game1.PopState();
            }
            _previousKeyboardState = currentKeyboardState;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _inventory.Draw(spriteBatch, Matrix.Identity);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            // Unload any resources specific to inventory
        }
    }
}