using Raylib_cs;
using Simulation_CSharp.Core;
using Simulation_CSharp.Entities;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Tiles;

/// <summary>
/// A Type of Tiles. Defines how the tile renders, and behaves
/// </summary>
public class TileType
{
    public readonly bool IsDecoration;
    private readonly string _tileName;
    protected virtual Lazy<string> TexturePath => new(() => "tiles\\" + _tileName.FileSafeFormat() + ".png");

    public TileType(string tileName, bool isDecoration)
    {
        _tileName = tileName;
        IsDecoration = isDecoration;
    }
    
    public virtual void Render(TileCell position)
    {
        // drawing large amount of texture at once is wayyyy too laggy
        var texture = ResourceLoader.GetTexture(TexturePath.Value);
        Raylib.DrawTexture(texture, (int) position.TruePosition.X, (int) position.TruePosition.Y, Color.WHITE);
    }

    public virtual Tile CreateTile(TileCell position)
    {
        return new Tile(this, position);
    }

    public virtual bool WalkableForEntity(Entity entity)
    {
        return true;
    }
}