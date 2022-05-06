using System.Numerics;
using Raylib_cs;

namespace Simulation_CSharp.Utils;

public static class Helper
{
    public static bool Chance(int chance)
    {
        return new Random().NextDouble() < chance / 100D;
    }

    public static string FileSafeFormat(this string value)
    {
        return value.ToLower().Replace(" ", "_").Replace("-", "_").Replace(":", "_");
    }
    
    public static Vector2 GetWorldSpaceMousePos(ref Camera2D camera)
    {
        return Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
    }

    public static bool IsMouseOverArea(Vector2 pos, Vector2 areaPos, int width, int height)
    {
        return pos.X > areaPos.X && pos.X < areaPos.X + width && pos.Y > areaPos.Y && pos.Y < areaPos.Y + height;
    }
}