using Deltadust.Core;
using Deltadust.Events;
using Deltadust.World;
using Deltadust.ItemManagement;
using Deltadust.Entities.Animated.Actor.States;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Aseprite;
using Deltadust.Core.Input;
using Deltadust.Entities.Animated.Components;
using System;


namespace Deltadust.Entities.Animated.Actor {
    public class Player : AnimatedEntity {
        // private new readonly float _speed = 100f;
        // private AnimatedSprite _moveForwardCycle;
        // private AnimatedSprite _moveBackwardCycle;
        // private AnimatedSprite _moveRightCycle;

        private AnimatedSprite _standForward;
        private AnimatedSprite _standBackward;
        private AnimatedSprite _standRight;

        private AnimatedSprite _attackForward;
        private AnimatedSprite _attackBackward;
        private AnimatedSprite _attackRight;

        private AnimatedSprite _currentAttackAnimation;

        private readonly Inventory _inventory;
        private const float InteractionDistance = 32f;


        public Player(Vector2 startPosition, string inventoryFilePath)
            : base(startPosition)
        {
            _inventory = Inventory.LoadFromFile(inventoryFilePath);
            // _currentState = new IdleState(this);

            AddComponent<StateMachineComponent>(new StateMachineComponent(this));
            AddComponent<MovementComponent>(new MovementComponent(this, 100f));
            AddComponent<IdleComponent>(new IdleComponent(this));
        }

        public override void LoadContent() {
            SpriteSheet characterSheet = ResourceManager.LoadSprite("Character/character");
            SpriteSheet attackSheet = ResourceManager.LoadSprite("Character/swipe");

            GetComponent<MovementComponent>().LoadContent("Character/character");
            GetComponent<IdleComponent>().LoadContent("Character/character");

            #region Standing animations
            _standForward = characterSheet.CreateAnimatedSprite("Stand Down");
            _standBackward = characterSheet.CreateAnimatedSprite("Stand Up");
            _standRight = characterSheet.CreateAnimatedSprite("Stand Right");
            #endregion

            #region Attack animations
            _attackForward = attackSheet.CreateAnimatedSprite("Forward Attack");
            _attackBackward = attackSheet.CreateAnimatedSprite("Backward Attack");
            _attackRight = attackSheet.CreateAnimatedSprite("Right Attack");
            #endregion

            // _currentAnimation = _moveForwardCycle;
            _currentAttackAnimation = null;

            _inventory.SetFont(ResourceManager.LoadFont("Fonts/ArialFont"));
        }

        public override void UnloadContent()
        {
        }

        #region Update
        public override void Update(GameTime gameTime) {

            GetComponent<StateMachineComponent>().Update(gameTime);
            _currentAnimation.Update(gameTime);
            // var keyboardState = Keyboard.GetState();

            // HandleMovement(gameTime, keyboardState);
            // HandleAttack(gameTime, keyboardState);

            if (InputManager.IsKeyPressed(Keys.E)) {
                CheckForInteraction();
            }
        }

        public new bool IsAttacking() {
            return _currentAttackAnimation != null && _currentAttackAnimation.IsAnimating;
        }

        // public new void SwitchToAttackAnimation() {
        //     if (_currentAnimation == _moveForwardCycle || _currentAnimation == _standForward) {
        //         _currentAttackAnimation = _attackForward;  
        //     } 
        //     else if (_currentAnimation == _moveBackwardCycle || _currentAnimation == _standBackward) {
        //         _currentAttackAnimation = _attackBackward;  
        //     } 
        //     else if (_currentAnimation == _moveRightCycle || _currentAnimation == _standRight) {
        //         _currentAttackAnimation = _attackRight; 
        //         _currentAttackAnimation.FlipHorizontally = false; 
        //     } 
        //     else if (_currentAnimation.FlipHorizontally) {
        //         _currentAttackAnimation = _attackRight; 
        //         _currentAttackAnimation.FlipHorizontally = true;
        //     }

        //     _currentAttackAnimation.Play(1);
        // }


        // private void HandleAttack(GameTime gameTime, KeyboardState keyboardState)
        // {
        //     if (_currentAttackAnimation != null)
        //     {
        //         _currentAttackAnimation.Update(gameTime);

        //         if (!_currentAttackAnimation.IsAnimating)
        //         {
        //             _currentAttackAnimation = null;
        //         }
        //         return; 
        //     }

        //     if (keyboardState.IsKeyDown(Keys.T))
        //     {
        //         if (_currentAnimation == _moveForwardCycle || _currentAnimation == _standForward)
        //         {
        //             _currentAttackAnimation = _attackForward;
        //         }
        //         else if (_currentAnimation == _moveBackwardCycle || _currentAnimation == _standBackward)
        //         {
        //             _currentAttackAnimation = _attackBackward;
        //         }
        //         else if (_currentAnimation == _moveRightCycle || _currentAnimation == _standRight)
        //         {
        //             _currentAttackAnimation = _attackRight;
        //             _currentAttackAnimation.FlipHorizontally = _currentAnimation.FlipHorizontally;
        //         }
        //         _currentAttackAnimation.Play(1);
        //     }
        // }
        #endregion
        
        // public Rectangle GetHitbox(Vector2 position)
        // {
        //     return new Rectangle(
        //         (int)(position.X + ((32 - hitboxWidth) / 2)),
        //         (int)(position.Y + 32 + 32 - 20),
        //         hitboxWidth,
        //         20
        //     );
        // }

        public override void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
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

            // #if DEBUG
            // DrawHitbox(spriteBatch);
            // #endif
        }

        public void CheckForInteraction() {
            Rectangle interactionRect = GetInteractionRectangle();

            foreach (var npc in Game1.GetWorld().GetCurrentMap().NPCs) {
                if (interactionRect.Intersects(npc.GetHitbox(npc.Position))) {
                    
                    // Check if player is facing the NPC
                    if (IsPlayerFacingEntity(npc.Position)) {
                        System.Diagnostics.Debug.WriteLine($"Interacting with NPC.");
                        npc.Interact();  // Trigger interaction only if player is facing NPC
                        break;
                    }
                }
            }
        }

        private bool IsPlayerFacingEntity(Vector2 entityPosition) {
            Vector2 directionToEntity = entityPosition - _position;

            // Check if the player is facing in the direction of the entity
            if (_facingVector == new Vector2(1, 0) && directionToEntity.X > 0 && Math.Abs(directionToEntity.X) > Math.Abs(directionToEntity.Y)) {
                return true;  // Player is facing right and entity is to the right
            } 
            else if (_facingVector == new Vector2(-1, 0) && directionToEntity.X < 0 && Math.Abs(directionToEntity.X) > Math.Abs(directionToEntity.Y)) {
                return true;  // Player is facing left and entity is to the left
            }
            else if (_facingVector == new Vector2(0, -1) && directionToEntity.Y < 0 && Math.Abs(directionToEntity.Y) > Math.Abs(directionToEntity.X)) {
                return true;  // Player is facing up and entity is above
            }
            else if (_facingVector == new Vector2(0, 1) && directionToEntity.Y > 0 && Math.Abs(directionToEntity.Y) > Math.Abs(directionToEntity.X)) {
                return true;  // Player is facing down and entity is below
            }

            // If none of these conditions are met, the player is not facing the entity
            return false;
        }


    private Rectangle GetInteractionRectangle() {

        Vector2 interactionPosition = new Vector2(_position.X, _position.Y + 32) + _facingVector * 32;

        return new Rectangle((int)interactionPosition.X, (int)interactionPosition.Y, 32, 32);
    }


        public Inventory GetInventory() => _inventory;

        public float Speed => _speed;
    }
}