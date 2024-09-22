
using Deltadust.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust.Core.GameStates
{
    public class OptionsState : GameState
    {
        private Options _optionsGUI;
        private Texture2D _sliderTexture, _handleTexture, _toggleTexture;

        public OptionsState() : base() { }

        public override void Initialize()
        {
            _optionsGUI = new(Game1.GetContent().Load<SpriteFont>("Fonts/ArialFont"));
            _optionsGUI.Initialize(Game1.GetGraphicsDevice());
        }

        public override void LoadContent()
        {
            _sliderTexture = new Texture2D(Game1.GetGraphicsDevice(), 1, 1);
            _sliderTexture.SetData(new[] { Color.White });

            _handleTexture = new Texture2D(Game1.GetGraphicsDevice(), 1, 1);
            _handleTexture.SetData(new[] { Color.White });

            _toggleTexture = new Texture2D(Game1.GetGraphicsDevice(), 1, 1);
            _toggleTexture.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime) {
            _optionsGUI.Update(gameTime);

            string clickedButton = _optionsGUI.GetClickedButton();
            if (clickedButton == "Apply")
            {
                if (_optionsGUI.IsFullscreen()) {
                    Game1.GetGraphics().IsFullScreen = true;
                }
                else
                {
                    Game1.GetGraphics().IsFullScreen = false;
                }

                Game1.GetGraphics().ApplyChanges();

                Game1.ChangeState(new MainMenuState());
            }
            else if (clickedButton == "Cancel")
            {
                Game1.ChangeState(new MainMenuState());
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _optionsGUI.Draw(spriteBatch, _sliderTexture, _handleTexture, _toggleTexture);

            spriteBatch.End();
        }

        public override void UnloadContent()
        {
        }
    }
}