using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class ReproduceGoal : Goal
{
    private readonly Predicate<Entity> _match;
    private Entity? _mate;
    private int _step;
    
    public ReproduceGoal(int priority, Entity entity, Brain brain) : base(priority, false, false, entity, brain, "Looking for a mate")
    {
        _match = entity1 => entity1.SameSpecieAs(Entity) &&
                           entity1.Genetics.BiologicalSex == Entity.Genetics.BiologicalSex.Opposite() &&
                           entity1 != Entity && 
                           entity1.ReproductiveUrge > 50;
    }

    public override void OnPicked()
    {
        _mate = Entity.FindEntity(_match);
        StatusText = "Looking for a mate";
        _step = 0;
    }

    public override void PerformTask()
    {
        if (_mate is null)
        {
            GoalCompleted();
            return;
        }
        
        var path = Entity.FindPathTo(_mate);
        _step = path.IndexOf(Entity.ClosestTileCell(path));
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
            Entity.MakeBaby(_mate);
            Entity.ReproductiveUrge = 0;
            GoalCompleted();
        }
    }

    public override bool ShouldResume()
    {
        return Entity.ReproductiveUrge > 50 && Entity.FindEntity(_match) is not null;
    }

    public override bool CanPick()
    {
        return Entity.ReproductiveUrge > 50 && Entity.FindEntity(_match) is not null;
    }
}