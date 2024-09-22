using System;
using System.Collections.Generic;
using Deltadust.Entities;
using Microsoft.Xna.Framework;

public class SpatialHashGrid
{
    private readonly int _cellSize; // The size of each grid cell
    private readonly Dictionary<Point, List<IEntity>> _grid; // Stores entities in grid cells

    public SpatialHashGrid(int cellSize)
    {
        _cellSize = cellSize;
        _grid = [];
    }

    private Point GetCellPosition(Vector2 position)
    {
        int cellX = (int)Math.Floor(position.X / _cellSize);
        int cellY = (int)Math.Floor(position.Y / _cellSize);
        return new Point(cellX, cellY);
    }

    // Adds an entity to the grid based on its position
    public void Insert(IMovingEntity entity, Vector2 position)
    {
        var cellPosition = GetCellPosition(position);
        if (!_grid.ContainsKey(cellPosition))
        {
            _grid[cellPosition] = new List<IEntity>();
        }
        _grid[cellPosition].Add(entity);
    }

    public void Remove(IMovingEntity entity, Vector2 position)
    {
        var cellPosition = GetCellPosition(position);
        if (_grid.ContainsKey(cellPosition))
        {
            _grid[cellPosition].Remove(entity);
            if (_grid[cellPosition].Count == 0)
            {
                _grid.Remove(cellPosition);
            }
        }
    }

    // Updates an entity's position in the grid
    public void Update(IMovingEntity entity, GameTime gameTime)
    {
        Vector2 oldPosition = entity.Position;
        entity.Update(gameTime);

            if (oldPosition != entity.Position)
            {
            Remove(entity, oldPosition);
            Insert(entity, entity.Position);
        }

    }

    // Retrieves all entities in the same and neighboring grid cells
    public List<IEntity> QueryNearby(Vector2 position)
    {
        var nearbyEntities = new List<IEntity>();
        var cellPosition = GetCellPosition(position);

        // Search in the neighboring cells (3x3 grid of cells around the entity)
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                var neighborCell = new Point(cellPosition.X + x, cellPosition.Y + y);
                if (_grid.ContainsKey(neighborCell))
                {
                    nearbyEntities.AddRange(_grid[neighborCell]);
                }
            }
        }

        return nearbyEntities;
    }

    public void Clear()
    {
        _grid.Clear();
    }
}
