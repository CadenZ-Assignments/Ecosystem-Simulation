namespace Simulation_CSharp.Entities.AI.Goals;

public class FeedPupsGoal : Goal
{
    public FeedPupsGoal(int priority, Entity entity, Brain brain) : base(priority, true, false, entity, brain, "Looking for food for pup(s)")
    {
    }

    public override void PerformTask()
    {
    }

    public override bool ShouldResume()
    {
        return true;
    }

    public override bool CanPick()
    {
        return true;
    }
}