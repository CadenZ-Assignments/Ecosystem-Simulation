using Raylib_cs;
using Simulation_CSharp.Registry.Tiles;
using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.World.Entities
{
    public class SheepEntity : Entity
    {
        private List<TileCell> path;

        public SheepEntity() : base(new EntityInfo(20, 100, 100, 100, 40))
        {
        }

        public override void RefreshGoals()
        {
            path = GoTo(TileTypes.WaterTile);
        }

        public override void Render()
        {
            Raylib.DrawCircle((int) Position.TruePosition.X, (int) Position.TruePosition.Y, 12, Color.WHITE);
            foreach (var cell in path)
            {
                Raylib.DrawRectangleV(cell.TruePosition, TileCell.CellSizeVec/2, Color.RED);
            }
        }

        public override void Update()
        {
            base.Update();
        }
    }
}