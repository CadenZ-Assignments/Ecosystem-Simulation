using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.Entities;
using Simulation_CSharp.Utils;
using Simulation_CSharp.Utils.Widgets;

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
        private int _countDown = 0;
        
        public GrowingBushTile(TileCell position) : base(TileTypes.GrowingBushTile, position)
        {
        }

        public override void Render()
        {
            base.Render();
            
            var texture = ResourceLoader.GetTexture(Type.TexturePath.Value);
            var mouseOver = Helper.IsMousePosOverArea(Position.TruePosition, texture.width, texture.height, ref SimulationCore.Camera2D);

            if (!mouseOver) return;
            
            var tooltipRenderer = new TooltipRenderer(Position.TruePosition.X + 20, Position.TruePosition.Y - 10, 10, 10);
            tooltipRenderer.DrawText("Growing Bush");
            tooltipRenderer.DrawSpace(15);
            tooltipRenderer.DrawProgressBar("Growth progress", 2000, _countDown, true);
            tooltipRenderer.DrawBackground();
        }

        public override bool Updatable()
        {
            return true;
        }

        public override void Update()
        {
            _countDown+=1*SimulationCore.Time;
            if (_countDown >= 2000)
            {
                Level.GetMap().SetDecorationAtCell(TileTypes.GrownBushTile, Position, false);
                _countDown = 0;
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