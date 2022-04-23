using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.PathFinding;

public class Node
{
    public readonly TileCell Position;
    public float GCost { get; private set; } // distance from starting node
    public float HCost { get; private set; } // distance from ending node
    public float FCost { get; private set; } // G + H = F
    public bool Initialized { get; private set; }
    public Node? Parent = null;
    
    public Node(TileCell position)
    {
        Position = position;
    }

    public Node(Node copyFrom)
    {
        Position = copyFrom.Position;
        GCost = copyFrom.GCost;
        HCost = copyFrom.HCost;
        FCost = copyFrom.FCost;
        Initialized = copyFrom.Initialized;
    }
    
    public void InitializeCosts(float gCost, float hCost)
    {
        GCost = gCost;
        HCost = hCost;
        FCost = gCost + hCost;
        Initialized = true;
    }

    public void UnInitializeCosts()
    {
        GCost = 0;
        HCost = 0;
        FCost = 0;
        Initialized = false;
    }
    
    public static bool operator ==(Node? a, Node? b)
    {
        return a is not null && b is not null && a.Position == b.Position;
    }
    
    public static bool operator !=(Node? a, Node? b)
    {
        return a is null || b is null || a.Position != b.Position;
    }
    
    protected bool Equals(Node other)
    {
        return Position.Equals(other.Position);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Node) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position);
    }
}