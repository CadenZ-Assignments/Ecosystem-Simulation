using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.AI.Goals;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.PathFinding;

namespace Simulation_CSharp.Entities.Wolf;

public class BabyWolf : BabyEntity
{
    public BabyWolf(Gene genetics) : base(genetics, "Baby Wolf")
    {
    }
    
    public static BabyWolf CreateBaby(Wolf parent1, Wolf parent2)
    {
        return new BabyWolf(Gene.InheritFrom(parent1.Genetics, parent2.Genetics));
    }
    
    protected override Brain CreateBrain()
    {
        var brain = new Brain(this, new AStarPathFinder(this));
        brain.RegisterGoal(new DrinkGoal(5, this, brain));
        brain.RegisterGoal(new EatSheepGoal(5, this, brain));
        brain.RegisterGoal(new RandomWalkGoal(0, this, brain, 10));
        return brain;
    }
}