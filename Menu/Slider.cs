using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Slider
{
    public Rectangle Bar { get; private set; }
    private Rectangle _handle;
    public Rectangle Handle => _handle;

    private int _minValue, _maxValue;
    private int _currentValue;
    private bool _isDragging;

    public Slider(Rectangle bar, int minValue, int maxValue, int initialValue)
    {
        Bar = bar;
        _minValue = minValue;
        _maxValue = maxValue;
        _currentValue = initialValue;

        _handle = new Rectangle(Bar.X + (initialValue - minValue) * Bar.Width / (maxValue - minValue) - 10, Bar.Y - 5, 20, Bar.Height + 10);
    }

    public int GetValue() => _currentValue;

    public void Update(MouseState mouseState)
    {
        if (_handle.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
        {
            _isDragging = true;
        }

        if (_isDragging)
        {
            if (mouseState.LeftButton == ButtonState.Released)
            {
                _isDragging = false;
            }
            else
            {
                SetHandlePosition(mouseState.X);
            }
        }
    }

    // Use this method to set the handle position
    private void SetHandlePosition(int mouseX)
    {
        int newX = MathHelper.Clamp(mouseX - _handle.Width / 2, Bar.X, Bar.X + Bar.Width - _handle.Width);
        _handle = new Rectangle(newX, _handle.Y, _handle.Width, _handle.Height);
        _currentValue = (int)((float)(newX - Bar.X) / Bar.Width * (_maxValue - _minValue)) + _minValue;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D barTexture, Texture2D handleTexture)
    {
        spriteBatch.Draw(barTexture, Bar, Color.Gray); // Draw the bar
        spriteBatch.Draw(handleTexture, _handle, Color.White); // Draw the handle
    }
}
