using System.Numerics;
using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Utils;

public static class Helper
{
    public static bool Chance(int chance)
    {
        return new Random().NextDouble() < chance / 100D;
    }
    
    public static bool Chance(double chance)
    {
        return new Random().NextDouble() < chance;
    }

    public static string FileSafeFormat(this string value)
    {
        return value.ToLower().Replace(" ", "_").Replace("-", "_").Replace(":", "_");
    }
    
    public static Vector2 GetWorldSpaceMousePos(ref Camera2D camera)
    {
        return Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
    }

    public static bool IsMousePosOverArea(Vector2 areaPos, int width, int height, ref Camera2D camera)
    {
        var pos = GetWorldSpaceMousePos(ref camera);
        return pos.X > areaPos.X && pos.X < areaPos.X + width && pos.Y > areaPos.Y && pos.Y < areaPos.Y + height;
    }
    
    public static bool IsMousePosOverAreaUI(Vector2 areaPos, int width, int height)
    {
        var pos = Raylib.GetMousePosition();
        return pos.X > areaPos.X && pos.X < areaPos.X + width && pos.Y > areaPos.Y && pos.Y < areaPos.Y + height;
    }

    public static bool IsPosInRange(TileCell pos, TileCell center, int range)
    {
        range += 1;
        return pos.X > center.X - range && pos.X < center.X + range && pos.Y > center.Y - range && pos.Y < center.Y + range;
    }

    public static Vector2 Vector(this Vector2 self, Vector2 point2)
    {
        return point2 - self;
    }

    public static float Magnitude(this Vector2 self)
    {
        return (float) Math.Sqrt(Math.Pow(self.X, 2) + Math.Pow(self.Y, 2));
    }

    public static Vector2 Normalized(this Vector2 self)
    {
        var mag = self.Magnitude();
        var x = self.X / mag;
        var y = self.Y / mag;
        return new Vector2(x, y);
    }
    
    public static Vector2 Opposite(this Vector2 self)
    {
        return new Vector2(-self.X, -self.Y);
    }

    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
    {
        var num1 = target.X - current.X;
        var num2 = target.Y - current.Y;
        var d = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
        if ((double) d == 0.0 || (double) maxDistanceDelta >= 0.0 && (double) d <= (double) maxDistanceDelta * (double) maxDistanceDelta)
            return target;
        var num3 = (float) Math.Sqrt((double) d);
        return new Vector2(current.X + num1 / num3 * maxDistanceDelta, current.Y + num2 / num3 * maxDistanceDelta);
    }
}