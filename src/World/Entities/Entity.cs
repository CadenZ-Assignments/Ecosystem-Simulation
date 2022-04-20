using Simulation_CSharp.Core;
using Simulation_CSharp.Registry.Tiles;
using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.World.Entities;

public abstract class Entity
{
    // ReSharper disable once InconsistentNaming
    public readonly List<Gene> DNA;
    public readonly EntityInfo EntityInfo;
    public TileCell Position = null!;

    protected Entity(EntityInfo entityInfo)
    {
        DNA = new List<Gene>();
        EntityInfo = entityInfo;
    }

    public void Destroy()
    {
        SimulationCore.Level.RemovalQueue.Enqueue(this);
    }

    public abstract void Render();

    public virtual void Update()
    {
        if (Position.X is > Level.WorldWidth or < 0 || Position.Y is > Level.WorldHeight or < 0)
        {
            Destroy();
        }
    }

    /// <summary>
    /// Find the closest tile of type in Entity's Sensor Range
    /// </summary>
    /// <param name="tileType">The tile type looking for.</param>
    /// <returns>The position of the closest tile, returns null if not found.</returns>
    protected TileCell? FindTile(ITileType tileType)
    {
        for (var x = 0; x < EntityInfo.MaxSensorRange; x++)
        {
            for (var y = 0; y < EntityInfo.MaxSensorRange; y++)
            {
                var tile = SimulationCore.Level.Map.GetTileAtCell(new TileCell(Position.X + x, Position.Y + y));
                if (tile != null && tile.Type == tileType)
                {
                    return tile.Position;
                }
            }
        }

        return null;
    }
}