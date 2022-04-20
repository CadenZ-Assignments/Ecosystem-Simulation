using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.Registry.Tiles;

public interface ITileType
{
    void Render(TileCell position);
}