using System.Numerics;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.World;

public interface IMap
{
    public void SetTileAtCell(TileType tileType, TileCell cell);

    public Tile? GetTileAtCell(TileCell cell);

    public void RemoveDecorationAtCell(TileCell cell);
    
    public void SetDecorationAtCell(TileType tileType, TileCell cell, bool blocking);
    
    public Tile? GetDecorationAtCell(TileCell cell);

    public bool ExistInRange(int x, int y);

    public void Render();
    
    public void Update();

    public void GenerateNew();

    public Dictionary<TileCell, Tile> GetGrid();
}