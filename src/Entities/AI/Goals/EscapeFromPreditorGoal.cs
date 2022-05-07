using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Entities.AI.Goals;

public class EscapeFromPredatorGoal : Goal
{
    private readonly Predicate<Entity> _match;
    private Entity? _predator;
    
    public EscapeFromPredatorGoal(int priority, Entity entity, Brain brain) : base(priority, true, true, entity, brain, "Running away from a predator")
    {
        _match = entity1 => entity1 is Wolf.Wolf;
    }

    public override void PerformTask()
    {
        _predator = Entity.FindEntity(_match);
        
        if (_predator is null)
        {
            GoalCompleted();
            return;
        }

        var direction = Entity.Position.TruePosition.Vector(_predator.Position.TruePosition).Normalized().Opposite();
        Entity.MoveTowardsLocation(Entity.Position.TruePosition + direction);
    }

    public override bool ShouldResume()
    {
        return Entity.FindEntity(_match) is not null;
    }

    public override bool CanPick()
    {
        return Entity.FindEntity(_match) is not null;
    }
}