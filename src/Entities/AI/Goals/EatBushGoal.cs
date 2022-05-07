using System.Security.Cryptography;
using Simulation_CSharp.Core;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Entities.AI.Goals;

public class EatBushGoal : TileTypeGoal
{
    private int _startingHunger;
    
    public EatBushGoal(int priority, Entity entity, Brain brain) : base(priority, true, false, entity, brain,  "Looking for food", TileTypes.GrownBushTile)
    {
    }

    public override void OnPicked()
    {
        base.OnPicked();
        _startingHunger = Entity.Hunger;
        StatusText = "Looking for food";
    }

    public override bool OnCompleted()
    {
        // if is not hungry then we can complete
        if (Entity.Hunger - _startingHunger > 20)
        {
            if (TargetCell is not null)
            {
                Entity.Level.GetMap().SetDecorationAtCell(TileTypes.GrowingBushTile, TargetCell, false);
            }
            return true;
        }
        // if not we add hunger and stop goal from completing
        if (!Helper.Chance(30*SimulationCore.Time)) return false;
        StatusText = "Eating berries";
        Entity.Hunger += 1*SimulationCore.Time;
        return false;
    }

    public override bool CanPick()
    {
        return Entity.IsBelowTolerance(Entity.Hunger) && Entity.FindTile(TileTypes.GrownBushTile) is not null;
    }

    public override bool ShouldResume()
    {
        return Entity.IsBelowTolerance(Entity.Hunger) && Entity.FindTile(TileTypes.GrownBushTile) is not null;
    }
}