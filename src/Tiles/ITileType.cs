namespace Simulation_CSharp.Tiles;

/// <summary>
/// A Type of Tiles. Defines how the tile renders
/// </summary>
public interface ITileType
{
    public void Render(TileCell position)
    {
        
    }

    public Tile CreateTile(TileCell position)
    {
        return new Tile(this, position);
    }
}