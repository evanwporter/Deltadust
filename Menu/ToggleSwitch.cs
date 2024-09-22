using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Menu
{
    public class ToggleSwitch
    {
        public Rectangle Area { get; private set; }
        private bool _isToggled;
        private Color _toggledColor = Color.Green;
        private Color _untoggledColor = Color.Red;
        private Color _currentColor;

        public ToggleSwitch(Rectangle area, bool isToggled)
        {
            Area = area;
            _isToggled = isToggled;
            _currentColor = _isToggled ? _toggledColor : _untoggledColor;
        }

        public bool IsToggled => _isToggled;

        public void Update(MouseState mouseState)
        {
            if (Area.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                _isToggled = !_isToggled;
                _currentColor = _isToggled ? _toggledColor : _untoggledColor;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Area, _currentColor);
        }
    }
}
