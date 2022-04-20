using System.Numerics;

namespace Simulation_CSharp.World.Tiles;

public class TileCell
{
    public const float CellSideLength = 20;
    
    public static readonly Vector2 CellSizeVec = new(CellSideLength, CellSideLength);
    private static readonly float SidePow = (float) Math.Pow(CellSideLength, 2);
    public static readonly float DiagonalDistancePerCell = (float) Math.Sqrt(SidePow + SidePow);

    private const float StartingX = 20;
    private const float StartingY = 20;

    public Vector2 TruePosition;
    public int X;
    public int Y;

    public TileCell(int x, int y)
    {
        X = x;
        Y = y;
            
        GenerateTruePosition();
    }

    public TileCell(Vector2 truePosition)
    {
        TruePosition = truePosition;

        GenerateXY();
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
    public void GenerateXY()
    {
        GenerateXY(TruePosition);
    }

    // ReSharper disable once InconsistentNaming
    private void GenerateXY(Vector2 vector2)
    {
        var x = vector2.X;
        x -= StartingX;
        x *= 2;
        x /= CellSideLength;

        X = (int) x;

        var y = vector2.Y;
        y -= StartingY;
        y *= 2;
        y /= CellSideLength;

        Y = (int) y;
    }

    public void GenerateTruePosition()
    {
        GenerateTruePosition(X, Y);
    }
        
    private void GenerateTruePosition(int x, int y)
    {
        TruePosition = new Vector2(StartingX + x * CellSideLength / 2, StartingY + y * CellSideLength / 2);
    }
    
    public static bool operator ==(TileCell a, TileCell b)
    {
        return a.X == b.X && a.Y == b.Y;
    }
    
    public static bool operator !=(TileCell a, TileCell b)
    {
        return a.X != b.X || a.Y != b.Y;
    }
    
    protected bool Equals(TileCell other)
    {
        return TruePosition.Equals(other.TruePosition) && X == other.X && Y == other.Y;
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
        return HashCode.Combine(TruePosition, X, Y);
    }
}