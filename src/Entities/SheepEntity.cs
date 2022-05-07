using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.AI.Goals;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.PathFinding;

namespace Simulation_CSharp.Entities;

public class SheepEntity : Entity
{
    public SheepEntity(Gene genetics) : base(genetics, "Sheep")
    {
        
    }

    public SheepEntity() : this(new Gene(20, 1, 100, 100, 30, 1, 10, false, true, false))
    {
        
    }
    
    protected override Brain CreateBrain()
    {
        var brain = new Brain(this, new AStarPathFinder(this));
        brain.RegisterGoal(new DrinkGoal(5, this, brain));
        brain.RegisterGoal(new EatBushGoal(5, this, brain));
        brain.RegisterGoal(new ReproduceGoal(6, this, brain));
        brain.RegisterGoal(new RandomWalkGoal(0, this, brain));
        return brain;
    }
}