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

        private AnimatedSprite _attackForward;
        private AnimatedSprite _attackBackward;
        private AnimatedSprite _attackRight;

        private AnimatedSprite _currentAnimation;
        private AnimatedSprite _currentAttackAnimation;

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
            SpriteSheet characterSheet = _resourceManager.LoadSprite("Character/character");
            SpriteSheet attackSheet = _resourceManager.LoadSprite("Character/swipe");

            #region Running animations
            _moveForwardCycle = characterSheet.CreateAnimatedSprite("Move Forward");
            _moveBackwardCycle = characterSheet.CreateAnimatedSprite("Move Backward");
            _moveRightCycle = characterSheet.CreateAnimatedSprite("Move Right");
            #endregion

            #region Standing animations
            _standForward = characterSheet.CreateAnimatedSprite("Stand Forward");
            _standBackward = characterSheet.CreateAnimatedSprite("Stand Backward");
            _standRight = characterSheet.CreateAnimatedSprite("Stand Right");
            #endregion

            #region Attack animations
            _attackForward = attackSheet.CreateAnimatedSprite("Forward Attack");
            _attackBackward = attackSheet.CreateAnimatedSprite("Backward Attack");
            _attackRight = attackSheet.CreateAnimatedSprite("Right Attack");
            #endregion

            _currentAnimation = _moveForwardCycle;
            _currentAttackAnimation = null;
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
                HandleAttack(gameTime, keyboardState);
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
            if (!_world.IsCollidingWithTile(playerHitbox)) {
                _position.X = newPosition.X;
            }

            newPosition = Position + new Vector2(0, movement.Y);
            playerHitbox = GetHitbox(newPosition);
            if (!_world.IsCollidingWithTile(playerHitbox)) {
                _position.Y = newPosition.Y;
            }

            if (isMoving) {
                if (!_world.IsCollidingWithTile(playerHitbox))
                {
                    _position = newPosition;
                }
                _currentAnimation.Play();
                _currentAnimation.Update(gameTime);
            }
            else {
                if (_currentAnimation == _moveForwardCycle) {
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

        private void HandleAttack(GameTime gameTime, KeyboardState keyboardState)
        {
            if (_currentAttackAnimation != null)
            {
                _currentAttackAnimation.Update(gameTime);

                if (!_currentAttackAnimation.IsAnimating)
                {
                    _currentAttackAnimation = null;
                }
                return; 
            }

            if (keyboardState.IsKeyDown(Keys.T))
            {
                if (_currentAnimation == _moveForwardCycle || _currentAnimation == _standForward)
                {
                    _currentAttackAnimation = _attackForward;
                }
                else if (_currentAnimation == _moveBackwardCycle || _currentAnimation == _standBackward)
                {
                    _currentAttackAnimation = _attackBackward;
                }
                else if (_currentAnimation == _moveRightCycle || _currentAnimation == _standRight)
                {
                    _currentAttackAnimation = _attackRight;
                    _currentAttackAnimation.FlipHorizontally = _currentAnimation.FlipHorizontally;
                }
                _currentAttackAnimation.Play(1);
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
            if (_currentAttackAnimation != null) {

                Vector2 drawPosition = _position;
                Vector2 attackOffset = Vector2.Zero;

                if (_currentAttackAnimation == _attackRight && !_currentAttackAnimation.FlipHorizontally)
                {
                    attackOffset.X = -16;
                }
                else if (_currentAttackAnimation == _attackRight && _currentAttackAnimation.FlipHorizontally)
                {
                    attackOffset.X = -16;
                }
                else if (_currentAttackAnimation == _attackForward)
                {
                    attackOffset.X = -16; 
                }
                else if (_currentAttackAnimation == _attackBackward) {
                    attackOffset.X = -24; 
                    attackOffset.Y = -8;
                }

                drawPosition += attackOffset;
                spriteBatch.Draw(_currentAttackAnimation, drawPosition);

            }

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