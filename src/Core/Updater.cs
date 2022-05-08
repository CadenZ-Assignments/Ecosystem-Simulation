using System;
using System.Numerics;
using Raylib_cs;
using Simulation_CSharp.Entities;
using Simulation_CSharp.Entities.Sheep;
using Simulation_CSharp.Entities.Wolf;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;
using Simulation_CSharp.Utils.Widgets;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Core;

public static class Updater
{
    public static ILevel Level = null!;

    private static readonly GraphRenderer GraphRenderer = new GraphRenderer(-4000, 16);
    
    public static void Update(ref Camera2D camera)
    {
        Input(ref camera);
        Raylib.BeginMode2D(camera);
        Raylib.ClearBackground(new Color(40, 40, 40, 255));
        CameraModification(ref camera);
        RenderMap();
        GraphRenderer.Render();
        RenderEntities();
        Raylib.EndMode2D();
        RenderHud();
    }

    private static void RenderEntities()
    {
        Level.GetEntities().ForEach(entity =>
        {
            entity.Render();
            if (SimulationCore.Time != 0)
            {
                entity.Update();
            }
        });
        if (SimulationCore.Time != 0)
        {
            Level.CleanQueues();
        }
    }

    private static void RenderMap()
    {
        Level.GetMap().Render();
        if (SimulationCore.Time != 0)
        {
            Level.GetMap().Update();
        }
    }

    private static readonly ButtonManager ButtonManager = new("Spawn Sheep", "Spawn Wolf", "Spawn Bush", "Clear Entities");

    private static void RenderHud()
    {
        Raylib.DrawFPS(20, 20);
        ButtonManager.Render();
    }

    private static void RenderDebugGrid()
    {
        for (int i = 0; i < World.Level.WorldWidth; i++)
        {
            for (int j = 0; j < World.Level.WorldHeight; j++)
            {
                var pos = new TileCell(i, j);
                Raylib.DrawRectangleLines((int) pos.TruePosition.X, (int) pos.TruePosition.Y, (int) TileCell.CellSideLength, (int) TileCell.CellSideLength, Color.BLACK);
            }
        }
    }

    private const float MinZoomValue = 0.1f;
    private static float _scrollMovement = 1;

    private static Vector2 _initialMousePos;
    private static Vector2 _secondMousePos;

    private static void CameraModification(ref Camera2D camera)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            _initialMousePos = Helper.GetWorldSpaceMousePos(ref camera);
        }

        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
        {
            var tempSecondPos = Helper.GetWorldSpaceMousePos(ref camera);
            if (_secondMousePos != tempSecondPos)
            {
                camera.target += -(tempSecondPos - _initialMousePos);
            }

            _secondMousePos = tempSecondPos;
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
        {
            _initialMousePos = Vector2.Zero;
        }

        // Zooming the camera
        camera.zoom = Math.Clamp(_scrollMovement, MinZoomValue, float.MaxValue);
    }

    private static void Input(ref Camera2D camera)
    {
        // Middle click generates a new map. For debugging purposes 
        if (Raylib.IsKeyDown(KeyboardKey.KEY_F))
        {
            Level.GetMap().GenerateNew();
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_R))
        {
            camera.target = Vector2.Zero;
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && !ButtonManager.IsMouseOver(ref camera))
        {
            var mp = new TileCell(Helper.GetWorldSpaceMousePos(ref camera));
            if (Level.GetMap().ExistInRange(mp.X, mp.Y))
            {
                switch (ButtonManager.Selected)
                {
                    case 0:
                        SpawnAtMouse(ref camera, () => new Sheep());
                        break;
                    case 1:
                        SpawnAtMouse(ref camera, () => new Wolf());
                        break;
                    case 2:
                        PlaceAtMouse(ref camera, TileTypes.GrownBushTile, false);
                        break;
                    case 3:
                        foreach (var entity in Level.GetEntities())
                        {
                            Level.RemoveEntity(entity);
                        }
                        break;
                }
            }
        }

        var value = Raylib.GetMouseWheelMove();
        switch (value)
        {
            case 0:
                return;
            case < 0 when _scrollMovement >= MinZoomValue:
                _scrollMovement += value / 2;
                break;
            case < 0:
                return;
            default:
                _scrollMovement += value / 2;
                break;
        }
    }
    
    private static void SpawnAtMouse(ref Camera2D camera, Func<Entity> entity)
    {
        Level.CreateEntity(entity, new TileCell(Helper.GetWorldSpaceMousePos(ref camera)));
    }
        
    private static void PlaceAtMouse(ref Camera2D camera, TileType type, bool blocking)
    {
        if (type.IsDecoration)
        {
            Level.GetMap().SetDecorationAtCell(type, new TileCell(Helper.GetWorldSpaceMousePos(ref camera)), blocking);
        }
        else
        {
            Level.GetMap().SetTileAtCell(type, new TileCell(Helper.GetWorldSpaceMousePos(ref camera)));
        }
    }
}