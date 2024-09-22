using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Deltadust.Menu
{
    public class MainMenu
    {
        private List<Button> _buttons;
        private SpriteFont _font;

        public MainMenu(SpriteFont font)
        {
            _font = font;
            _buttons = [];
        }

        public void Initialize()
        {
            _buttons.Add(new Button(new Rectangle(300, 200, 200, 50), "Start Game", _font));
            _buttons.Add(new Button(new Rectangle(300, 270, 200, 50), "Options", _font));
            _buttons.Add(new Button(new Rectangle(300, 340, 200, 50), "Exit", _font));
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            foreach (var button in _buttons)
            {
                button.Update(currentMouseState);
            }
        }

        public string GetClickedButton()
        {
            MouseState currentMouseState = Mouse.GetState();

            foreach (var button in _buttons)
            {
                if (button.IsClicked(currentMouseState))
                {
                    return button.Text;
                }
            }

            return null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
