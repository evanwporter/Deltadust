// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using MonoGame.Extended.Tiled;
// using MonoGame.Extended.Tiled.Renderers;

// namespace Deltadust.Entities.Static {
//     public class StaticEntity {
//         public float Y { get; protected set; }  // Y position for sorting
//         public virtual void Draw(SpriteBatch spriteBatch, Matrix viewMatrix) {
//             // Base draw method
//         }
//     }

//     public class TileLayerEntity : StaticEntity {
//         private readonly TiledMapTileLayer _layer;
//         private readonly TiledMapRenderer _renderer;

//         public TileLayerEntity(TiledMapTileLayer layer, float y, TiledMapRenderer renderer) {
//             _layer = layer;
//             Y = y;
//             _renderer = renderer;
//         }

//         public override void Draw(SpriteBatch spriteBatch, Matrix viewMatrix) {
//             _layer.Draw(spriteBatch, viewMatrix);  // Draw the tilemap layer
//         }
//     }
// }
