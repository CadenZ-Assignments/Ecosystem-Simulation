using Raylib_cs;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class TileTypeGoal : Goal
{
    private readonly ITileType _tileType;
    private int _step;
    private List<TileCell> _path = null!;

    public TileTypeGoal(int priority, bool canOverrideRandom, Entity entity, Brain brain, string statusText, ITileType tileType) : base(priority, canOverrideRandom, entity, brain, statusText)
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
        if (!_path.Any())
        {
            GoalCompleted();
            return;
        }
        if (_step >= _path.Count)
        {
            GoalCompleted();
            return;
        }

        for (var i = 0; i < _path.Count; i++)
        {
            var st = _path[i];
            Raylib.DrawCircle((int) st.TruePosition.X, (int) st.TruePosition.Y, 5, Color.YELLOW);
            Raylib.DrawText(i.ToString(), (int) st.TruePosition.X, (int) st.TruePosition.Y, 2, Color.BLACK);
        }

        var stepPos = _path[_step];
        
        // moves entity towards the next step's position
        Entity.MoveTowardsLocation(stepPos.TruePosition);
        // if we are close enough to this step then we move towards the next step
        if (!(Entity.Position.Distance(stepPos) < 0.5)) return;
        _step++;
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