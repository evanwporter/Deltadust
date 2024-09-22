using Deltadust.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Core.GameStates
{
    public class MainMenuState : GameState
    {
        private SpriteFont _font;
        private MainMenu _menuGUI;

        public MainMenuState() : base() { }

        public override void Initialize()
        {
            _menuGUI = new(Game1.GetContent().Load<SpriteFont>("Fonts/ArialFont"));
            _menuGUI.Initialize();
        }

        public override void LoadContent()
        {
            _font = Game1.GetContent().Load<SpriteFont>("Fonts/ArialFont");
        }

        public override void Update(GameTime gameTime)
        {
            _menuGUI.Update(gameTime);

            // Check if any button was clicked
            string clickedButton = _menuGUI.GetClickedButton();
            if (clickedButton != null)
            {
                if (clickedButton == "Start Game")
                {
                    Game1.ChangeState(new WorldState()); // Start the game
                }
                else if (clickedButton == "Options") {
                    Game1.ChangeState(new OptionsState()); // Open the options menu
                }
                else if (clickedButton == "Exit")
                {
                    Game1.ExitGame(); // Exit the game
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _menuGUI.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void UnloadContent()
        {
        }
    }
}
