using System.Collections.Generic;
using Raylib_cs;

namespace Simulation_CSharp.Src.Tiles
{
    public class TileTypes
    {
        public static readonly Dictionary<string, ITileType> TileTypesRegistry;

        static TileTypes()
        {
            TileTypesRegistry = new Dictionary<string, ITileType>();
            
            TileTypesRegistry.Add("GRASS", GrassTile);
            TileTypesRegistry.Add("WATER", WaterTile);
        }

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
}