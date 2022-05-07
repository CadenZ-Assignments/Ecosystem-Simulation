using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Entities.AI;

public class Brain
{
    public readonly IPathFindingAgent<Tile> PathFinder;
    protected readonly Entity Entity;
    protected readonly List<Goal> Goals;
    protected Goal? CurrentGoal;
    
    public Brain(Entity entity, IPathFindingAgent<Tile> pathFinder)
    {
        PathFinder = pathFinder;
        Entity = entity;
        Goals = new List<Goal>();
    }

    public virtual void Update()
    {
        if (CurrentGoal == null)
        {
            if (Helper.Chance(1*SimulationCore.Time))
            {
                RefreshGoal();
            }
        }
        CurrentGoal?.PerformTask();
    }
    
    public void GoalCompleted()
    {
        RefreshGoal();
    }

    public void RegisterGoal(Goal goal)
    {
        Goals.Add(goal);
    }

    public void RefreshGoal()
    {
        CurrentGoal = PickGoal();
        CurrentGoal?.OnPicked();
    }

    public string GetStatus()
    {
        return CurrentGoal == null ? "Idling" : CurrentGoal.StatusText;
    }
    
    protected virtual Goal? PickGoal()
    {
        Goal? picked = null;
        
        foreach (var goal in Goals)
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
            if (Helper.Chance(Entity.Genetics.MaxRandomness))
            {
                if (!goal.CanOverrideRandomness)
                {
                    Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Randomly skipped goal of " + goal.StatusText);
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
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Picked new goal of " + picked.StatusText);
        }
        
        return picked;
    }
}