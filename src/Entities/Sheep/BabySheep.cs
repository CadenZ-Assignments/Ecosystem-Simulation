using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.AI.Goals;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.PathFinding;

namespace Simulation_CSharp.Entities.Sheep;

public class BabySheep : BabyEntity
{
    private BabySheep(Gene genetics) : base(genetics, "Baby Sheep")
    {
    }

    public static BabySheep CreateBaby(Sheep parent1, Sheep parent2)
    {
        return new BabySheep(Gene.InheritFrom(parent1.Genetics, parent2.Genetics));
    }

    protected override Brain CreateBrain()
    {
        var brain = new Brain(this, new AStarPathFinder(this));
        brain.RegisterGoal(new DrinkGoal(5, this, brain));
        brain.RegisterGoal(new EatBushGoal(5, this, brain));
        brain.RegisterGoal(new RandomWalkGoal(0, this, brain, 5));
        brain.RegisterGoal(new EscapeFromPredatorGoal(10, this, brain));
        return brain;
    }
}