namespace Simulation_CSharp.Entities.AI;

public abstract class Goal
{
    public readonly int Priority;
    protected readonly Brain Brain;
    protected readonly Entity Entity;
    public bool Completed;
    
    protected Goal(int priority, Entity entity, Brain brain)
    {
        Priority = priority;
        Entity = entity;
        Brain = brain;
    }

    public abstract void PerformTask();

    // will automatically call resume goal if this returns true during the goal picking process.
    public abstract bool NeedToPerformTask();

    // makes this goal not available until Resume is called
    public void GoalCompleted()
    {
        Completed = true;
        Brain.GoalCompleted();
    }

    // makes this goal able to be selected again
    public void ResumeGoal()
    {
        Completed = false;
    }
}