using System.Security.Cryptography;
using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Entities.AI.Goals;

public class DrinkGoal : TileTypeGoal
{
    public DrinkGoal(int priority, Entity entity, Brain brain) : base(priority, true, false, entity, brain,  "Looking for water", TileTypes.WaterTile)
    {
    }

    public override void OnPicked()
    {
        base.OnPicked();
        StatusText = "Looking for water";
    }

    public override bool OnCompleted()
    {
        // if is not thirsty then we can complete
        if (Entity.Genetics.MaxThirst - Entity.Thirst < Raylib.GetRandomValue(1, 10)) return true;
        // if not we add thirst and stop goal from completing
        if (!Helper.Chance(30*SimulationCore.Time)) return false;
        StatusText = "Drinking";
        Entity.Thirst += 1*SimulationCore.Time;
        return false;
    }

    public override bool CanPick()
    {
        return Entity.IsBelowTolerance(Entity.Thirst) && Entity.FindTile(TileTypes.WaterTile) is not null;
    }

    public override bool ShouldResume()
    {
        return Entity.IsBelowTolerance(Entity.Thirst) && Entity.FindTile(TileTypes.WaterTile) is not null;
    }
}