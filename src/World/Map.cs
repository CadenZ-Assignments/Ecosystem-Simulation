using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.World;

public class Map : IMap
{
    public readonly Dictionary<TileCell, Tile> Tiles;
    public readonly Dictionary<TileCell, Tile> Decorations;
    public readonly List<Tile> UpdatableTiles;

    public readonly int WorldWidth;
    public readonly int WorldHeight;
    private readonly ILevel _level;
    private readonly float _generationFrequency;

    public Map(int worldWidth, int worldHeight, ILevel level, float generationFrequency = 0.01f)
    {
        WorldWidth = worldWidth;
        WorldHeight = worldHeight;

        Tiles = new Dictionary<TileCell, Tile>();
        _level = level;
        Decorations = new Dictionary<TileCell, Tile>();
        UpdatableTiles = new List<Tile>();

        _generationFrequency = generationFrequency;

        GenerateNew();
    }

    public void SetTileAtCell(TileType tileType, TileCell cell)
    {
        if (!ExistInRange(cell.X, cell.Y)) return;
        Tiles.Remove(cell);
        AddTile(cell, tileType);
    }
    
    public Tile? GetTileAtCell(TileCell cell)
    {
        return ExistInRange(cell.X, cell.Y) ? Tiles[cell] : null;
    }

    public void SetDecorationAtCell(TileType tileType, TileCell cell, bool blocking)
    {
        if (!ExistInRange(cell.X, cell.Y)) return;
        Decorations.Remove(cell);
        AddDecoration(cell, tileType, blocking);
    }

    public void RemoveDecorationAtCell(TileCell cell)
    {
        if (!ExistInRange(cell.X, cell.Y)) return;
        if (Decorations.ContainsKey(cell))
        {
            Decorations.Remove(cell);
        }
    }

    public Tile? GetDecorationAtCell(TileCell cell)
    {
        return ExistInRange(cell.X, cell.Y) && Decorations.ContainsKey(cell) ? Decorations[cell] : null;
    }

    public bool ExistInRange(int x, int y)
    {
        return x > 0 && y > 0 && x < WorldWidth && y < WorldHeight;
    }

    public void Render()
    {
        foreach (var tile in Tiles.Values)
        {
            tile.Render();
        }

        foreach (var decoration in Decorations.Values)
        {
            decoration.Render();
        }
    }

    public void Update()
    {
        foreach (var updatableTile in UpdatableTiles)
        {
            updatableTile.Update();
        }
    }
    
    public void GenerateNew()
    {
        Tiles.Clear();
        Decorations.Clear();
        UpdatableTiles.Clear();

        // base layer
        var noiseGenerator = new FastNoiseLite(new Random().Next());
        noiseGenerator.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noiseGenerator.SetFrequency(_generationFrequency);

        for (var x = 0; x < WorldWidth; x++)
        {
            for (var y = 0; y < WorldHeight; y++)
            {
                var noiseValue = noiseGenerator.GetNoise(x, y);
                var tileCell = new TileCell(x, y);
                switch (noiseValue)
                {
                    case < 0:
                        AddTile(tileCell, TileTypes.WaterTile);
                        break;
                    case >= 0:
                        AddTile(tileCell, TileTypes.GrassTile);
                        if (Helper.Chance(1))
                        {
                            AddDecoration(tileCell, Helper.Chance(70) ? TileTypes.GrownBushTile : TileTypes.GrowingBushTile, false);
                        }
                        break;
                }
            }
        }
    }

    public Dictionary<TileCell, Tile> GetGrid()
    {
        return Tiles;
    }

    private void AddTile(TileCell cell, TileType tileType)
    {
        var tile = tileType.CreateTile(cell);
        tile.Level = _level;
        Tiles.Add(cell, tile);
        if (tile.Updatable())
        {
            UpdatableTiles.Add(tile);
        }
    }

    private void AddDecoration(TileCell cell, TileType type, bool blocking)
    {
        var tile = type.CreateTile(cell);
        tile.Level = _level;
        Decorations.Add(cell, tile);
        if (blocking)
        {
            Tiles[cell].IsObstructed = true;
        }

        if (tile.Updatable())
        {
            UpdatableTiles.Add(tile);
        }
    }
}