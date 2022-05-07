using Simulation_CSharp.Entities.Sheep;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class EatSheepGoal : Goal
{
    private readonly Predicate<Entity> _match;
    private Entity? _prey;
    private int _step;

    public EatSheepGoal(int priority, Entity entity, Brain brain) : base(priority, true, false, entity, brain, "Hunting for prey")
    {
        _match = entity1 => entity1 is Sheep.Sheep or BabySheep;
    }
    
    public override void OnPicked()
    {
        base.OnPicked();
        StatusText = "Hunting for prey";
        _prey = Entity.FindEntity(_match);
        _step = 0;
    }

    public override void PerformTask()
    {
        if (_prey is null)
        {
            GoalCompleted();
            return;
        }
        
        var path = Entity.FindPathTo(_prey);
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

        if (Entity.Position.Distance(path.Last()) <= 1.5)
        {
            Entity.Hunger += _prey is BabySheep ? 20 : 35;
            _prey.Destroy();
            GoalCompleted();
        }
    }

    public override bool CanPick()
    {
        return Entity.IsBelowTolerance(Entity.Hunger) && Entity.FindEntity(_match) is not null;
    }

    public override bool ShouldResume()
    {
        return Entity.IsBelowTolerance(Entity.Hunger) && Entity.FindEntity(_match) is not null;
    }
}