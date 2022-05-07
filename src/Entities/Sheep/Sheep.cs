using System.Security.Cryptography;
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

    public Sheep() : this(new Gene(20, 1, 100, 100, 18, 1, 10, false, true, false))
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

    public override void MakeBaby(Entity mate)
    {
        if (mate is Sheep)
        {
            for (var i = 0; i < RandomNumberGenerator.GetInt32(1, 4); i++)
            {
                Level.CreateEntity(() => BabySheep.CreateBaby(this, (Sheep) mate), Position);
            }
        }
    }
}