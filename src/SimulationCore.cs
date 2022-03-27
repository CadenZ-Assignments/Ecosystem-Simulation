using System;
using System.Numerics;
using Raylib_cs;
using Simulation_CSharp.World;

namespace Simulation_CSharp
{
    public static class SimulationCore
    {
        public static readonly Level Level = new();

        public static void Main(string[] args)
        {
            try
            {
                SaveLoad.Load(args[0]);
            }
            catch (IndexOutOfRangeException)
            {
            }

            Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            Raylib.InitWindow(800, 480, "Simulation of Ecosystem");

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
}