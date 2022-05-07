using System.Security.Cryptography;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Entities.AI.Goals;

public class EatBushGoal : TileTypeGoal
{
    public EatBushGoal(int priority, Entity entity, Brain brain) : base(priority, true, entity, brain,  "Looking for food", TileTypes.GrownBushTile)
    {
    }

    public override void OnPicked()
    {
        base.OnPicked();
        StatusText = "Looking for food";
    }

    public override bool OnCompleted()
    {
        // if is not hungry then we can complete
        if (Entity.Genetics.MaxHunger - Entity.Hunger < RandomNumberGenerator.GetInt32(1, 10))
        {
            Entity.Level.GetMap().SetDecorationAtCell(TileTypes.GrowingBushTile, Path.Last());
            return true;
        }
        // if not we add hunger and stop goal from completing
        if (!Helper.Chance(30)) return false;
        StatusText = "Eating berries";
        Entity.Hunger++;
        return false;
    }

    public override bool CanPick()
    {
        return Entity.IsBelowTolerance(Entity.Hunger);
    }

    public override bool ShouldResume()
    {
        return Entity.IsBelowTolerance(Entity.Hunger);
    }
}