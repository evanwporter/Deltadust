// #define DEBUG


using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Tiled;
using AsepriteDotNet.Aseprite;
using System.Collections.Generic;


namespace MyGame {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Camera _camera;
        private SpriteFont _font;

        private Player _player;
        private List<NPC> _npcs;
        private List<Slime> _monsters;

        private ResourceManager _resourceManager;


        private World _world;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            _camera = new Camera(GraphicsDevice.Viewport);
            _resourceManager = new ResourceManager(Content, GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent() {

            _npcs = [];
            _monsters = [];

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Fonts/ArialFont");

            TiledMap tiledMap = Content.Load<TiledMap>("Maps/starter_island");
            _world = new World(tiledMap, GraphicsDevice);

            _player = new Player(
                new Vector2(300, 300), 
                Path.Combine(Content.RootDirectory, "inventory.xml"),
                _world,
                _resourceManager
            );

            _player.LoadContent();

            Slime _slimeMonster = new Slime(new Vector2(200, 200), _world, _resourceManager);
            _slimeMonster.LoadContent();

            Friendly _npc = new Friendly(new Vector2(400, 300), "Hello, traveler!", _world, _resourceManager);
            _npc.LoadContent();

            _npcs.Add(_npc);
            _monsters.Add(_slimeMonster);

        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);

            if (!IsPaused()) {
                foreach (var npc in _npcs) {
                    npc.Update(gameTime);
                }

                foreach (var monster in _monsters) {
                    monster.Update(gameTime);
                }
            }

            var warpPoint = _world.CheckForWarp(_player.GetHitbox(_player.Position));
            if (warpPoint != null) {
                WarpToMap(warpPoint.MapName, warpPoint.TargetPosition);
            }

            // Update the camera position to follow the player
            _camera.Position = _player.Position - new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) / _camera.Zoom;

            _world.Update(gameTime);

            base.Update(gameTime);
        }

        public bool IsPaused() {
            if (_player.InventoryOpen) {
                return true;
            }
            return false;
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Apply camera transformation
            Matrix viewMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: viewMatrix, samplerState: SamplerState.PointClamp);

            _world.Draw(_spriteBatch, viewMatrix);

            _player.Draw(_spriteBatch, _font, viewMatrix);

            foreach (var npc in _npcs) {
                npc.Draw(_spriteBatch, _font, viewMatrix);
            }

            foreach (var monster in _monsters) {
                monster.Draw(_spriteBatch, _font, viewMatrix);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void WarpToMap(string mapName, Vector2 newPlayerPosition)
        {
            TiledMap newMap = Content.Load<TiledMap>(mapName);

            _world = new World(newMap, GraphicsDevice);

            _player.SetPosition(newPlayerPosition);
            _camera.Position = _player.Position - new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) / _camera.Zoom;

            System.Diagnostics.Debug.WriteLine($"Warping to {mapName} at position {newPlayerPosition}");
        }
    }
}
