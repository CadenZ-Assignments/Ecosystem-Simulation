using Raylib_cs;
using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.AI.Goals;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities;

public class SheepEntity : Entity
{
    public SheepEntity() : base(new EntityInfo(20, 100, 100, 100, 40))
    {
    }

    protected override Brain CreateBrain()
    {
        var brain = new Brain(this, new AStarPathFinder<Tile>());
        brain.RegisterGoal(new TileTypeGoal(0, this, brain, TileTypes.WaterTile));
        return brain;
    }

    public override void Render()
    {
        Raylib.DrawCircle((int) Position.TruePosition.X, (int) Position.TruePosition.Y, 12, Color.WHITE);
    }
}