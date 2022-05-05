using Simulation_CSharp.PathFinding;

namespace Simulation_CSharp.Tiles;

/// <summary>
/// Created instance is an instance of a tile in the world. Not to be confused with ITileType. There is only ever 1 single instance of each ITileType child. 
/// </summary>
public class Tile : Node
{
    public ITileType Type;

    public Tile(ITileType type, TileCell position) : base(position)
    {
        Type = type;
    }

    public void Render()
    {
        Type.Render(Position);
    }
}