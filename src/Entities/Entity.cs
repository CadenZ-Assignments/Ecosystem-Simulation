using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.World.Entities;

public abstract class Entity
{
    // ReSharper disable once InconsistentNaming
    public readonly List<Gene> DNA;
    public readonly EntityInfo EntityInfo;
    protected readonly IPathFindingAgent<Tile> PathFinder;
    public TileCell Position = null!;

    protected Entity(EntityInfo entityInfo, IPathFindingAgent<Tile> pathFinder)
    {
        DNA = new List<Gene>();
        EntityInfo = entityInfo;
        PathFinder = pathFinder;
    }

    public void Destroy()
    {
        SimulationCore.Level.RemovalQueue.Enqueue(this);
    }

    public abstract void Render();

    public virtual void RefreshGoals()
    {
    }

    public virtual void Update()
    {
        if (Position.X is > Level.WorldWidth or < 0 || Position.Y is > Level.WorldHeight or < 0)
        {
            Destroy();
        }
    }

    protected List<TileCell> GoTo(ITileType type)
    {
        var tc = FindTile(type);
        var map = SimulationCore.Level.Map;
        
        if (tc is not null)
        {
            PathFinder.Init(map.GetTileAtCell(Position)!, map.GetTileAtCell(tc)!, map.Tiles);
            return PathFinder.FindPath();
        }

        return new List<TileCell>();
    }

    /// <summary>
    /// Find the closest tile of type in Entity's Sensor Range
    /// </summary>
    /// <param name="tileType">The tile type looking for.</param>
    /// <returns>The position of the closest tile, returns null if not found.</returns>
    protected TileCell? FindTile(ITileType tileType)
    {
        var range = EntityInfo.MaxSensorRange / 2;
        TileCell? bestSoFar = null;
        
        for (var x = -range; x < range; x++)
        {
            for (var y = -range; y < range; y++)
            {
                if (!SimulationCore.Level.Map.ExistInRange(Position.X + x, Position.Y + y)) continue;
                var tile = SimulationCore.Level.Map.GetTileAtCell(new TileCell(Position.X + x, Position.Y + y));
                
                if (tile is null || tile.Type != tileType) continue;
                
                if (bestSoFar is null)
                {
                    bestSoFar = tile.Position;
                } else if (tile.Position.Distance(Position) < bestSoFar.Distance(Position))
                {
                    bestSoFar = tile.Position;
                }
            }
        }

        return bestSoFar;
    }
}