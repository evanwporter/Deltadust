#define DEBUG

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

namespace MyGame {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 _characterPosition;
        private readonly float _speed = 100f;

        private readonly int hitboxWidth = 20; // New width for the hitbox and drawing

        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer _collisionLayer;

        private SpriteSheet _spriteSheet;
        private AnimatedSprite _runForwardCycle;
        private AnimatedSprite _runBackwardCycle;
        private AnimatedSprite _runRightCycle;

        private AnimatedSprite _standForward;
        private AnimatedSprite _standBackward;
        private AnimatedSprite _standRight;

        private AnimatedSprite _currentAnimation;

        private Vector2 _baseResolution = new Vector2(800, 600);
        private Vector2 _scale;
        private Camera _camera;

        private SpriteFont _font;

        private Inventory _inventory;
        private bool _showInventory;
        private string _inventoryFilePath;

        private KeyboardState _previousKeyboardState;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            _inventoryFilePath = Path.Combine(Content.RootDirectory, "inventory.xml");

            _inventory = Inventory.LoadFromFile(_inventoryFilePath);

            _showInventory = false;

            _characterPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            _camera = new Camera(GraphicsDevice.Viewport);

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Fonts/ArialFont");

            // Load the Tiled map
            _tiledMap = Content.Load<TiledMap>("Map/starter_island");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            AsepriteFile aseFile = Content.Load<AsepriteFile>("Character/character");

            _spriteSheet = aseFile.CreateSpriteSheet(GraphicsDevice);

            // Running animations
            _runForwardCycle = _spriteSheet.CreateAnimatedSprite("Run Forward");
            _runBackwardCycle = _spriteSheet.CreateAnimatedSprite("Run Backward");
            _runRightCycle = _spriteSheet.CreateAnimatedSprite("Run Right");

            _standForward = _spriteSheet.CreateAnimatedSprite("Stand Forward");
            _standBackward = _spriteSheet.CreateAnimatedSprite("Stand Backward");
            _standRight = _spriteSheet.CreateAnimatedSprite("Stand Right");

            _currentAnimation = _runForwardCycle;

            _collisionLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Collisions");

            #if DEBUG
            if (_collisionLayer == null) {
                System.Diagnostics.Debug.WriteLine("Collision tile layer not found!");
            }
            #endif

            _collisionLayer.IsVisible = false;
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();
            Vector2 previousPosition = _characterPosition;

            bool isMoving = false;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) {
                _characterPosition.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runBackwardCycle;
                _currentAnimation.FlipHorizontally = false; // Ensure it is not flipped
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) {
                _characterPosition.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runForwardCycle;
                _currentAnimation.FlipHorizontally = false; // Ensure it is not flipped
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) {
                _characterPosition.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runRightCycle;
                _currentAnimation.FlipHorizontally = true; // Flip the right animation to simulate running left
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)) {
                _characterPosition.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runRightCycle;
                _currentAnimation.FlipHorizontally = false; // Ensure it is not flipped
                isMoving = true;
            }

            // Trigger the run animation if moving, otherwise switch to stand animation
            if (isMoving) {
                _currentAnimation.Play();
                _currentAnimation.Update(gameTime);
            }
            else {
                // Switch to the appropriate standing animation
                if (_currentAnimation == _runForwardCycle) {
                    _currentAnimation = _standForward;
                }
                else if (_currentAnimation == _runBackwardCycle) {
                    _currentAnimation = _standBackward;
                }
                else if (_currentAnimation == _runRightCycle && !_currentAnimation.FlipHorizontally) {
                    _currentAnimation = _standRight;
                    _currentAnimation.FlipHorizontally = false;
                }
                else if (_currentAnimation.FlipHorizontally) {
                    _currentAnimation = _standRight;
                    _currentAnimation.FlipHorizontally = true; // Maintain the flip for left standing
                }

                _currentAnimation.Play(); // Play the stand animation
            }

            if (keyboardState.IsKeyDown(Keys.E) && !_previousKeyboardState.IsKeyDown(Keys.E)) {
                _showInventory = !_showInventory;
                _previousKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.H) && !_previousKeyboardState.IsKeyDown(Keys.H)) {
                _inventory.AddItem("Health Potion");
                _inventory.SaveToFile(_inventoryFilePath);
                _previousKeyboardState = keyboardState;
            }

            // Update the camera position to follow the player
            _camera.Position = _characterPosition - new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) / _camera.Zoom;

            _tiledMapRenderer.Update(gameTime);

            base.Update(gameTime);
        }

        private bool IsCollidingWithTile(Rectangle playerRectangle) {
            int tileWidth = _tiledMap.TileWidth;
            int tileHeight = _tiledMap.TileHeight;

            var corners = new List<Vector2> {
                new Vector2(playerRectangle.Left, playerRectangle.Top),      // Top-left
                new Vector2(playerRectangle.Right, playerRectangle.Top),     // Top-right
                new Vector2(playerRectangle.Left, playerRectangle.Bottom),   // Bottom-left
                new Vector2(playerRectangle.Right, playerRectangle.Bottom)   // Bottom-right
            };

            foreach (var corner in corners) {
                ushort tileX = (ushort)(corner.X / tileWidth);
                ushort tileY = (ushort)(corner.Y / tileHeight);

                if (tileX < _collisionLayer.Width && tileY < _collisionLayer.Height) {
                    var tile = _collisionLayer.GetTile(tileX, tileY);
                    if (tile.GlobalIdentifier != 0) {
                        return true;
                    }
                }
            }

            return false;
        }


        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Apply camera transformation
            Matrix viewMatrix = _camera.GetViewMatrix();

            _tiledMapRenderer.Draw(viewMatrix);

            _spriteBatch.Begin(transformMatrix: viewMatrix, samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(_currentAnimation, _characterPosition);

            if (_showInventory)
            {
                DrawInventory();
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawInventory() {
            Vector2 position = new Vector2(10, 10);

            _spriteBatch.DrawString(_font, "Inventory:", position, Color.White);
            position.Y += 30;

            foreach (string item in _inventory.Items)
            {
                _spriteBatch.DrawString(_font, item, position, Color.White);
                position.Y += 30;
            }
        }
    }
}
