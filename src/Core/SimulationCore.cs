using System.Numerics;
using Raylib_cs;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Core;

public static class SimulationCore
{
    public static ILevel Level = null!;

    public static void Main(string[] args)
    {
        Level = new Level();
        
        Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.InitWindow(800, 480, "Simulation Engine");

        var screenWidth = Raylib.GetScreenWidth();
        var screenHeight = Raylib.GetScreenHeight();
        var camera = new Camera2D(new Vector2(screenWidth / 2f, screenHeight / 2f), Vector2.Zero, 0, 1);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Updater.Update(ref camera);
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}