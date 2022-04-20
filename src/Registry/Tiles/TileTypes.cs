using Raylib_cs;
using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.Registry.Tiles;

public class TileTypes
{
    public static readonly Dictionary<RegistryName, ITileType> TileTypesRegistry;

    public static readonly ITileType GrassTile = new GrassTile();
    public static readonly ITileType WaterTile = new WaterTile();

    static TileTypes()
    {
        TileTypesRegistry = new Dictionary<RegistryName, ITileType>
        {
            {new RegistryName("vanilla", "grass"), GrassTile},
            {new RegistryName("vanilla", "water"), WaterTile}
        };
    }
}
    
public class GrassTile : ITileType
{
    public void Render(TileCell position)
    {
        Raylib.DrawRectangleV(position.TruePosition, TileCell.CellSizeVec, Color.GREEN);
    }
}

public class WaterTile : ITileType
{
    public void Render(TileCell position)
    {
        Raylib.DrawRectangleV(position.TruePosition, TileCell.CellSizeVec, Color.BLUE);
    }
}