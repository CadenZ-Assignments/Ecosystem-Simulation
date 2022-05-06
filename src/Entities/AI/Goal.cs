﻿namespace Simulation_CSharp.Entities.AI;

public abstract class Goal
{
    public readonly int Priority;
    public readonly bool CanOverrideRandomness;
    protected readonly Brain Brain;
    protected readonly Entity Entity;
    public bool Completed;
    
    protected Goal(int priority, bool canOverrideRandomness, Entity entity, Brain brain)
    {
        Priority = priority;
        CanOverrideRandomness = canOverrideRandomness;
        Entity = entity;
        Brain = brain;
    }

    /// <summary>
    /// A call back for when this goal gets picked, can be used to initialize variables for each iteration of this goal. See TileTypeGoal as an example
    /// </summary>
    public virtual void OnPicked()
    {
    }
    
    /// <summary>
    /// A call back when GoalCompleted is called. Normally not used, used mostly for inheriting from existing goals
    /// </summary>
    /// <returns>If returns false will stop the process of GoalComplete</returns>
    public virtual bool OnCompleted()
    {
        return true;
    }

    /// <summary>
    /// Called every update used to perform the task this goal is entitled with.
    /// </summary>
    public abstract void PerformTask();

    /// <summary>
    /// This is called everytime the brain tries to pick a new goal
    /// Determines whether this goal should be able to be selected again. Will call ResumeGoal if return is true.
    /// </summary>
    /// <returns>If the goal should be available again</returns>
    public abstract bool ShouldResume();

    /// <summary>
    /// Determines if this goal can be picked
    /// </summary>
    /// <returns></returns>
    public abstract bool CanPick();
    
    /// <summary>
    /// States that this goal is complete. Therefore will be removed from the available goals to select from. To make the goal available again override NeedToPerformTask (preferred) or call ResumeGoal 
    /// </summary>
    public void GoalCompleted()
    {
        if (!OnCompleted()) return;
        Completed = true;
        Brain.GoalCompleted();
    }

    /// <summary>
    /// Makes this goal available from selection again
    /// </summary>
    public void ResumeGoal()
    {
        Completed = false;
    }
}