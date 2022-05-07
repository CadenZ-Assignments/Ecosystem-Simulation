using System.Numerics;

namespace Simulation_CSharp.Tiles;

public class TileCell
{
    public const float CellSideLength = 32;
    public static readonly Vector2 CellSizeVec = new(CellSideLength, CellSideLength);

    private const float StartingX = 20;
    private const float StartingY = 20;

    public readonly Vector2 TruePosition;
    public readonly int X;
    public readonly int Y;

    public TileCell(int x, int y)
    {
        X = x;
        Y = y;
        TruePosition = GenerateTruePosition();
    }

    public TileCell(Vector2 truePosition)
    {
        TruePosition = truePosition;
        var xy = GenerateXY();
        X = xy.Item1;
        Y = xy.Item2;
    }

    public float Distance(TileCell other)
    {
        return DistanceBetween(this, other);
    }

    public static float DistanceBetween(TileCell a, TileCell b)
    {
        // d = sqrt((x2-x1)^2 + (y2-y1)^2)
        return (float) Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
    }

    // ReSharper disable once InconsistentNaming
    public (int, int) GenerateXY()
    {
        return GenerateXY(TruePosition);
    }

    // ReSharper disable once InconsistentNaming
    public static (int, int) GenerateXY(Vector2 vector2)
    {
        var x = vector2.X;
        x -= StartingX;
        x *= 2;
        x /= CellSideLength;

        var y = vector2.Y;
        y -= StartingY;
        y *= 2;
        y /= CellSideLength;

        return ((int) x, (int) y);
    }

    public Vector2 GenerateTruePosition()
    {
        return GenerateTruePosition(X, Y);
    }
    
    public static Vector2 GenerateTruePosition(int x, int y)
    {
        return new Vector2(StartingX + x * CellSideLength / 2, StartingY + y * CellSideLength / 2);
    }
    
    public static bool operator ==(TileCell? a, TileCell? b)
    {
        return a is not null && b is not null && a.X == b.X && a.Y == b.Y;
    }
    
    public static bool operator !=(TileCell? a, TileCell? b)
    {
        return a is null || b is null || a.X != b.X || a.Y != b.Y;
    }
    
    protected bool Equals(TileCell other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TileCell) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}