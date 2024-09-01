// // using Microsoft.Xna.Framework;
// // using Microsoft.Xna.Framework.Graphics;
// // using MonoGame.Aseprite;
// // using AsepriteDotNet;

// // namespace MyGame {
// //     public class AnimatedSprite {
// //         private Vector2 _position;
// //         private AnimatedSprite _moveUpAnimation;
// //         private AnimatedSprite _moveDownAnimation;
// //         private AnimatedSprite _moveLeftAnimation;
// //         private AnimatedSprite _moveRightAnimation;
// //         private AnimatedSprite _standUpAnimation;
// //         private AnimatedSprite _standDownAnimation;
// //         private AnimatedSprite _standLeftAnimation;
// //         private AnimatedSprite _standRightAnimation;

// //         private AnimatedSprite _currentAnimation;

// //         public AnimatedSprite(Vector2 startPosition) {
// //             _position = startPosition;
// //         }

// //         public void LoadContent(AsepriteFile aseFile, GraphicsDevice graphicsDevice) {
// //             SpriteSheet spriteSheet = aseFile.CreateSpriteSheet(graphicsDevice);

// //             // Load movement animations
// //             _moveUpAnimation = spriteSheet.CreateAnimatedSprite("Move Up");
// //             _moveDownAnimation = spriteSheet.CreateAnimatedSprite("Move Down");
// //             _moveLeftAnimation = spriteSheet.CreateAnimatedSprite("Move Left");
// //             _moveRightAnimation = spriteSheet.CreateAnimatedSprite("Move Right");

// //             // Load standing animations
// //             _standUpAnimation = spriteSheet.CreateAnimatedSprite("Stand Up");
// //             _standDownAnimation = spriteSheet.CreateAnimatedSprite("Stand Down");
// //             _standLeftAnimation = spriteSheet.CreateAnimatedSprite("Stand Left");
// //             _standRightAnimation = spriteSheet.CreateAnimatedSprite("Stand Right");

// //             _currentAnimation = _standDownAnimation;  // Set initial animation
// //         }

// //         public void MoveUp() {
// //             _currentAnimation = _moveUpAnimation;
// //             _currentAnimation.Play();
// //         }

// //         public void MoveDown() {
// //             _currentAnimation = _moveDownAnimation;
// //             _currentAnimation.Play();
// //         }

// //         public void MoveLeft() {
// //             _currentAnimation = _moveLeftAnimation;
// //             _currentAnimation.Play();
// //         }

// //         public void MoveRight() {
// //             _currentAnimation = _moveRightAnimation;
// //             _currentAnimation.Play();
// //         }

// //         public void StandUp() {
// //             _currentAnimation = _standUpAnimation;
// //             _currentAnimation.Play();
// //         }

// //         public void StandDown() {
// //             _currentAnimation = _standDownAnimation;
// //             _currentAnimation.Play();
// //         }

// //         public void StandLeft() {
// //             _currentAnimation = _standLeftAnimation;
// //             _currentAnimation.Play();
// //         }

// //         public void StandRight() {
// //             _currentAnimation = _standRightAnimation;
// //             _currentAnimation.Play();
// //         }

// //         public void Update(GameTime gameTime) {
// //             _currentAnimation.Update(gameTime);
// //         }

// //         public void Draw(SpriteBatch spriteBatch) {
// //             spriteBatch.Draw(_currentAnimation, _position);
// //         }

// //         public void SetPosition(Vector2 position) {
// //             _position = position;
// //         }

// //         public Vector2 GetPosition() {
// //             return _position;
// //         }
// //     }
// // }


// public static class ResourceManager {
//     private static Dictionary<string, SpriteSheet> _spriteSheets = new Dictionary<string, SpriteSheet>();

//     public static SpriteSheet GetSpriteSheet(string entityName, GraphicsDevice graphicsDevice) {
//         if (!_spriteSheets.ContainsKey(entityName)) {
//             LoadSpriteSheet(entityName, graphicsDevice);
//         }
//         return _spriteSheets[entityName];
//     }

//     private static void LoadSpriteSheet(string entityName, GraphicsDevice graphicsDevice) {
//         AsepriteFile aseFile = Content.Load<AsepriteFile>($"Content/{entityName}/{entityName}");
//         _spriteSheets[entityName] = aseFile.CreateSpriteSheet(graphicsDevice);
//     }
// }

// public class AnimatedSprite {
//     private AnimatedSprite _currentAnimation;
//     private Vector2 _position;

//     public AnimatedSprite(Vector2 startPosition, string entityName, GraphicsDevice graphicsDevice) {
//         _position = startPosition;

//         // Get shared sprite sheet from ResourceManager
//         SpriteSheet sharedSpriteSheet = ResourceManager.GetSpriteSheet(entityName, graphicsDevice);

//         // Set default animation
//         _currentAnimation = sharedSpriteSheet.CreateAnimatedSprite("Stand Down");
//     }

//     public void MoveUp() {
//         _currentAnimation = _sharedSpriteSheet.CreateAnimatedSprite("Move Up");
//         _currentAnimation.Play();
//     }

//     // Other movement methods would similarly change _currentAnimation

//     public void Update(GameTime gameTime) {
//         _currentAnimation.Update(gameTime);
//     }

//     public void Draw(SpriteBatch spriteBatch) {
//         spriteBatch.Draw(_currentAnimation, _position);
//     }

//     public void SetPosition(Vector2 position) {
//         _position = position;
//     }

//     public Vector2 GetPosition() {
//         return _position;
//     }
// }
