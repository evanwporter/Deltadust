using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace MyGame {
    public class World
    {
        private readonly TiledMap _tiledMap;
        private readonly TiledMapRenderer _tiledMapRenderer;
        private readonly TiledMapTileLayer _collisionLayer;

        public World(TiledMap tiledMap, GraphicsDevice graphicsDevice)
        {
            _tiledMap = tiledMap;
            _tiledMapRenderer = new TiledMapRenderer(graphicsDevice, _tiledMap);

            // Assume the collision layer is named "Collisions"
            _collisionLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Collisions");

            #if DEBUG
            if (_collisionLayer == null) {
                System.Diagnostics.Debug.WriteLine("Collision tile layer not found in the tiled map!");
            }
            #endif

            // _collisionLayer.IsVisible = false;

        }
        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            _tiledMapRenderer.Draw(viewMatrix);
        }

        public bool IsCollidingWithTile(Rectangle entityRectangle)
        {
            int tileWidth = _tiledMap.TileWidth;
            int tileHeight = _tiledMap.TileHeight;

            // Check the four corners of the entity's hitbox
            var corners = new List<Vector2> {
                new(entityRectangle.Left, entityRectangle.Top),      // Top-left
                new(entityRectangle.Right, entityRectangle.Top),     // Top-right
                new(entityRectangle.Left, entityRectangle.Bottom),   // Bottom-left
                new(entityRectangle.Right, entityRectangle.Bottom)   // Bottom-right
            };

            foreach (var corner in corners)
            {
                ushort tileX = (ushort)(corner.X / tileWidth);
                ushort tileY = (ushort)(corner.Y / tileHeight);

                if (tileX >= 0 && tileX < _collisionLayer.Width && tileY >= 0 && tileY < _collisionLayer.Height)
                {
                    var tile = _collisionLayer.GetTile(tileX, tileY);
                    if (tile.GlobalIdentifier != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}