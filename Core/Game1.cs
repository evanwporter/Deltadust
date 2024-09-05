#define DEBUG

using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Tiled;
using System.Collections.Generic;


using Deltadust.World;
using Deltadust.Entities;
using Deltadust.Events;
using Deltadust.Quests;


namespace Deltadust.Core {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Camera _camera;
        private SpriteFont _font;

        private Player _player;
        private List<NPC> _npcs;

        private ResourceManager _resourceManager;

        private QuestManager _questManager;

        private WorldEngine _world;

        private CentralEventHandler _eventHandler;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            _camera = new Camera(GraphicsDevice.Viewport);
            _resourceManager = new ResourceManager(Content, GraphicsDevice);
            _questManager = new QuestManager();
            _eventHandler = new CentralEventHandler();

            base.Initialize();
        }

        protected override void LoadContent() {

            _npcs = [];

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Fonts/ArialFont");

            TiledMap tiledMap = Content.Load<TiledMap>("Maps/starter_island");
            _world = new WorldEngine(tiledMap, GraphicsDevice, _npcs);

            _player = new Player(
                new Vector2(300, 300), 
                Path.Combine(Content.RootDirectory, "inventory.xml"),
                _world,
                _resourceManager,
                _eventHandler
            );

            _player.LoadContent();

            Slime _slimeMonster = new Slime(new Vector2(200, 200), _world, _resourceManager, _eventHandler);
            _slimeMonster.LoadContent();

            Friendly _npc = new Friendly(new Vector2(400, 300), "Hello, traveler!", _world, _resourceManager, _eventHandler);
            _npc.LoadContent();

            _npcs.Add(_npc);
            _npcs.Add(_slimeMonster);

        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);

            if (!IsPaused()) {
                foreach (var npc in _npcs) {
                    npc.Update(gameTime);
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

            var entities = new List<Entity>(_npcs.Count + 1);
            entities.AddRange(_npcs);
            entities.Add(_player);

            entities.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));

            foreach (var entity in entities) {
                entity.Draw(_spriteBatch, _font, viewMatrix);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void WarpToMap(string mapName, Vector2 newPlayerPosition)
        {
            TiledMap newMap = Content.Load<TiledMap>(mapName);

            _world = new WorldEngine(newMap, GraphicsDevice, _npcs);

            _player.SetPosition(newPlayerPosition);
            _camera.Position = _player.Position - new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) / _camera.Zoom;

            System.Diagnostics.Debug.WriteLine($"Warping to {mapName} at position {newPlayerPosition}");
        }
    }
}
