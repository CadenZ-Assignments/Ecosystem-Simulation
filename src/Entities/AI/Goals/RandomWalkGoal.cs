using System.Numerics;
using System.Security.Cryptography;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class RandomWalkGoal : Goal
{
    private int _direction;
    private Vector2 _targetPos;

    public RandomWalkGoal(int priority, Entity entity, Brain brain) : base(priority, false, entity, brain)
    {
    }

    public override void OnPicked()
    {
        _direction = RandomNumberGenerator.GetInt32(1, 9);
        Vector2 dir;

        switch (_direction)
        {
            case 1:
                dir = new Vector2(1, 0);
                break;
            case 2:
                dir = new Vector2(0, 1);
                break;
            case 3:
                dir = new Vector2(-1, 0);
                break;
            case 4:
                dir = new Vector2(0, -1);
                break;
            case 5:
                dir = new Vector2(1, 1);
                break;
            case 6:
                dir = new Vector2(-1, 1);
                break;
            case 7:
                dir = new Vector2(1, -1);
                break;
            default:
                dir = new Vector2(-1, -1);
                break;
        }

        _targetPos = Entity.Position.TruePosition + dir * 30;
    }

    public override void PerformTask()
    {
        // moves entity towards the next step's position
        Entity.MoveTowardsLocation(_targetPos);
        
        // if we are close enough to this step then we move towards the next step
        if (Entity.Position.Distance(new TileCell(_targetPos)) < 2)
        {
            GoalCompleted();
        }
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