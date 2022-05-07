using System.Numerics;
using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.Entities.AI;
using Simulation_CSharp.Entities.Inheritance;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.Utils;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Entities;

public abstract class Entity
{
    protected readonly Lazy<Brain> Brain;
    public readonly Gene Genetics;
    public TileCell Position = null!;
    public ILevel Level = null!;

    public int Health;
    public int Hunger;
    public int Thirst;
    public int ReproductiveUrge;

    public readonly string EntityName;
    protected virtual Lazy<string> TexturePath => new(() => "entities\\" + EntityName.FileSafeFormat() + ".png");
    
    protected bool IsSelected;
    
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
        var mousePos = Helper.GetWorldSpaceMousePos(ref SimulationCore.Camera2D);
        var mouseOver = Helper.IsMouseOverArea(mousePos, Position.TruePosition, texture.width, texture.height);
        
        Raylib.DrawTexture(texture, (int) Position.TruePosition.X, (int) Position.TruePosition.Y, GetRenderColor(mouseOver));
        RenderHoverTooltip(texture, mouseOver);
    }

    public virtual void Update()
    {
        Brain.Value.Update();
        
        if (Position.X is > World.Level.WorldWidth or < 0 || Position.Y is > World.Level.WorldHeight or < 0)
        {
            Destroy();
        }
        
        if (IsSelected)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DELETE))
            {
                Destroy();
            }
        }
    }

    public virtual bool IsBelowTolerance(int value)
    {
        return value < 35 / Genetics.MaxConstitution;
    }

    public void Destroy()
    {
        Level.RemoveEntity(this);
    }

    public void RefreshGoals()
    {
        Brain.Value.RefreshGoal();
    }

    public List<TileCell> FindPathTo(TileType type)
    {
        var tc = FindTile(type);
        var map = Level.GetMap();
        if (tc is null) return new List<TileCell>();
        Brain.Value.PathFinder.Init(map.GetTileAtCell(Position)!, map.GetTileAtCell(tc)!, map.GetGrid());
        return Brain.Value.PathFinder.FindPath();
    }

    /// <summary>
    /// Moves entity towards the target location give
    /// </summary>
    /// <param name="position"></param>
    /// <returns>Returns false if entity can not walk on target location.</returns>
    public bool MoveTowardsLocation(Vector2 position)
    {
        var targetPos = new TileCell(Vector2.Lerp(Position.TruePosition, position, 0.008F * Genetics.MaxSpeed));
        var targetTile = Level.GetMap().GetTileAtCell(targetPos);
        
        if (targetTile is not null && !targetTile.WalkableForEntity(this))
        {
            // if entity can not walk on target position we will stop moving
            return false;
        }

        Position = targetPos;
        
        if (Helper.Chance(2))
        { 
            Thirst--;
        } else if (Helper.Chance(2))
        {
            Hunger--;
        }

        return true;
    }

    protected Color GetRenderColor(bool mouseOver)
    {
        if (mouseOver && !IsSelected || IsSelected && !mouseOver)
        {
            return Color.DARKGRAY;
        }

        if (mouseOver && IsSelected)
        {
            return Color.BLACK;
        }

        return Color.WHITE;
    }

    protected void RenderHoverTooltip(Texture2D texture2D, bool mouseOver)
    {
        if (mouseOver)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                IsSelected = !IsSelected;
            }
        }
        
        if (!IsSelected && !mouseOver) return;

        var rectX = Position.TruePosition.X + 40;
        var rectY = Position.TruePosition.Y - 45;
        var rectWidth = 200;
        var rectHeight = 10;
        var contentYModifier = 5;
        
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
            
            Raylib.DrawText(value.ToString(), (int) barX + 8, (int) barY + 5, 3, Color.BLACK);

            rectHeight += barHeight + 5;
            contentYModifier += barHeight + 2;
        }
    }

    /// <summary>
    /// Find the closest tile of type in Entity's Sensor Range
    /// </summary>
    /// <param name="tileType">The tile type looking for.</param>
    /// <returns>The position of the closest tile, returns null if not found.</returns>
    protected TileCell? FindTile(TileType tileType)
    {
        var range = Genetics.MaxSensorRange / 2;
        TileCell? bestSoFar = null;

        for (var x = -range; x < range; x++)
        {
            for (var y = -range; y < range; y++)
            {
                if (!Level.GetMap().ExistInRange(Position.X + x, Position.Y + y)) continue;
                var pos = new TileCell(Position.X + x, Position.Y + y);
                
                var tile = tileType.IsDecoration ? Level.GetMap().GetDecorationAtCell(pos) : Level.GetMap().GetTileAtCell(pos);

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