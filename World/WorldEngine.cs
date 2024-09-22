using Deltadust.Core;
using Deltadust.Entities;
using Deltadust.Entities.Animated.Actor;
using Deltadust.Events;
using Deltadust.Quests;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

using System.Collections.Generic;
using System.IO;

namespace Deltadust.World {
    public class WorldEngine {
        private MapEngine _map;
        private Player _player;
        private EventManager _eventManager;
        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphicsDevice;
        private Camera _camera;
        private QuestManager _questManager;
        private float _debugTimer = 0f;
        private SpatialHashGrid _spatialHashGrid;
        private int _gridSize = 64;


        public WorldEngine(GraphicsDevice graphicsDevice, ContentManager content, EventManager eventManager) {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _eventManager = eventManager;
        }

        public void Initialize() {
            _camera = new Camera(_graphicsDevice.Viewport);
            ResourceManager.Initialize(_content, _graphicsDevice);
            _questManager = new QuestManager();
            _spatialHashGrid = new SpatialHashGrid(_gridSize);
        }

        public void LoadContent() {
            TiledMap tiledMap = _content.Load<TiledMap>("Maps/starter_island");
            _map = new MapEngine(tiledMap, _graphicsDevice, _eventManager);

            _player = new Player(
                new Vector2(300, 300), 
                Path.Combine(_content.RootDirectory, "inventory.xml")
            );
            _player.LoadContent();

            _map.AddNPCs(_eventManager);

        }

        public void Update(GameTime gameTime) {
            _player.Update(gameTime);

            _map.Update(gameTime);

            var warpPoint = _map.CheckForWarp(_player.GetHitbox(_player.Position));
            if (warpPoint != null) {
                WarpToMap(warpPoint.MapName, warpPoint.TargetPosition);
            }

            _camera.Position = _player.Position - new Vector2(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2) / _camera.Zoom;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font) {
            Matrix viewMatrix = _camera.GetViewMatrix();

            spriteBatch.Begin(
                transformMatrix: viewMatrix, 
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                sortMode: SpriteSortMode.Immediate
            );

            _map.Draw(spriteBatch, viewMatrix);

            var entities = new List<IDrawable>(_map.NPCs.Count + 1);
            entities.AddRange(_map.NPCs);
            entities.Add(_player);

            #if DEBUG
            if (_map.StaticEntities == null)
            {
                System.Diagnostics.Debug.WriteLine("_map.StaticEntities is null");
            }
            #endif

            entities.AddRange(_map.StaticEntities);
            

            entities.Sort((a, b) => a.Y.CompareTo(b.Y));

            #if DEBUG
            _debugTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_debugTimer >= 5f)
            {
                foreach (var entity in entities)
                {
                    System.Diagnostics.Debug.WriteLine($"Entity: {entity.GetType().Name}, Y: {entity.Y}");
                }
                _debugTimer = 0f;
            }
            #endif

            foreach (var entity in entities) {
                entity.Draw(spriteBatch, viewMatrix);
            }

            spriteBatch.End();
        }

        public void WarpToMap(string mapName, Vector2 newPlayerPosition)
        {
            TiledMap newMap = _content.Load<TiledMap>(mapName);

            _map = new MapEngine(newMap, _graphicsDevice, _eventManager);

            _player.SetPosition(newPlayerPosition);
            _camera.Position = _player.Position - new Vector2(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2) / _camera.Zoom;

            System.Diagnostics.Debug.WriteLine($"Warping to {mapName} at position {newPlayerPosition}");
        }

        public Player GetPlayer() => _player;

        internal MapEngine GetCurrentMap()
        {
            return _map;
        }
    }
}
