using Raylib_cs;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.World.Entities;

namespace Simulation_CSharp.Entities
{
    public class SheepEntity : Entity
    {
        private List<TileCell> _path;

        public SheepEntity() : base(new EntityInfo(20, 100, 100, 100, 40), new AStarPathFinder<Tile>())
        {
            _path = new List<TileCell>();
        }

        public override void RefreshGoals()
        {
            _path = GoTo(TileTypes.WaterTile);
        }

        public override void Render()
        {
            Raylib.DrawCircle((int) Position.TruePosition.X, (int) Position.TruePosition.Y, 12, Color.WHITE);
            foreach (var cell in _path)
            {
                Raylib.DrawRectangleV(cell.TruePosition, TileCell.CellSizeVec / 2, Color.RED);
            }
        }

        public override void Update()
        {
            base.Update();
        }
    }
}