using System.Security.Cryptography;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class DrinkGoal : TileTypeGoal
{
    public DrinkGoal(int priority, Entity entity, Brain brain) : base(priority, true, entity, brain, TileTypes.WaterTile)
    {
    }

    public override bool OnCompleted()
    {
        // if is not thirsty then we can complete
        if (Entity.Genetics.MaxThirst - Entity.Thirst < RandomNumberGenerator.GetInt32(1, 10)) return true;
        
        // if not we add thirst and stop goal from completing
        Entity.Thirst++;
        return false;
    }

    public override bool CanPick()
    {
        return Entity.IsBelowTolerance(Entity.Thirst);
    }

    public override bool ShouldResume()
    {
        return base.ShouldResume() && Entity.IsBelowTolerance(Entity.Thirst);
    }
}