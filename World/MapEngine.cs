using System.Collections.Generic;
using Deltadust.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Deltadust.World {
    public class MapEngine {
        private readonly TiledMap _tiledMap;
        private readonly TiledMapRenderer _tiledMapRenderer;
        private readonly TiledMapTileLayer _collisionLayer;
        private readonly TiledMapTileLayer _groundLayer;
        private readonly List<LayerData> _sortedLayers;
        private Dictionary<Point, List<WarpPoint>> _warpPointsDictionary;
        public List<NPC> NPCs { get; private set; }

        public MapEngine(TiledMap tiledMap, GraphicsDevice graphicsDevice, List<NPC> npcs) {
            _tiledMap = tiledMap;
            _tiledMapRenderer = new TiledMapRenderer(graphicsDevice, _tiledMap);

            // Collision layer is named "Collisions"; need to change to something more general like Barriers
            _collisionLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Barriers");

            _sortedLayers = new List<LayerData>();
            _groundLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Ground");

            #if DEBUG
            if (_collisionLayer == null) {
                System.Diagnostics.Debug.WriteLine("Collision tile layer not found in the tiled map!");
            }
            #endif

            #if !DEBUG
            _collisionLayer.IsVisible = false;
            #endif

            LoadWarpPoints();

            // TODO Fully move these into MapEngine
            NPCs = npcs;

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

        public bool IsCollidingWithEntities(Rectangle playerHitbox) {
            foreach (var npc in NPCs) {
                if (playerHitbox.Intersects(npc.GetHitbox(npc.Position))) {
                    // Debugging output to check collisions with NPCs
                    System.Diagnostics.Debug.WriteLine($"Colliding with NPC: {npc.GetType().Name} at {npc.Position}");
                    return true;
                }
            }

            return false;
        }

        public bool IsColliding(Rectangle playerHitbox) {
            return IsCollidingWithEntities(playerHitbox) || IsCollidingWithTile(playerHitbox);
        }


        private void LoadWarpPoints()
        {
            _warpPointsDictionary = [];

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
                        _warpPointsDictionary[tilePosition] = [];
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

    public class LayerData {
        public TiledMapTileLayer Layer { get; }
        public float YHeight { get; }

        public LayerData(TiledMapTileLayer layer, float yHeight) {
            Layer = layer;
            YHeight = yHeight;
        }
    }

    // public class DrawableObject {
    //     private readonly TiledMapTileLayer _layer;
    //     private readonly NPC _npc;
    //     public float Y { get; }

    //     public DrawableObject(TiledMapTileLayer layer, float y) {
    //         _layer = layer;
    //         Y = y;
    //     }

    //     public DrawableObject(NPC npc, float y) {
    //         _npc = npc;
    //         Y = y;
    //     }

    //     public void Draw(SpriteBatch spriteBatch, Matrix viewMatrix) {
    //         if (_layer != null) {
    //             // Draw Tiled layer
    //             _layer.Draw(spriteBatch, viewMatrix);
    //         } else if (_npc != null) {
    //             // Draw NPC
    //             _npc.Draw(spriteBatch, _npc.Font, viewMatrix);
    //         }
    //     }
    // }

}