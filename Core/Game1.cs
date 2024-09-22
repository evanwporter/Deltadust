// #define DEBUG
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Deltadust.Core.GameStates;
using Deltadust.Core.Input;
using Deltadust.Events;
using Deltadust.World;


namespace Deltadust.Core {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _font;
        // private WorldEngine _world;
        // private GameState _currentState;

        private LinkedList<GameState> _states;
        private readonly EventManager _eventManager;

        private static Game1 _instance;
        private WorldEngine _world;

        private Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            _eventManager = new EventManager();
        }

        public static Game1 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Game1();
                }
                return _instance;
            }
        }

        protected override void Initialize() {
            _states = new LinkedList<GameState>();
            InputManager.Initialize();

            PushState(new MainMenuState());

            _eventManager.OnDialogueTriggered += HandleDialogueTriggered;

            base.Initialize();
        }

        protected override void LoadContent() {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // _world.LoadContent();

            _font = Content.Load<SpriteFont>("Fonts/ArialFont");

        }

        protected override void Update(GameTime gameTime) {
            InputManager.Update();

            if (InputManager.IsKeyPressed(Keys.Escape))
            {
                Exit();
            }

            if (_states.Count > 0) {
                _states.Last.Value.Update(gameTime);
            }

            // _world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var state in _states) {
                state.Draw(gameTime, _spriteBatch);
            }

            // _world.Draw(gameTime, _spriteBatch, _font);

            base.Draw(gameTime);
        }

        public static void PushState(GameState state) {
            Instance._states.AddLast(state);
            state.Initialize();
            state.LoadContent();
        }

        public static void PopState() {
            if (Instance._states.Count > 0)
            {
                var state = Instance._states.Last.Value;
                state.UnloadContent();
                Instance._states.RemoveLast(); // Remove the last element (top of stack)
            }
        }

        public static void ChangeState(GameState state) {
            PopState();
            PushState(state);
        }

        private void HandleDialogueTriggered(object sender, DialogueEventArgs e)
        {
            PushState(new DialogueState(e.DialogueText));
        }

        public static EventManager GetEventManager() => Instance._eventManager;

        public static GraphicsDeviceManager GetGraphics() => Instance._graphics;

        public static SpriteFont Font => Instance._font;

        public static ContentManager GetContent() => Instance.Content;

        public static WorldEngine GetWorld() => Instance._world;

        public static GraphicsDevice GetGraphicsDevice() => Instance.GraphicsDevice;

        public static void SetWorld(WorldEngine world) => Instance._world = world;

        public static void ExitGame()
        {
            Game1.Instance.Exit();
        }
    }
}
