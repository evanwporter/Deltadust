using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deltadust.Core.Input
{
    public static class InputManager
    {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;

        private static bool _isThereInput;

        public static void Initialize()
        {
            _currentKeyboardState = Keyboard.GetState();
            _previousKeyboardState = _currentKeyboardState;

            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
        }

        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _isThereInput = _currentKeyboardState.GetPressedKeyCount() > 0;
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return !_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key);
        }

        public static bool IsLeftMouseButtonClicked()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;
        }

        public static bool IsRightMouseButtonClicked()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released;
        }

        public static Point GetMousePosition()
        {
            return _currentMouseState.Position;
        }

        public static bool IsMouseWithin(Rectangle area)
        {
            return area.Contains(_currentMouseState.Position);
        }

        public static bool IsMoveUp()
        {
            return IsKeyDown(Keys.W) || IsKeyDown(Keys.Up);
        }

        public static bool IsMoveDown()
        {
            return IsKeyDown(Keys.S) || IsKeyDown(Keys.Down);
        }

        public static bool IsMoveLeft()
        {
            return IsKeyDown(Keys.A) || IsKeyDown(Keys.Left);
        }

        public static bool IsMoveRight()
        {
            return IsKeyDown(Keys.D) || IsKeyDown(Keys.Right);
        }

        public static bool IsThereInput => _isThereInput;

        public static Vector2 GetMovementInput() {
            Vector2 movement = Vector2.Zero;
            if (IsKeyDown(Keys.W) || IsKeyDown(Keys.Up)) {
                movement.Y -= 1;
            }
            if (IsKeyDown(Keys.S) || IsKeyDown(Keys.Down)) {
                movement.Y += 1;
            }
            if (IsKeyDown(Keys.A) || IsKeyDown(Keys.Left)) {
                movement.X -= 1;
            }
            if (IsKeyDown(Keys.D) || IsKeyDown(Keys.Right)) {
                movement.X += 1;
            }
            return movement;
        }

    }
}
