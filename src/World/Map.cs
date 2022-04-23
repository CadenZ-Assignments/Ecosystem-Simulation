using System.Security.Cryptography;
using Simulation_CSharp.Registry.Tiles;
using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.World;

public class Map
{
    public readonly Dictionary<TileCell, Tile> Tiles;
    public readonly List<Tile> Decorations;

    public readonly int WorldWidth;
    public readonly int WorldHeight;

    public Map(int worldWidth, int worldHeight)
    {
        WorldWidth = worldWidth;
        WorldHeight = worldHeight;

        Tiles = new Dictionary<TileCell, Tile>();
        Decorations = new List<Tile>();

        GenerateNew();
    }

    public void GenerateNew()
    {
        Tiles.Clear();

        var noiseGenerator = new FastNoiseLite(RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));
        noiseGenerator.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        for (var x = 0; x < WorldWidth; x++)
        {
            for (var y = 0; y < WorldHeight; y++)
            {
                var noiseValue = noiseGenerator.GetNoise(x, y);
                var tileCell = new TileCell(x, y);
                switch (noiseValue)
                {
                    case < 0:
                        Tiles.Add(tileCell, new Tile(TileTypes.WaterTile, tileCell));
                        break;
                    case >= 0:
                        Tiles.Add(tileCell, new Tile(TileTypes.GrassTile, tileCell));
                        break;
                }
            }
        }
    }

    public void Render()
    {
        foreach (var tile in Tiles.Values)
        {
            tile.Render();
        }
    }

    public Tile? GetTileAtCell(TileCell cell)
    {
        try
        {
            return Tiles[cell];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }
    
    public bool ExistInRange(int x, int y)
    {
        return x > 0 && y > 0 && x < WorldWidth && y < WorldHeight;
    }
}