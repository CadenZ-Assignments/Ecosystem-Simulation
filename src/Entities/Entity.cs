using Simulation_CSharp.Core;
using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Entities;

public abstract class Entity
{
    protected readonly Lazy<Brain> Brain;
    public readonly Lazy<EntityInfo> EntityInfo;
    public readonly Gene Genetics;
    public TileCell Position = null!;

    protected Entity(Gene genetics)
    {
        Genetics = genetics;
        EntityInfo = new Lazy<EntityInfo>(CreateEntityInfo);
        Brain = new Lazy<Brain>(CreateBrain);
    }
    
    public abstract void Render();

    protected abstract Brain CreateBrain();

    protected abstract EntityInfo CreateEntityInfo();
    
    public void Destroy()
    {
        SimulationCore.Level.RemoveEntity(this);
    }
    
    public void RefreshGoals()
    {
        Brain.Value.RefreshGoal();
    }

    public virtual void Update()
    {
        Brain.Value.Update();
        if (Position.X is > Level.WorldWidth or < 0 || Position.Y is > Level.WorldHeight or < 0)
        {
            Destroy();
        }
    }
    
    public List<TileCell> GoTo(ITileType type)
    {
        var tc = FindTile(type);
        var map = SimulationCore.Level.GetMap();
        
        if (tc is not null)
        {
            Brain.Value.PathFinder.Init(map.GetTileAtCell(Position)!, map.GetTileAtCell(tc)!, map.GetGrid());
            return Brain.Value.PathFinder.FindPath();
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
        var range = EntityInfo.Value.MaxSensorRange / 2;
        TileCell? bestSoFar = null;
        
        for (var x = -range; x < range; x++)
        {
            for (var y = -range; y < range; y++)
            {
                if (!SimulationCore.Level.GetMap().ExistInRange(Position.X + x, Position.Y + y)) continue;
                var tile = SimulationCore.Level.GetMap().GetTileAtCell(new TileCell(Position.X + x, Position.Y + y));
                
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