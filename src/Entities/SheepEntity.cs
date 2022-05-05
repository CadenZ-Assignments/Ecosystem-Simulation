using Raylib_cs;
using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.AI.Goals;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities;

public class SheepEntity : Entity
{
    public SheepEntity(Gene genetics) : base(genetics)
    {
        
    }

    public SheepEntity() : this(new Gene())
    {
        
    }
    
    protected override Brain CreateBrain()
    {
        var brain = new Brain(this, new AStarPathFinder<Tile>());
        brain.RegisterGoal(new TileTypeGoal(0, this, brain, TileTypes.WaterTile));
        return brain;
    }

    protected override EntityInfo CreateEntityInfo()
    {
        return new EntityInfo(
            20,
            100,
            100, 
            100,
            40
        );
    }

    public override void Render()
    {
        Raylib.DrawCircle((int) Position.TruePosition.X, (int) Position.TruePosition.Y, 12, Color.WHITE);
    }
}