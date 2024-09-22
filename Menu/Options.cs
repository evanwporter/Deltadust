using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Menu
{
    public class Options
    {
        private Slider _volumeSlider;
        private ToggleSwitch _fullscreenToggle;
        private Button _applyButton;
        private Button _cancelButton;
        private SpriteFont _font;

        public Options(SpriteFont font)
        {
            _font = font;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            // Create sliders, buttons, and toggles
            _volumeSlider = new Slider(new Rectangle(300, 200, 300, 20), 0, 100, 50); // Volume slider from 0 to 100, starts at 50
            _fullscreenToggle = new ToggleSwitch(new Rectangle(300, 250, 50, 25), false); // Fullscreen toggle off by default
            _applyButton = new Button(new Rectangle(300, 320, 200, 50), "Apply", _font);
            _cancelButton = new Button(new Rectangle(300, 390, 200, 50), "Cancel", _font);
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            _volumeSlider.Update(currentMouseState);
            _fullscreenToggle.Update(currentMouseState);
            _applyButton.Update(currentMouseState);
            _cancelButton.Update(currentMouseState);
        }

        public string GetClickedButton()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (_applyButton.IsClicked(currentMouseState))
            {
                return "Apply";
            }
            else if (_cancelButton.IsClicked(currentMouseState))
            {
                return "Cancel";
            }

            return null;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D sliderTexture, Texture2D handleTexture, Texture2D toggleTexture)
        {
            _volumeSlider.Draw(spriteBatch, sliderTexture, handleTexture);
            _fullscreenToggle.Draw(spriteBatch, toggleTexture);
            _applyButton.Draw(spriteBatch);
            _cancelButton.Draw(spriteBatch);

            // Draw labels
            spriteBatch.DrawString(_font, "Volume", new Vector2(200, 200), Color.White);
            spriteBatch.DrawString(_font, "Fullscreen", new Vector2(200, 250), Color.White);
        }

        public int GetVolume() => _volumeSlider.GetValue();
        public bool IsFullscreen() => _fullscreenToggle.IsToggled;
    }
}
