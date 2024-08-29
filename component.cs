using System;
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
        
    public abstract class Component
    {
        // Base class for all components if needed
    }

    public class TransformComponent : Component
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public TransformComponent(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
        }
    }

    public class RenderComponent : Component {
        public AnimatedSprite Sprite { get; set; }

        public RenderComponent(AnimatedSprite sprite) {
            Sprite = sprite;
        }
    }

    public class HealthComponent : Component
    {
        public int Health { get; set; }

        public HealthComponent(int health)
        {
            Health = health;
        }
    }
}