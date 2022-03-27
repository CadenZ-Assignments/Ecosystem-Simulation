using System.Collections.Generic;
using System.Security.Cryptography;
using Simulation_CSharp.Src.Tiles;

namespace Simulation_CSharp.Src.World
{
    public class Map
    {
        private readonly Dictionary<TileCell, Tile> _tiles;
        private readonly List<Tile> _decorations;

        public Map()
        {
            _tiles = new Dictionary<TileCell, Tile>();
            _decorations = new List<Tile>();
            GenerateNew();
        }
        
        public void GenerateNew()
        {
            _tiles.Clear();
            
            var noiseGenerator = new FastNoiseLite(RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));
            noiseGenerator.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            
            for (var x = 0; x < 255; x++)
            {
                for (var y = 0; y < 255; y++)
                {
                    var noiseValue = noiseGenerator.GetNoise(x, y);
                    var tileCell = new TileCell(x, y);
                    switch (noiseValue)
                    {
                        case < 0:
                            _tiles.Add(tileCell, new Tile(TileTypes.WaterTile, tileCell));
                            break;
                        case >= 0:
                            _tiles.Add(tileCell, new Tile(TileTypes.GrassTile, tileCell));
                            break;
                    }
                }
            }
        }

        public void Render()
        {
            foreach (var tile in _tiles.Values)
            {
                tile.Render();
            }
        }

        public Tile? GetTileAtCell(TileCell cell)
        {
            try
            {
                return _tiles[cell];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}