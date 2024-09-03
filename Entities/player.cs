using Deltadust.Core;
using Deltadust.Events;
using Deltadust.World;
using Deltadust.ItemManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Aseprite;


namespace Deltadust.Entities {
    public class Player : Entity
    {
        private new readonly float _speed = 100f;
        private AnimatedSprite _moveForwardCycle;
        private AnimatedSprite _moveBackwardCycle;
        private AnimatedSprite _moveRightCycle;

        private AnimatedSprite _standForward;
        private AnimatedSprite _standBackward;
        private AnimatedSprite _standRight;

        private AnimatedSprite _currentAnimation;

        private readonly Inventory _inventory;
        private bool _showInventory;

        public bool InventoryOpen {
            get {
                return _showInventory;
            }
        }
        private KeyboardState _previousKeyboardState;
        private readonly int hitboxWidth = 20;

        public Player(Vector2 startPosition, string inventoryFilePath, WorldEngine world, ResourceManager resourceManager, CentralEventHandler eventHandler)
            : base(startPosition, world, resourceManager, eventHandler)
        {
            _inventory = Inventory.LoadFromFile(inventoryFilePath);
            _showInventory = false;
            _previousKeyboardState = Keyboard.GetState();
        }

        public override void LoadContent() {
            SpriteSheet spriteSheet = _resourceManager.LoadSprite("Character/character");

            #region Running animations
            _moveForwardCycle = spriteSheet.CreateAnimatedSprite("Move Forward");
            _moveBackwardCycle = spriteSheet.CreateAnimatedSprite("Move Backward");
            _moveRightCycle = spriteSheet.CreateAnimatedSprite("Move Right");
            #endregion

            #region Standing animations
            _standForward = spriteSheet.CreateAnimatedSprite("Stand Forward");
            _standBackward = spriteSheet.CreateAnimatedSprite("Stand Backward");
            _standRight = spriteSheet.CreateAnimatedSprite("Stand Right");
            #endregion

            _currentAnimation = _moveForwardCycle;
        }

        public override void Update(GameTime gameTime) {
            var keyboardState = Keyboard.GetState();

            // Toggle inventory on/off
            if (keyboardState.IsKeyDown(Keys.E) && !_previousKeyboardState.IsKeyDown(Keys.E)) {
                _showInventory = !_showInventory;
            }

            // Only handle movement if the inventory is not open
            if (!_showInventory) {
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
                _currentAnimation = _moveBackwardCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                movement.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _moveForwardCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                movement.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _moveRightCycle;
                _currentAnimation.FlipHorizontally = true;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                movement.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnimation = _moveRightCycle;
                _currentAnimation.FlipHorizontally = false;
                isMoving = true;
            }

            Vector2 newPosition = Position + new Vector2(movement.X, 0);
            Rectangle playerHitbox = GetHitbox(newPosition);
            if (!_world.IsCollidingWithTile(playerHitbox))
            {
                _position.X = newPosition.X;
            }

            newPosition = Position + new Vector2(0, movement.Y);
            playerHitbox = GetHitbox(newPosition);
            if (!_world.IsCollidingWithTile(playerHitbox))
            {
                _position.Y = newPosition.Y;
            }


            if (isMoving)
            {
                if (!_world.IsCollidingWithTile(playerHitbox))
                {
                    _position = newPosition;
                }
                _currentAnimation.Play();
                _currentAnimation.Update(gameTime);
            }
            else
            {
                if (_currentAnimation == _moveForwardCycle)
                {
                    _currentAnimation = _standForward;
                }
                else if (_currentAnimation == _moveBackwardCycle)
                {
                    _currentAnimation = _standBackward;
                }
                else if (_currentAnimation == _moveRightCycle && !_currentAnimation.FlipHorizontally)
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

        public Rectangle GetHitbox(Vector2 position)
        {
            return new Rectangle(
                (int)(position.X + ((32 - hitboxWidth) / 2)),
                (int)(position.Y + 32 + 32 - 20),
                hitboxWidth,
                20
            );
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Matrix viewMatrix)
        {
            spriteBatch.Draw(_currentAnimation, _position);

            if (_showInventory)
            {
                _inventory.Draw(spriteBatch, font, viewMatrix);
            }

            Texture2D hitboxTexture = new(spriteBatch.GraphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.Red });

            spriteBatch.Draw(hitboxTexture, GetHitbox(_position), Color.Red * 0.5f);
        }

        public void SetPosition(Vector2 newPosition)
        {
            _position = newPosition;
        }
    }
}
