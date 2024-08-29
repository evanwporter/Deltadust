using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using MonoGame.Aseprite;

namespace MyGame {
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private EntityManager _entityManager;
        private List<System> _systems;

        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private Camera _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _entityManager = new EntityManager();
            _systems = new List<System>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the Tiled map
            _tiledMap = Content.Load<TiledMap>("Map/starter_island");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            // Initialize the camera
            _camera = new Camera(GraphicsDevice.Viewport) 
            {
                Zoom = 1.0f
            };

            // Load the Slime sprite from Aseprite
            AsepriteFile aseFile = Content.Load<AsepriteFile>("Monsters/Slime");
            SpriteSheet slimeSpriteSheet = aseFile.CreateSpriteSheet(GraphicsDevice);
            AnimatedSprite slimeSprite = slimeSpriteSheet.CreateAnimatedSprite("Jump");

            // Create Slime entities
            var slime = _entityManager.CreateEntity();
            _entityManager.AddComponent(slime, new TransformComponent(new Vector2(100, 100)));
            _entityManager.AddComponent(slime, new RenderComponent(slimeSprite));

            // Add systems
            _systems.Add(new MovementSystem(_entityManager));
            _systems.Add(new RenderSystem(_spriteBatch, _entityManager));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Logic to update the game, including movement and game state, but NOT rendering
            foreach (var system in _systems)
            {
                // Call non-rendering systems here, like MovementSystem
                if (!(system is RenderSystem))
                {
                    system.Update(gameTime, _entityManager.GetEntitiesWithComponents(typeof(TransformComponent), typeof(RenderComponent)));
                }
            }

            // Update the camera to follow the player or any target
            var firstEntity = _entityManager.GetEntitiesWithComponent<TransformComponent>().FirstOrDefault();
            if (firstEntity != null)
            {
                _camera.Position = _entityManager.GetComponent<TransformComponent>(firstEntity).Position;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Apply the camera transformation
            Matrix viewMatrix = _camera.GetViewMatrix();

            // Begin the sprite batch with the camera view matrix
            _spriteBatch.Begin(transformMatrix: viewMatrix);

            // Draw the Tiled map
            _tiledMapRenderer.Draw(viewMatrix);

            // Call the render system to draw all entities (this includes any system that deals with drawing)
            foreach (var system in _systems)
            {
                if (system is RenderSystem)
                {
                    system.Update(gameTime, _entityManager.GetEntitiesWithComponents(typeof(TransformComponent), typeof(RenderComponent)));
                }
            }

            // End the sprite batch after all drawing is done
            _spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
