using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace MyGame {
    public class World {
        private readonly TiledMap _tiledMap;
        private readonly TiledMapRenderer _tiledMapRenderer;
        private readonly TiledMapTileLayer _collisionLayer;
        private Dictionary<Point, List<WarpPoint>> _warpPointsDictionary;

        public World(TiledMap tiledMap, GraphicsDevice graphicsDevice) {
            _tiledMap = tiledMap;
            _tiledMapRenderer = new TiledMapRenderer(graphicsDevice, _tiledMap);

            // Collision layer is named "Collisions"; need to change to something more general like Barriers
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
        public void Update(GameTime gameTime) {
            _tiledMapRenderer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Matrix viewMatrix) {
            _tiledMapRenderer.Draw(viewMatrix);
        }

        public bool IsCollidingWithTile(Rectangle entityRectangle) {
            int tileWidth = _tiledMap.TileWidth;
            int tileHeight = _tiledMap.TileHeight;

            var corners = new List<Vector2> {
                new(entityRectangle.Left, entityRectangle.Top),      
                new(entityRectangle.Right, entityRectangle.Top), 
                new(entityRectangle.Left, entityRectangle.Bottom),  
                new(entityRectangle.Right, entityRectangle.Bottom) 
            };

            foreach (var corner in corners) {
                ushort tileX = (ushort)(corner.X / tileWidth);
                ushort tileY = (ushort)(corner.Y / tileHeight);

                if (tileX >= 0 && tileX < _collisionLayer.Width && tileY >= 0 && tileY < _collisionLayer.Height) {
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
            _warpPointsDictionary = new Dictionary<Point, List<WarpPoint>>();

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

                    // Determine the top-left corner of the tile the warp point is in
                    var tileX = warpPoint.Bounds.X / _tiledMap.TileWidth;
                    var tileY = warpPoint.Bounds.Y / _tiledMap.TileHeight;
                    var tilePosition = new Point(tileX, tileY);

                    if (!_warpPointsDictionary.ContainsKey(tilePosition))
                    {
                        _warpPointsDictionary[tilePosition] = new List<WarpPoint>();
                    }
                    _warpPointsDictionary[tilePosition].Add(warpPoint);
                }
            }
        }

        public WarpPoint CheckForWarp(Rectangle playerHitbox)
        {
            // Determine the top-left corner of the tile the player's hitbox is in
            var tileX = playerHitbox.Left / _tiledMap.TileWidth;
            var tileY = playerHitbox.Top / _tiledMap.TileHeight;
            var playerTilePosition = new Point(tileX, tileY);

            // Check if there is any warp point in the player's current tile
            if (_warpPointsDictionary.TryGetValue(playerTilePosition, out var warpPoints))
            {
                foreach (var warpPoint in warpPoints)
                {
                    if (warpPoint.Bounds.Contains(playerHitbox))
                    {
                        return warpPoint;
                    }
                }
            }
            
            return null;
        }


    }
}