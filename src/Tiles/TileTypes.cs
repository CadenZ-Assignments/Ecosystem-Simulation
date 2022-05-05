using Raylib_cs;

namespace Simulation_CSharp.Tiles;

public static class TileTypes
{
    public static readonly ITileType GrassTile = new GrassTile();
    public static readonly ITileType WaterTile = new WaterTile();
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