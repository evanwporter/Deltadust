using System;
using System.Collections.Generic;
using Deltadust.Core;
using Deltadust.Entities.Animated.Actor;
using Deltadust.Entities.Animated.Components;
using Microsoft.Xna.Framework;

namespace Deltadust.Entities.Animated.Monsters.States
{
    public class MonsterIdleState : MonsterState
    {
        private readonly Player _player;

        public MonsterIdleState(AnimatedEntity monster, Player player) : base(monster) { 
            _player = player;
        }

        public override void Enter() {
            _entity.GetComponent<IdleComponent>().SwitchToIdleAnimation();
        }

        public override void Update(GameTime gameTime) {
            if (Vector2.Distance(_entity.Position, _player.Position) <= _entity.VisionRadius) {
                if (HasLineOfSightToPlayer()) {
                    _entity.GetComponent<StateMachineComponent>().SetState(new MonsterChasingState(_entity, _player));
                }
            }
        }

        public override void Exit() {
        }

        private bool HasLineOfSightToPlayer()
        {
            Vector2 start = _entity.Position;
            Vector2 end = _player.Position;

            int tileSize = Game1.GetWorld().GetCurrentMap().TileWidth;

            Point startTile = new Point((int)start.X / tileSize, (int)start.Y / tileSize);
            Point endTile = new Point((int)end.X / tileSize, (int)end.Y / tileSize);

            List<Point> lineOfSightTiles = BresenhamLine(startTile, endTile);

            foreach (Point tile in lineOfSightTiles)
            {
                if (IsBlockingTile(tile))
                {
                    return false;
                }
            }

            return true;
        }

        private List<Point> BresenhamLine(Point start, Point end)
        {
            List<Point> points = [];

            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int err = dx - dy;

            int x = start.X;
            int y = start.Y;

            while (true)
            {
                points.Add(new Point(x, y));

                if (x == end.X && y == end.Y)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }

            return points;
        }

        private bool IsBlockingTile(Point tilePosition)
        {
            ushort tileX = (ushort)tilePosition.X;
            ushort tileY = (ushort)tilePosition.Y;

            if (tileX >= 0 && tileX < Game1.GetWorld().GetCurrentMap().TileWidth && tileY >= 0 && tileY < Game1.GetWorld().GetCurrentMap().TileHeight)
            {
                var tile = Game1.GetWorld().GetCurrentMap().CollisionLayer.GetTile(tileX, tileY);
                return tile.GlobalIdentifier != 0;
            }

            return false;
        }

    }
}