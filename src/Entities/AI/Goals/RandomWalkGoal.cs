using System.Numerics;
using System.Security.Cryptography;
using Simulation_CSharp.Core;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities.AI.Goals;

public class RandomWalkGoal : Goal
{
    private readonly int _walkRange;
    private int _direction;
    private int _walkTime;
    private Vector2 _targetPos;

    public RandomWalkGoal(int priority, Entity entity, Brain brain, int walkRange) : base(priority, false, false, entity, brain, "Wondering")
    {
        _walkRange = walkRange;
    }

    public override void OnPicked()
    {
        _direction = RandomNumberGenerator.GetInt32(1, 9);
        _walkTime = RandomNumberGenerator.GetInt32(100, 600);
        
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

        _targetPos = Entity.Position.TruePosition + dir * 5 * _walkRange;
    }

    public override void PerformTask()
    {
        _walkTime -= 1*SimulationCore.Time;
        
        // moves entity towards the next step's position
        if (!Entity.MoveTowardsLocation(_targetPos) || _walkTime <= 0)
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