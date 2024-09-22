using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Menu
{
    public class Button
    {
        public Rectangle Area { get; private set; }
        public string Text { get; private set; }
        private SpriteFont _font;
        private Color _normalColor = Color.White;
        private Color _hoverColor = Color.Gray;
        private Color _currentColor;
        private bool _isHovered;

        public Button(Rectangle area, string text, SpriteFont font)
        {
            Area = area;
            Text = text;
            _font = font;
            _currentColor = _normalColor;
        }

        public void Update(MouseState mouseState)
        {
            _isHovered = Area.Contains(mouseState.Position);
            _currentColor = _isHovered ? _hoverColor : _normalColor;
        }

        public bool IsClicked(MouseState mouseState)
        {
            return _isHovered && mouseState.LeftButton == ButtonState.Pressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the button rectangle
            Texture2D rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });
            spriteBatch.Draw(rectTexture, Area, _currentColor);

            // Draw the text centered in the button
            Vector2 textSize = _font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
                Area.X + (Area.Width / 2) - (textSize.X / 2),
                Area.Y + (Area.Height / 2) - (textSize.Y / 2)
            );
            spriteBatch.DrawString(_font, Text, textPosition, Color.Black);
        }
    }
}
