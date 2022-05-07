using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.World;

public interface IMap
{
    public void SetTileAtCell(TileType tileType, TileCell cell);

    public Tile? GetTileAtCell(TileCell cell);

    public void RemoveDecorationAtCell(TileCell cell);
    
    public void SetDecorationAtCell(TileType tileType, TileCell cell);
    
    public Tile? GetDecorationAtCell(TileCell cell);

    public bool ExistInRange(int x, int y);

    public void Render();

    public void GenerateNew();

    public Dictionary<TileCell, Tile> GetGrid();
}