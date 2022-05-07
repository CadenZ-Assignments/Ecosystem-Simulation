using Raylib_cs;
using Simulation_CSharp.Entities;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Tiles;

public static class TileTypes
{
    public static readonly TileType GrassTile = new GrassTile();
    public static readonly TileType WaterTile = new WaterTile();
    public static readonly TileType GrownBushTile = new("bush_grown", true);
    public static readonly TileType GrowingBushTile = new GrowingBushTileType();
}

public class GrowingBushTileType : TileType
{
    public GrowingBushTileType() : base("bush_growing", true)
    {
    }

    public override Tile CreateTile(TileCell position)
    {
        return new GrowingBushTile(position);
    }

    public class GrowingBushTile : Tile
    {
        private int _countDown = 2000;
        
        public GrowingBushTile(TileCell position) : base(TileTypes.GrowingBushTile, position)
        {
        }

        public override bool Updatable()
        {
            return true;
        }

        public override void Update()
        {
            _countDown--;
            if (_countDown <= 0)
            {
                Level.GetMap().SetDecorationAtCell(TileTypes.GrownBushTile, Position);
                _countDown = 2000;
            }
        }
    }
}

public class GrassTile : TileType
{
    public GrassTile() : base("grass", false)
    {
    }

    public override bool WalkableForEntity(Entity entity)
    {
        return entity.Genetics.LandBorne;
    }

    public override void Render(TileCell position)
    {
        Raylib.DrawRectangleV(position.TruePosition, TileCell.CellSizeVec, SimulationColors.GrassColor);
    }
}

public class WaterTile : TileType
{
    public WaterTile() : base("water", false)
    {
    }
    
    public override bool WalkableForEntity(Entity entity)
    {
        return entity.Genetics.WaterBorne;
    }

    public override void Render(TileCell position)
    {
        Raylib.DrawRectangleV(position.TruePosition, TileCell.CellSizeVec, Color.BLUE);
    }
}