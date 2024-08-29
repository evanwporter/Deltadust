using System.Collections.Generic; // This namespace is needed for List<>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AsepriteDotNet.Aseprite;
using MonoGame.Aseprite;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;


namespace MyGame {
    public class Player : Entity
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

        private TiledMap _tiledMap;
        private Vector2 vector2;
        private float v1;
        private string v2;
        private TiledMapTileLayer collisionLayer;
        private readonly TiledMapTileLayer _collisionLayer;
        private readonly int hitboxWidth = 20;

        public Player(Vector2 startPosition, float speed, string inventoryFilePath, TiledMapTileLayer collisionLayer, TiledMap tiledMap)
            // : base(startPosition, speed)
        {
            _position = startPosition;
            _speed = speed;
            _inventoryFilePath = inventoryFilePath;

            _inventory = Inventory.LoadFromFile(_inventoryFilePath);
            _showInventory = false;

            _previousKeyboardState = Keyboard.GetState();
            _collisionLayer = collisionLayer;

            _tiledMap = tiledMap;
        }

        public Player(Vector2 vector2, float v1, string v2, TiledMapTileLayer collisionLayer)
        {
            this.vector2 = vector2;
            this.v1 = v1;
            this.v2 = v2;
            this.collisionLayer = collisionLayer;
        }

        public override void LoadContent(AsepriteFile aseFile, GraphicsDevice graphicsDevice) {
            
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

        public override void Update(GameTime gameTime) {
            var keyboardState = Keyboard.GetState();

            // Toggle inventory on/off
            if (keyboardState.IsKeyDown(Keys.E) && !_previousKeyboardState.IsKeyDown(Keys.E)) {
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
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                movement.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runBackwardCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                movement.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runForwardCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                movement.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runRightCycle;
                _currentAnimation.FlipHorizontally = true;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                movement.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _runRightCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }

            Vector2 newPosition = Position + movement;

            Rectangle playerHitbox = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                hitboxWidth,
                hitboxWidth
            );

            if (isMoving)
            {
                if (!IsCollidingWithTile(playerHitbox))
                {
                    _position = newPosition;
                }
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

        private bool IsCollidingWithTile(Rectangle playerRectangle) {
            
            int tileWidth = _tiledMap.TileWidth;
            int tileHeight = _tiledMap.TileHeight;

            var corners = new List<Vector2> {
                new Vector2(playerRectangle.Left, playerRectangle.Top),      // Top-left
                new Vector2(playerRectangle.Right, playerRectangle.Top),     // Top-right
                new Vector2(playerRectangle.Left, playerRectangle.Bottom),   // Bottom-left
                new Vector2(playerRectangle.Right, playerRectangle.Bottom)   // Bottom-right
            };

            foreach (var corner in corners)
            {
                ushort tileX = (ushort)(corner.X / tileWidth);
                ushort tileY = (ushort)(corner.Y / tileHeight);

                if (tileX < _collisionLayer.Width && tileY < _collisionLayer.Height)
                {
                    var tile = _collisionLayer.GetTile(tileX, tileY);
                    if (tile.GlobalIdentifier != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix)
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
