using System.Numerics;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class TileTypeGoal : Goal
{
    private readonly ITileType _tileType;
    private int _step;
    private List<TileCell> _path = null!;

    public TileTypeGoal(int priority, bool canOverrideRandom, Entity entity, Brain brain, ITileType tileType) : base(priority, canOverrideRandom, entity, brain)
    {
        _tileType = tileType;
    }

    public override void OnPicked()
    { 
        _path = Entity.GoTo(_tileType);
        _step = 0;
    }

    public override void PerformTask()
    {
        // moves entity towards the next step's position
        Entity.MoveTowardsLocation(_path[_step].TruePosition);
        // if we are close enough to this step then we move towards the next step
        if (!(Entity.Position.Distance(_path[_step]) < 0.5)) return;
        _step++;
        if (_step == _path.Count)
        {
            GoalCompleted();
        }
    }
    
    public override bool ShouldResume()
    {
        var path = Entity.GoTo(_tileType);
        return path.Any() && Entity.Position.Distance(path.Last()) > 1;
    }

    public override bool CanPick()
    {
        return true;
    }
}