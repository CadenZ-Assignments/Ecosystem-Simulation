using System.Numerics;
using Raylib_cs;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Core;

public class MainMenu
{
    private Camera2D _camera1;
    private Camera2D _camera2;
    private Camera2D _camera3;
    private readonly ILevel _miniatureLevel1;
    private readonly ILevel _miniatureLevel2;
    private readonly ILevel _miniatureLevel3;
    private Vector2 _pos1;
    private Vector2 _pos2;
    private Vector2 _pos3;

    public MainMenu()
    {
        _pos1 = new Vector2(400, 300);
        _pos2 = new Vector2(1300, 400);
        _pos3 = new Vector2(550, 800);
        
        _camera1 = new Camera2D(new Vector2(400, 300),  new Vector2(12*TileCell.CellSideLength, 12*TileCell.CellSideLength), 0, 0.5F);
        _camera2 = new Camera2D(new Vector2(1300, 400),  new Vector2(16*TileCell.CellSideLength, 16*TileCell.CellSideLength), 0, 0.5F);
        _camera3 = new Camera2D(new Vector2(550, 800),  new Vector2(8*TileCell.CellSideLength, 8*TileCell.CellSideLength), 0, 0.5F);
        _miniatureLevel1 = new Level(48, 48, 0.05f);
        _miniatureLevel2 = new Level(64, 64, 0.04f);
        _miniatureLevel3 = new Level(32, 32, 0.08f);
    }
    
    public void Render(out Scene scene)
    {
        var screenWidth = Raylib.GetScreenWidth();
        var screenHeight = Raylib.GetScreenHeight();
        var titleMeasure = Raylib.MeasureText("Ecosystem Simulation", screenWidth/20);
        
        _pos1 = new Vector2(screenWidth / 6F, screenHeight / 5.5F);
        _pos2 = new Vector2(screenWidth / 1.3F, screenHeight / 2.4F);
        _pos3 = new Vector2(screenWidth / 2.4F, screenHeight / 1.3F);

        Color color1;
        Color color2;
        Color color3;
        
        Raylib.ClearBackground(SimulationColors.BackgroundColor);

        _camera1.rotation+=0.01f;
        _camera1.zoom = screenWidth / 1920F / 2;
        _camera1.offset = _pos1;
        Raylib.BeginMode2D(_camera1);
        _miniatureLevel1.GetMap().Render();
        if (Helper.IsMousePosOverArea(_pos1 / 8, (int) (24 * TileCell.CellSideLength), (int) (24 * TileCell.CellSideLength), ref _camera1))
        {
            color1 = Color.WHITE;
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
            {
            }
        }
        else
        {
            color1 = Color.BLACK;
        }
        Raylib.DrawText("Settings", (int) (12 * TileCell.CellSideLength) - Raylib.MeasureText("Settings", 100) / 2, (int) (12 * TileCell.CellSideLength), 100, color1);
        Raylib.EndMode2D();
        
        _camera2.rotation-=0.005f;
        _camera2.zoom = screenWidth / 1920F / 2;
        _camera2.offset = _pos2;
        Raylib.BeginMode2D(_camera2);
        _miniatureLevel2.GetMap().Render();
        if (Helper.IsMousePosOverArea(_pos2/32, (int) (32 * TileCell.CellSideLength), (int) (32 * TileCell.CellSideLength), ref _camera2))
        {
            color2 = Color.WHITE;
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
            {
                scene = Scene.Game;
                return;
            }
        }
        else
        {
            color2 = Color.BLACK;
        }
        Raylib.DrawText("Play", (int) (12 * TileCell.CellSideLength) + 50, (int) (12 * TileCell.CellSideLength), 100, color2);
        Raylib.EndMode2D();
        
        _camera3.rotation+=0.04f;
        _camera3.zoom = screenWidth / 1920F / 2;
        _camera3.offset = _pos3;
        Raylib.BeginMode2D(_camera3);
        _miniatureLevel3.GetMap().Render();
        if (Helper.IsMousePosOverArea(_pos3/16, (int) (16 * TileCell.CellSideLength), (int) (16 * TileCell.CellSideLength), ref _camera3))
        {
            color3 = Color.WHITE;
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Raylib.CloseWindow();
            }
        }
        else
        {
            color3 = Color.BLACK;
        }
        Raylib.DrawText("Quit", (int) (12 * TileCell.CellSideLength) - Raylib.MeasureText("Quit", 100), (int) (12 * TileCell.CellSideLength) - 150, 100, color3);
        Raylib.EndMode2D();
        
        Raylib.DrawText("Ecosystem Simulation", screenWidth / 2 - titleMeasure / 2, 100, screenWidth/20, Color.WHITE);
        Raylib.DrawFPS(20, 20);

        scene = Scene.MainMenu;
    }
}