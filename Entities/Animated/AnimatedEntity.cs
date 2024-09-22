using System;
using System.Collections.Generic;
using Deltadust.Entities.Animated.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Aseprite;
using Deltadust.Entities.Animated.Actor.States;

namespace Deltadust.Entities.Animated {
    public abstract class AnimatedEntity : IEntity
    {
        protected Vector2 _position;
        protected readonly float _speed;
        protected readonly int hitboxWidth = 20;

        protected Vector2 _facingVector;
        
        public Guid Id { get; private set; }
        private Dictionary<Type, Component> _components;
        protected AnimatedSprite _currentAnimation;

        public AnimatedEntity(Vector2 startPosition)
        {
            _position = startPosition;
            _facingVector = new Vector2(0, 1);
            _components = [];
        }

        public abstract void LoadContent();

        public virtual void UnloadContent() {}

        public virtual void Update(GameTime gameTime)
        {
            _currentAnimation.Play();
            _currentAnimation.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            spriteBatch.Draw(_currentAnimation, _position);
        }

        protected void DrawHitbox(SpriteBatch spriteBatch) {
            Texture2D hitboxTexture = new(spriteBatch.GraphicsDevice, 1, 1);
            hitboxTexture.SetData(new[] { Color.Red });

            spriteBatch.Draw(hitboxTexture, GetHitbox(_position), Color.Red * 0.5f);
        }

        public virtual Rectangle GetHitbox(Vector2 position)
        {
            return new Rectangle(
                (int)(position.X + ((32 - hitboxWidth) / 2)), // Centers it on X axis
                (int)(position.Y + 32 + 32 - 20), // Move down two tiles then up 20 pixels
                hitboxWidth,
                20
            );
        }

        public Vector2 Position => _position;
        public float Y => _position.Y;
        public void SetPosition(Vector2 newPosition) => _position = newPosition;

        public void SetFacingVector(Vector2 facingVector) {
            if (Math.Abs(facingVector.X) > 0 && Math.Abs(facingVector.Y) > 0) {
                System.Diagnostics.Debug.WriteLine("Error tried to set the facing vector in two different directions");
                facingVector.X = 0;
            }
            _facingVector = facingVector;
        }
        public Vector2 GetFacingVector() => _facingVector;


        public void AddComponent<T>(T component) where T : Component
        {
            _components[typeof(T)] = component;
        }

        public T GetComponent<T>() where T : Component
        {
            if (_components.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            return null;
        }

        public bool HasComponent<T>() where T : Component
        {
            return _components.ContainsKey(typeof(T));
        }

        public void RemoveComponent<T>() where T : Component
        {
            _components.Remove(typeof(T));
        }

        internal void SwitchToAttackAnimation()
        {
            throw new NotImplementedException();
        }

        internal virtual bool IsAttacking()
        {
            throw new NotImplementedException();
        }

        internal virtual void SetState(IdleState idleState)
        {
            throw new NotImplementedException();
        }

        public AnimatedSprite GetCurrentAnimation() => _currentAnimation;

        public void SetCurrentAnimation(AnimatedSprite currentAnimation) => _currentAnimation = currentAnimation;

    }
}
