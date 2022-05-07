using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.AI.Goals;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities;

public class SheepEntity : Entity
{
    private static readonly Gene BaseSheepGene = new(20, 1, 100, 100, 100, 40, 1, 10, false, true, false);
    
    public SheepEntity(Gene genetics) : base(genetics, "Sheep")
    {
        
    }

    public SheepEntity() : this(BaseSheepGene)
    {
        
    }
    
    protected override Brain CreateBrain()
    {
        var brain = new Brain(this, new AStarPathFinder<Tile>());
        brain.RegisterGoal(new DrinkGoal(5, this, brain));
        brain.RegisterGoal(new EatBushGoal(5, this, brain));
        brain.RegisterGoal(new RandomWalkGoal(0, this, brain));
        return brain;
    }
}