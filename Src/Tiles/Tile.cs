namespace Simulation_CSharp.Src.Tiles
{
    public class Tile
    {
        public readonly ITileType Type;
        public readonly TileCell Position;

        public Tile(ITileType type, TileCell position)
        {
            Type = type;
            Position = position;
        }

        public void Render()
        {
            Type.Render(Position);
        }
    }
}