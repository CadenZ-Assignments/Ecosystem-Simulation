using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.World;

public interface IMap
{
    public void SetTileAtCell(ITileType tileType, TileCell cell);

    public Tile? GetTileAtCell(TileCell cell);

    public bool ExistInRange(int x, int y);

    public void Render();

    public void GenerateNew();

    public Dictionary<TileCell, Tile> GetGrid();
}