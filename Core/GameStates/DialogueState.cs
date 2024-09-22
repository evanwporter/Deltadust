using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Deltadust.Menu;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Deltadust.Core.Input;
using Deltadust.DialogueContainer;

namespace Deltadust.Core.GameStates
{
    public class DialogueState : GameState
    {
        private Dialogue _dialogueGUI;
        private Texture2D _dialogueBoxTexture;
        private DialogueTree _dialogueTree;
        private DialogueNode _currentNode;
        private bool _dialogueLoadedSuccessfully;

        public DialogueState(string dialogueFilePath) : base()
        {
            _dialogueGUI = new Dialogue();
            _dialogueLoadedSuccessfully = LoadDialogueTree(dialogueFilePath); // Attempt to load dialogue tree
        }

        private bool LoadDialogueTree(string dialogueFilePath)
        {
            try
            {
                // Check if the file exists
                if (!File.Exists(dialogueFilePath))
                {
                    Debug.WriteLine($"[Warning] Dialogue file '{dialogueFilePath}' not found.");
                    return false;
                }

                string json = File.ReadAllText(dialogueFilePath);
                _dialogueTree = JsonConvert.DeserializeObject<DialogueTree>(json);

                if (_dialogueTree == null || _dialogueTree.Nodes == null || _dialogueTree.Nodes.Count == 0)
                {
                    Debug.WriteLine($"[Warning] Dialogue file '{dialogueFilePath}' is empty or incorrectly formatted.");
                    return false;
                }

                _currentNode = _dialogueTree.Nodes[1]; // Start at the first node
                return true;
            }
            catch (JsonException jsonEx)
            {
                // Handle JSON parsing errors
                Debug.WriteLine($"[Error] Failed to parse dialogue file '{dialogueFilePath}': {jsonEx.Message}");
                return false;
            }
            catch (IOException ioEx)
            {
                // Handle general I/O errors
                Debug.WriteLine($"[Error] Failed to load dialogue file '{dialogueFilePath}': {ioEx.Message}");
                return false;
            }
        }

        public override void Initialize() { }

        public override void LoadContent()
        {
            _dialogueBoxTexture = new Texture2D(Game1.GetGraphicsDevice(), 1, 1);
            _dialogueBoxTexture.SetData(new[] { Color.White });

            if (_dialogueLoadedSuccessfully)
            {
                ShowCurrentNode(); // Show the first node's dialogue and options
            }
            else
            {
                _dialogueGUI.SetText("[Error] Unable to load dialogue. Please check the file and try again.");
            }
        }

        private void ShowCurrentNode()
        {
            if (_dialogueLoadedSuccessfully && _currentNode != null)
            {
                _dialogueGUI.SetText(_currentNode.Text);

                // Pass the dialogue options to the GUI
                var options = new List<string>();
                foreach (var option in _currentNode.Options)
                {
                    options.Add(option.OptionText);
                }
                _dialogueGUI.SetOptions(options);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!_dialogueLoadedSuccessfully)
            {
                // Handle case where dialogue was not loaded successfully
                if (InputManager.IsKeyPressed(Keys.Escape) || InputManager.IsKeyPressed(Keys.Enter))
                {
                    Game1.PopState(); // Exit dialogue if it's not loaded correctly
                }
                return;
            }

            bool moveUp = InputManager.IsKeyPressed(Keys.Up);
            bool moveDown = InputManager.IsKeyPressed(Keys.Down);
            bool select = InputManager.IsKeyPressed(Keys.Enter);

            _dialogueGUI.UpdateInput(moveUp, moveDown, select);

            if (select)
            {
                var selectedOption = _currentNode.Options[_dialogueGUI.GetSelectedOption()];
                if (selectedOption.NextNodeId == -1)
                {
                    Game1.PopState(); 
                    _currentNode = null;
                    return;
                }
                else
                {
                    _currentNode = _dialogueTree.Nodes[selectedOption.NextNodeId];
                    ShowCurrentNode(); 
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _dialogueGUI.Draw(spriteBatch, _dialogueBoxTexture);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            _dialogueBoxTexture.Dispose();
        }
    }
}
