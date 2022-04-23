using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Registry.Tiles;

namespace Simulation_CSharp.World.Tiles;

public class Tile : Node
{
    public readonly ITileType Type;

    public Tile(ITileType type, TileCell position) : base(position)
    {
        Type = type;
    }

    public void Render()
    {
        Type.Render(Position);
    }
}