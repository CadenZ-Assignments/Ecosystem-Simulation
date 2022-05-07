using System.Security.Cryptography;
using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.AI.Goals;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.PathFinding;

namespace Simulation_CSharp.Entities.Wolf;

public class Wolf : Entity
{
    public Wolf(Gene genetics) : base(genetics, "Wolf")
    {
        
    }

    public Wolf() : this(new Gene(20, 2, 100, 100, 20, 1, 10, false, true, false))
    {
        
    }
    
    protected override Brain CreateBrain()
    {
        var brain = new Brain(this, new AStarPathFinder(this));
        brain.RegisterGoal(new DrinkGoal(5, this, brain));
        brain.RegisterGoal(new EatSheepGoal(5, this, brain));
        brain.RegisterGoal(new ReproduceGoal(6, this, brain));
        brain.RegisterGoal(new RandomWalkGoal(0, this, brain, 30));
        return brain;
    }

    public override void MakeBaby(Entity mate)
    {
        if (mate is not Wolf) return;
        for (var i = 0; i < RandomNumberGenerator.GetInt32(4, 7); i++)
        {
            Level.CreateEntity(() => BabyWolf.CreateBaby(this, (Wolf) mate), Position);
        }
    }
}