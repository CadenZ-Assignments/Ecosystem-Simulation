using Raylib_cs;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class TileTypeGoal : Goal
{
    private readonly TileType _tileType;
    private int _step;
    protected List<TileCell> Path = null!;

    public TileTypeGoal(int priority, bool canOverrideRandom, Entity entity, Brain brain, string statusText, TileType tileType) : base(priority, canOverrideRandom, entity, brain, statusText)
    {
        _tileType = tileType;
    }

    public override void OnPicked()
    { 
        Path = Entity.FindPathTo(_tileType);
        _step = 0;
    }

    public override void PerformTask()
    {
        if (!Path.Any())
        {
            GoalCompleted();
            return;
        }
        if (_step >= Path.Count)
        {
            GoalCompleted();
            return;
        }

        for (var i = 0; i < Path.Count; i++)
        {
            var st = Path[i];
            Raylib.DrawCircle((int) st.TruePosition.X, (int) st.TruePosition.Y, 5, Color.YELLOW);
            Raylib.DrawText(i.ToString(), (int) st.TruePosition.X, (int) st.TruePosition.Y, 2, Color.BLACK);
        }

        var stepPos = Path[_step];
        
        // moves entity towards the next step's position
        if (!Entity.MoveTowardsLocation(stepPos.TruePosition))
        {
            GoalCompleted();
            return;
        }
        
        // if we are close enough to this step then we move towards the next step
        if (Entity.Position.Distance(stepPos) <= 1.5)
        {
            _step++;
        }
    }
    
    public override bool ShouldResume()
    {
        var path = Entity.FindPathTo(_tileType);
        return path.Any() && Entity.Position.Distance(path.Last()) > 1;
    }

    public override bool CanPick()
    {
        return true;
    }
}