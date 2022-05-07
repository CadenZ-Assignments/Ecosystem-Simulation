using Raylib_cs;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class TileTypeGoal : Goal
{
    private readonly TileType _tileType;
    private int _step;
    protected List<TileCell> Path = null!;
    protected TileCell? TargetCell;
    
    public TileTypeGoal(int priority, bool canOverrideRandom, Entity entity, Brain brain, string statusText, TileType tileType) : base(priority, canOverrideRandom, entity, brain, statusText)
    {
        _tileType = tileType;
    }
    
    public override void OnPicked()
    { 
        // since tiles are static, we only need to evaluate the path once
        TargetCell = Entity.FindTile(_tileType);
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

        if (Entity.IsSelected)
        {
            for (var i = 0; i < Path.Count; i++)
            {
                var st = Path[i];

                try
                {
                    var st2 = Path[i + 1];
                    Raylib.DrawLine((int) st.TruePosition.X, (int) st.TruePosition.Y, (int) st2.TruePosition.X, (int) st2.TruePosition.Y, Color.WHITE);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
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