using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Entities.AI;

public class Brain
{
    public readonly IPathFindingAgent<Tile> PathFinder;
    private readonly Entity _entity;
    private readonly List<Goal> _goals;
    private readonly List<Goal> _panicGoals;
    private Goal? _currentGoal;
    
    public Brain(Entity entity, IPathFindingAgent<Tile> pathFinder)
    {
        PathFinder = pathFinder;
        _entity = entity;
        _goals = new List<Goal>();
        _panicGoals = new List<Goal>();
    }

    public virtual void Update()
    {
        if (_currentGoal == null)
        {
            if (Helper.Chance(1*SimulationCore.Time))
            {
                RefreshGoal();
            }
        }

        foreach (var panicGoal in _panicGoals)
        {
            if (panicGoal.ShouldResume())
            {
                panicGoal.ResumeGoal();
            }

            if (panicGoal.Completed)
            {
                continue;
            }

            if (!panicGoal.CanPick())
            {
                continue;
            }
            
            SetGoal(panicGoal);
        }
        
        _currentGoal?.PerformTask();
    }
    
    public void GoalCompleted()
    {
        RefreshGoal();
    }

    public void RegisterGoal(Goal goal)
    {
        if (goal.Panic)
        {
            _panicGoals.Add(goal);
            return;
        }
        _goals.Add(goal);
    }

    public void FinishRegistering()
    {
        SortPriority();
    }

    public void RefreshGoal()
    {
        SetGoal(PickGoal());
    }

    public void SetGoal(Goal? goal)
    {
        _currentGoal = goal;
        _currentGoal?.OnPicked();
    }

    public string GetStatus()
    {
        return _currentGoal == null ? "Idling" : _currentGoal.StatusText;
    }
    
    private Goal? PickGoal()
    {
        Goal? picked = null;
        
        foreach (var goal in _goals)
        {
            if (goal.ShouldResume())
            {
                goal.ResumeGoal();
            }

            if (goal.Completed)
            {
                continue;
            }

            if (!goal.CanPick())
            {
                continue;
            }
            
            // have a chance (defined by genetics) to not pick the most important task at hand
            if (Helper.Chance(_entity.Genetics.MaxRandomness))
            {
                if (!goal.CanOverrideRandomness)
                {
                    Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, "Randomly skipped goal of " + goal.StatusText);
                    continue;
                }
            }
            
            if (picked == null)
            {
                picked = goal;
            }
            else if (picked.Priority < goal.Priority) 
            {
                picked = goal;
            }
        }

        if (picked != null)
        {
            Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, "Picked new goal of " + picked.StatusText);
        }
        
        return picked;
    }

    private void SortPriority()
    {
        _panicGoals.Sort((goal, goal1) =>
        {
            if (goal.Priority < goal1.Priority)
            {
                return -1;
            }
            return goal.Priority == goal1.Priority ? 0 : 1;
        });
    }
}