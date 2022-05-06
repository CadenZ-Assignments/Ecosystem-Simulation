using System.Numerics;
using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.PathFinding;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Entities;

public abstract class Entity
{
    protected readonly Lazy<Brain> Brain;
    public readonly Gene Genetics;
    public TileCell Position = null!;

    public int Health;
    public int Hunger;
    public int Thirst;
    public int ReproductiveUrge;

    public readonly string EntityName;
    protected virtual Lazy<string> TexturePath => new(() => "entities\\" + EntityName.FileSafeFormat() + ".png");
    
    protected bool IsSelected = false;
    
    protected Entity(Gene genetics, string entityName)
    {
        Genetics = genetics;
        EntityName = entityName;
        Brain = new Lazy<Brain>(CreateBrain);
        Genetics.InfluenceStats(this);
    }

    protected abstract Brain CreateBrain();

    public virtual void Render()
    {
        var texture = ResourceLoader.GetTexture(TexturePath.Value);
        Raylib.DrawTexture(texture, (int) Position.TruePosition.X, (int) Position.TruePosition.Y,
            GetRenderColor(texture));
        RenderHoverTooltip(texture);
    }

    public virtual void Update()
    {
        Brain.Value.Update();
        if (Position.X is > Level.WorldWidth or < 0 || Position.Y is > Level.WorldHeight or < 0)
        {
            Destroy();
        }
    }

    public virtual bool IsBelowTolerance(int value)
    {
        return value < 30 / Genetics.MaxConstitution;
    }

    public void Destroy()
    {
        SimulationCore.Level.RemoveEntity(this);
    }

    public void RefreshGoals()
    {
        Brain.Value.RefreshGoal();
    }

    public List<TileCell> GoTo(ITileType type)
    {
        var tc = FindTile(type);
        var map = SimulationCore.Level.GetMap();
        return tc is not null ? Brain.Value.PathFinder.FindPath(map.GetTileAtCell(Position)!, map.GetTileAtCell(tc)!, map.GetGrid()) : new List<TileCell>();
    }

    public void MoveTowardsLocation(Vector2 position)
    {
        Position = new TileCell(Vector2.Lerp(Position.TruePosition, position, 0.005F * Genetics.MaxSpeed));
        if (Helper.Chance(2))
        { 
            Thirst--;
        } else if (Helper.Chance(2))
        {
            Hunger--;
        }
    }

    protected Color GetRenderColor(Texture2D texture2D)
    {
        var mousePos = Helper.GetWorldSpaceMousePos(ref SimulationCore.Camera2D);
        return Helper.IsMouseOverArea(mousePos, Position.TruePosition, texture2D.width, texture2D.height) || IsSelected
            ? Color.DARKGRAY
            : Color.WHITE;
    }

    protected void RenderHoverTooltip(Texture2D texture2D)
    {
        var mousePos = Helper.GetWorldSpaceMousePos(ref SimulationCore.Camera2D);
        // draw hover tool tip if mouse is hovering
        if (!IsSelected && !Helper.IsMouseOverArea(mousePos, Position.TruePosition, texture2D.width, texture2D.height)) return;

        var rectX = Position.TruePosition.X + 10;
        var rectY = Position.TruePosition.Y - 45;
        var rectWidth = 200;
        var rectHeight = 10;
        var contentYModifier = 5;

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            IsSelected = !IsSelected;
        }

        DrawText(EntityName);
        DrawText(Brain.Value.GetStatus() + "..");

        contentYModifier += 15;
        
        DrawProgressBar("Health", Genetics.MaxHealth, Health);
        DrawProgressBar("Hunger", Genetics.MaxHunger, Hunger);
        DrawProgressBar("Thirst", Genetics.MaxThirst, Thirst);
        DrawProgressBar("Reproductive Urge", Genetics.MaxReproductiveUrge, ReproductiveUrge);
        
        // background rect
        Raylib.DrawRectangleRounded(
            new Rectangle(
                rectX,
                rectY,
                rectWidth,
                rectHeight
            ),
            0.05f,
            20,
            new Color(20, 20, 20, 20)
        );

        void DrawText(string text)
        {
            Raylib.DrawText(text, (int) rectX + 5, (int) rectY + contentYModifier, 5, Color.WHITE);
            rectHeight += 10;
            contentYModifier += 10;
        }

        void DrawProgressBar(string label, float max, float value)
        {
            DrawText(label);
            
            var barWidth = rectWidth - 10;
            const int barHeight = 20;
            var barX = rectX + 5;
            var barY = rectY + contentYModifier;

            Raylib.DrawRectangleRounded(
                new Rectangle(
                    barX,
                    barY,
                    barWidth,
                    barHeight
                ),
                0.5f,
                4,
                new Color(200, 200, 200, 200)
            );
            
            Raylib.DrawRectangleRounded(
                new Rectangle(
                    barX + 2,
                    barY + 2,
                    (barWidth - 4) * (value / max),
                    barHeight - 4
                ),
                0.5f,
                4,
                new Color(100, 100, 100, 250)
            );
            
            Raylib.DrawText(value.ToString(), (int) barX + 8, (int) barY + 5, 3, Color.LIGHTGRAY);

            rectHeight += barHeight + 5;
            contentYModifier += barHeight + 2;
        }
    }

    /// <summary>
    /// Find the closest tile of type in Entity's Sensor Range
    /// </summary>
    /// <param name="tileType">The tile type looking for.</param>
    /// <returns>The position of the closest tile, returns null if not found.</returns>
    protected TileCell? FindTile(ITileType tileType)
    {
        var range = Genetics.MaxSensorRange / 2;
        TileCell? bestSoFar = null;

        for (var x = -range; x < range; x++)
        {
            for (var y = -range; y < range; y++)
            {
                if (!SimulationCore.Level.GetMap().ExistInRange(Position.X + x, Position.Y + y)) continue;
                var tile = SimulationCore.Level.GetMap().GetTileAtCell(new TileCell(Position.X + x, Position.Y + y));

                if (tile is null || tile.Type != tileType) continue;

                if (bestSoFar is null)
                {
                    bestSoFar = tile.Position;
                }
                else if (tile.Position.Distance(Position) < bestSoFar.Distance(Position))
                {
                    bestSoFar = tile.Position;
                }
            }
        }

        return bestSoFar;
    }
}