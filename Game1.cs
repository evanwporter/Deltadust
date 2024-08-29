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

        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer _collisionLayer;

        private Camera _camera;
        private SpriteFont _font;

        private Player _player;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            _player = new Player(
                new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2),
                100f,
                Path.Combine(Content.RootDirectory, "inventory.xml")
            );

            _camera = new Camera(GraphicsDevice.Viewport);

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Fonts/ArialFont");

            // Load the Tiled map
            _tiledMap = Content.Load<TiledMap>("Map/starter_island");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            // Load player content
            AsepriteFile aseFile = Content.Load<AsepriteFile>("Character/character");
            _player.LoadContent(aseFile, GraphicsDevice);

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

            _player.Update(gameTime);

            // Update the camera position to follow the player
            _camera.Position = _player.Position - new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) / _camera.Zoom;

            _tiledMapRenderer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Apply camera transformation
            Matrix viewMatrix = _camera.GetViewMatrix();

            _tiledMapRenderer.Draw(viewMatrix);

            _spriteBatch.Begin(transformMatrix: viewMatrix, samplerState: SamplerState.PointClamp);

            _player.Draw(_spriteBatch, _font, viewMatrix);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
