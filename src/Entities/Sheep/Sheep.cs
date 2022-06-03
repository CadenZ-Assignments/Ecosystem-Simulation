using System.Security.Cryptography;
using Raylib_cs;
using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.AI.Goals;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.PathFinding;

namespace Simulation_CSharp.Entities.Sheep;

public class Sheep : Entity
{
    public Sheep(Gene genetics) : base(genetics, "Sheep")
    {
        
    }

    public Sheep() : this(new Gene(20, 1, 100, 100, 18, 1, 10, false, true, false, 1, 1))
    {
        
    }
    
    protected override Brain CreateBrain()
    {
        var brain = new Brain(this, new AStarPathFinder(this));
        brain.RegisterGoal(new DrinkGoal(5, this, brain));
        brain.RegisterGoal(new EatBushGoal(5, this, brain));
        brain.RegisterGoal(new ReproduceGoal(6, this, brain));
        brain.RegisterGoal(new RandomWalkGoal(0, this, brain, 15));
        brain.RegisterGoal(new EscapeFromPredatorGoal(10, this, brain));
        return brain;
    }

    public override void CreateOffspring(Entity mate)
    {
        if (mate is not Sheep) return;
        for (var i = 0; i < Raylib.GetRandomValue(1, 4); i++)
        {
            Level.CreateEntity(() => BabySheep.CreateBaby(this, (Sheep) mate), Position);
        }
    }
}