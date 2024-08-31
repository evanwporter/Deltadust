#define DEBUG


using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Tiled;
using AsepriteDotNet.Aseprite;


namespace MyGame {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Camera _camera;
        private SpriteFont _font;

        private Player _player;
        private Slime _slimeMonster;
        private NPC _npc;


        private World _world;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            _camera = new Camera(GraphicsDevice.Viewport);

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Fonts/ArialFont");

            TiledMap tiledMap = Content.Load<TiledMap>("Maps/starter_island");
            _world = new World(tiledMap, GraphicsDevice);

            _player = new Player(
                new Vector2(300, 300), 
                100f, 
                Path.Combine(Content.RootDirectory, "inventory.xml"),
                _world
            );

            AsepriteFile aseFile = Content.Load<AsepriteFile>("Character/character");
            _player.LoadContent(aseFile, GraphicsDevice);

            _slimeMonster = new Slime(new Vector2(200, 200), 100f, _world);
            AsepriteFile slimeAseFile = Content.Load<AsepriteFile>("Monsters/slime");
            _slimeMonster.LoadContent(slimeAseFile, GraphicsDevice);

            _npc = new NPC(new Vector2(400, 300), "Hello, traveler!", _world);
            AsepriteFile npcAseFile = Content.Load<AsepriteFile>("Character/Lewis_Beach");
            _npc.LoadContent(npcAseFile, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _npc.Update(gameTime);
            _player.Update(gameTime);
            _slimeMonster.Update(gameTime);

            var warpPoint = _world.CheckForWarp(_player.GetHitbox(_player.Position));
            if (warpPoint != null) {
                WarpToMap(warpPoint.MapName, warpPoint.TargetPosition);
            }

            // Update the camera position to follow the player
            _camera.Position = _player.Position - new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) / _camera.Zoom;

            _world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Apply camera transformation
            Matrix viewMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: viewMatrix, samplerState: SamplerState.PointClamp);

            _world.Draw(_spriteBatch, viewMatrix);

            _player.Draw(_spriteBatch, _font, viewMatrix);

            _npc.Draw(_spriteBatch, _font, viewMatrix);

            _slimeMonster.Draw(_spriteBatch, _font, viewMatrix);

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
