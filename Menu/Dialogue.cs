using Deltadust.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Deltadust.Menu
{
    public class Dialogue
    {
        private Rectangle _dialogueBox;
        private string _currentText;
        private List<string> _wrappedText;
        private const int Padding = 10;

        // New fields for options
        private List<string> _options;
        private int _selectedOptionIndex;

        public Dialogue()
        {
            _dialogueBox = new Rectangle(0, Game1.GetGraphicsDevice().Viewport.Height - 150, Game1.GetGraphicsDevice().Viewport.Width, 150);
            _wrappedText = [];

            _options = []; 
            _selectedOptionIndex = 0; 
        }

        public void SetText(string text)
        {
            _currentText = text;
            WrapText();
        }

        public void SetOptions(List<string> options)
        {
            _options = options;
            _selectedOptionIndex = 0; 
        }

        private void WrapText()
        {
            _wrappedText.Clear();

            string[] words = _currentText.Split(' ');
            string currentLine = string.Empty;

            foreach (var word in words)
            {
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                Vector2 textSize = Game1.Font.MeasureString(testLine);

                if (textSize.X > _dialogueBox.Width - Padding * 2) // If text exceeds the dialogue box width
                {
                    _wrappedText.Add(currentLine);
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                _wrappedText.Add(currentLine);
            }
        }

        // Handle input for navigating the options (up/down arrow keys)
        public void UpdateInput(bool moveUp, bool moveDown, bool select)
        {
            if (moveUp)
            {
                _selectedOptionIndex = (_selectedOptionIndex - 1 + _options.Count) % _options.Count;
            }
            else if (moveDown)
            {
                _selectedOptionIndex = (_selectedOptionIndex + 1) % _options.Count;
            }

            // The "select" bool would trigger the actual selection logic, but that's handled in the DialogueState
        }

        public int GetSelectedOption()
        {
            return _selectedOptionIndex; // Return the currently selected option index
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D rectangleTexture)
        {
            spriteBatch.Draw(rectangleTexture, _dialogueBox, Color.Black * 0.75f); // Semi-transparent black

            Vector2 position = new Vector2(_dialogueBox.X + Padding, _dialogueBox.Y + Padding);
            foreach (var line in _wrappedText)
            {
                spriteBatch.DrawString(Game1.Font, line, position, Color.White);
                position.Y += Game1.Font.LineSpacing; // Move to the next line
            }

            // Draw the options (below the dialogue text)
            if (_options.Count > 0)
            {
                position.Y += Game1.Font.LineSpacing; // Add some spacing before showing options
                for (int i = 0; i < _options.Count; i++)
                {
                    var color = (i == _selectedOptionIndex) ? Color.Yellow : Color.White; // Highlight selected option
                    spriteBatch.DrawString(Game1.Font, _options[i], position, color);
                    position.Y += Game1.Font.LineSpacing;
                }
            }
        }
    }
}
