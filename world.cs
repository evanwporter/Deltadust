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

        private List<WarpPoint> _warpPoints;


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

            #if !DEBUG
            _collisionLayer.IsVisible = false;
            #endif

            LoadWarpPoints();

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

        private void LoadWarpPoints()
        {
            _warpPoints = new List<WarpPoint>();

            var objectLayer = _tiledMap.GetLayer<TiledMapObjectLayer>("Warp");
            if (objectLayer != null)
            {
                foreach (var obj in objectLayer.Objects)
                {
                    var warpPoint = new WarpPoint
                    {
                        Name = obj.Name,
                        MapName = obj.Properties["MapName"]?.ToString(),
                        TargetPosition = new Vector2(
                            float.Parse(obj.Properties["TargetX"].ToString()),
                            float.Parse(obj.Properties["TargetY"].ToString())
                        ),
                        Bounds = new Rectangle(
                            (int)obj.Position.X,
                            (int)obj.Position.Y - 32,
                            (int)obj.Size.Width,
                            (int)obj.Size.Height
                        )
                    };
                    _warpPoints.Add(warpPoint);
                }
            }
        }

        public WarpPoint CheckForWarp(Rectangle playerHitbox)
        {
            foreach (var warpPoint in _warpPoints)
            {
                if (warpPoint.Bounds.Contains(playerHitbox))
                {
                    System.Diagnostics.Debug.WriteLine($"Player fully entered warp point: {warpPoint.Name}");
                    return warpPoint;
                }
            }
            return null;
        }

    }
}