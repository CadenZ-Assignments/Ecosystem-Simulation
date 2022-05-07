using Simulation_CSharp.Entities;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Tiles;

/// <summary>
/// Created instance is an instance of a tile in the world. Not to be confused with ITileType. There is only ever 1 single instance of each ITileType child. 
/// </summary>
public class Tile : Node
{
    public TileType Type;
    public ILevel Level = null!;

    public Tile(TileType type, TileCell position) : base(position)
    {
        Type = type;
    }

    public virtual bool Updatable()
    {
        return false;
    }
    
    public virtual void Update()
    {
    }

    public virtual void Render()
    {
        Type.Render(Position);
    }

    public bool WalkableForEntity(Entity entity)
    {
        return (!IsObstructed && Type.WalkableForEntity(entity)) || entity.Genetics.AirBorne;
    }
}