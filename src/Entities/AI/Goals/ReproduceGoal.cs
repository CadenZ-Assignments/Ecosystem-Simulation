using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class ReproduceGoal : Goal
{
    private Predicate<Entity> match;
    private Entity? _targetEntity;
    private int _step;
    
    public ReproduceGoal(int priority, Entity entity, Brain brain) : base(priority, false, entity, brain, "Looking for a mate")
    {
        match = entity1 => entity1.SameSpecieAs(Entity) &&
                           entity1.Genetics.BiologicalSex == Entity.Genetics.BiologicalSex.Opposite() &&
                           entity1 != Entity;
    }

    public override void OnPicked()
    {
        _targetEntity = Entity.FindEntity(match);
        StatusText = "Looking for a mate";
        _step = 0;
    }

    public override void PerformTask()
    {
        var path = Entity.FindPathTo(match);
        _step = path.IndexOf(ClosestTileCell(path));
        TileCell stepPos;
        
        try
        {
            stepPos = path[_step + 1];
        }
        catch (ArgumentOutOfRangeException)
        {
            stepPos = path[_step];
        }
        
        if (!Entity.MoveTowardsLocation(stepPos.TruePosition))
        {
            GoalCompleted();
            return;
        }

        if (Entity.Position.Distance(path.Last()) <= 0.5)
        {
            Entity.ReproductiveUrge = 0;
            GoalCompleted();
        }
    }

    public override bool ShouldResume()
    {
        return Entity.ReproductiveUrge > 60;
    }

    public override bool CanPick()
    {
        return Entity.ReproductiveUrge > 60 && Entity.FindEntity(match) is not null;
    }

    private TileCell ClosestTileCell(List<TileCell> cells)
    {
        TileCell? closest = null;

        foreach (var cell in cells)
        {
            if (closest is null || Entity.Position.Distance(cell) < Entity.Position.Distance(closest))
            {
                closest = cell;
            }
        }

        return closest!;
    }
}