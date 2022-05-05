using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;

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
        CurrentGoal ??= PickGoal();
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
    }
    
    protected virtual Goal? PickGoal()
    {
        Goal? picked = null;
        
        foreach (var goal in Goals)
        {
            if (goal.NeedToPerformTask())
            {
                goal.ResumeGoal();
            }

            if (goal.Completed)
            {
                continue;
            }
            
            if (picked == null)
            {
                picked = goal;
            } else if (picked.Priority < goal.Priority)
            {
                picked = goal;
            }
        }

        return picked;
    }
}