using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using MonoGame.Aseprite;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame {
    public class Player
    {
        private Vector2 _position;
        private readonly float _speed;
        private AnimatedSprite _runForwardCycle;
        private AnimatedSprite _runBackwardCycle;
        private AnimatedSprite _runRightCycle;

        private AnimatedSprite _standForward;
        private AnimatedSprite _standBackward;
        private AnimatedSprite _standRight;

        private AnimatedSprite _currentAnimation;

        private Inventory _inventory;
        private bool _showInventory;
        private string _inventoryFilePath;

        private KeyboardState _previousKeyboardState;

        public Player(Vector2 startPosition, float speed, string inventoryFilePath)
        {
            _position = startPosition;
            _speed = speed;
            _inventoryFilePath = inventoryFilePath;

            _inventory = Inventory.LoadFromFile(_inventoryFilePath);
            _showInventory = false;

            _previousKeyboardState = Keyboard.GetState();
        }

        public void LoadContent(AsepriteFile aseFile, GraphicsDevice graphicsDevice)
        {
            SpriteSheet spriteSheet = aseFile.CreateSpriteSheet(graphicsDevice);

            // Running animations
            _runForwardCycle = spriteSheet.CreateAnimatedSprite("Run Forward");
            _runBackwardCycle = spriteSheet.CreateAnimatedSprite("Run Backward");
            _runRightCycle = spriteSheet.CreateAnimatedSprite("Run Right");

            // Standing animations
            _standForward = spriteSheet.CreateAnimatedSprite("Stand Forward");
            _standBackward = spriteSheet.CreateAnimatedSprite("Stand Backward");
            _standRight = spriteSheet.CreateAnimatedSprite("Stand Right");

            _currentAnimation = _runForwardCycle;
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            // Toggle inventory on/off
            if (keyboardState.IsKeyDown(Keys.E) && !_previousKeyboardState.IsKeyDown(Keys.E))
            {
                _showInventory = !_showInventory;
            }

            // Only handle movement if the inventory is not open
            if (!_showInventory)
            {
                HandleMovement(gameTime, keyboardState);
            }

            _previousKeyboardState = keyboardState;
        }

        private void HandleMovement(GameTime gameTime, KeyboardState keyboardState)
        {
            bool isMoving = false;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                _position.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runBackwardCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                _position.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runForwardCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                _position.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runRightCycle;
                _currentAnimation.FlipHorizontally = true;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runRightCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }

            if (isMoving)
            {
                _currentAnimation.Play();
                _currentAnimation.Update(gameTime);
            }
            else
            {
                if (_currentAnimation == _runForwardCycle)
                {
                    _currentAnimation = _standForward;
                }
                else if (_currentAnimation == _runBackwardCycle)
                {
                    _currentAnimation = _standBackward;
                }
                else if (_currentAnimation == _runRightCycle && !_currentAnimation.FlipHorizontally)
                {
                    _currentAnimation = _standRight;
                    _currentAnimation.FlipHorizontally = false;
                }
                else if (_currentAnimation.FlipHorizontally)
                {
                    _currentAnimation = _standRight;
                    _currentAnimation.FlipHorizontally = true;
                }
                _currentAnimation.Play();
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix)
        {
            // Draw player
            spriteBatch.Draw(_currentAnimation, _position);

            // Draw inventory if it's shown
            if (_showInventory)
            {
                DrawInventory(spriteBatch, font, viewMatrix);
            }
        }

        private void DrawInventory(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix)
        {
            // Draw the inventory relative to the camera position (top-left corner of the screen)
            Vector2 position = Vector2.Transform(new Vector2(10, 10), Matrix.Invert(viewMatrix));

            spriteBatch.DrawString(font, "Inventory:", position, Color.White);
            position.Y += 30;

            foreach (string item in _inventory.Items)
            {
                spriteBatch.DrawString(font, item, position, Color.White);
                position.Y += 30;
            }
        }

        public Vector2 Position => _position;
    }
}
