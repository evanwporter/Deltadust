// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using MonoGame.Aseprite;
// using AsepriteDotNet.Aseprite;

// namespace MyGame {
//     public class SpriteAnimation {
//         private Dictionary<string, AnimatedSprite> _animations;
//         private AnimatedSprite _currentAnimation;

//         public SpriteAnimation(string assetKey, GraphicsDevice graphicsDevice) {
//             LoadAnimations(assetKey, graphicsDevice);
//         }

//         private void LoadAnimations(string assetKey, GraphicsDevice graphicsDevice) {
//             _animations = new Dictionary<string, AnimatedSprite>();

//             if (AnimationAssets.AsepriteFiles.ContainsKey(assetKey)) {
//                 var aseFile = AnimationAssets.AsepriteFiles[assetKey];
//                 var spriteSheet = aseFile.CreateSpriteSheet(graphicsDevice);

//                 // Add animations based on tags in the aseprite file
//                 _animations["Stand Up"] = spriteSheet.CreateAnimatedSprite("Stand Up");
//                 _animations["Stand Down"] = spriteSheet.CreateAnimatedSprite("Stand Down");
//                 _animations["Stand Left"] = spriteSheet.CreateAnimatedSprite("Stand Left");
//                 _animations["Stand Right"] = spriteSheet.CreateAnimatedSprite("Stand Right");
//                 _animations["Move Up"] = spriteSheet.CreateAnimatedSprite("Move Up");
//                 _animations["Move Down"] = spriteSheet.CreateAnimatedSprite("Move Down");
//                 _animations["Move Left"] = spriteSheet.CreateAnimatedSprite("Move Left");
//                 _animations["Move Right"] = spriteSheet.CreateAnimatedSprite("Move Right");

//                 // Set a default animation (e.g., standing down)
//                 _currentAnimation = _animations["Stand Down"];
//             }
//         }

//         public void Play(string animationKey) {
//             if (_animations.ContainsKey(animationKey)) {
//                 _currentAnimation = _animations[animationKey];
//                 _currentAnimation.Play();
//             }
//         }

//         public void Update(GameTime gameTime) {
//             _currentAnimation?.Update(gameTime);
//         }

//         public void Draw(SpriteBatch spriteBatch, Vector2 position) {
//             if (_currentAnimation != null) {
//                 spriteBatch.Draw(_currentAnimation, position);
//             }
//         }
//     }
// }
