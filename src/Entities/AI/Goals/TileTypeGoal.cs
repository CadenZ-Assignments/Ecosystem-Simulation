using System.Numerics;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class TileTypeGoal : Goal
{
    private readonly ITileType _tileType;
    
    public TileTypeGoal(int priority, Entity entity, Brain brain, ITileType tileType) : base(priority, entity, brain)
    {
        _tileType = tileType;
    }
    
    public override void PerformTask()
    {
        var path = Entity.GoTo(_tileType);
        Entity.Position = new TileCell(Vector2.Lerp(Entity.Position.TruePosition, path[0].TruePosition, 0.005F));
        if (Entity.Position.Distance(path[0]) < 0.5)
        {
            GoalCompleted();
        }
    }

    public override bool NeedToPerformTask()
    {
        var path = Entity.GoTo(_tileType);
        return Entity.Position.Distance(path[0]) > 1;
    }
}