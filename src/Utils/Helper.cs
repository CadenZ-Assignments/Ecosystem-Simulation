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

    public static bool IsMousePosOverArea(Vector2 areaPos, int width, int height)
    {
        var pos = GetWorldSpaceMousePos(ref SimulationCore.Camera2D);
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
}