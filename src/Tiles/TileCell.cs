using System.Numerics;

namespace Simulation_CSharp.Src.Tiles
{
    public class TileCell
    {
        public static readonly Vector2 CellSizeVec = new(10, 10);
        public const float CellSize = 10;

        private const float StartingX = 10;
        private const float StartingY = 10;

        public Vector2 TruePosition;
        public int X;
        public int Y;

        public TileCell(int x, int y)
        {
            X = x;
            Y = y;
            
            GenerateTruePosition();
        }

        public TileCell(Vector2 truePosition)
        {
            TruePosition = truePosition;

            GenerateXY();
        }
        
        public void GenerateXY()
        {
            GenerateXY(TruePosition);
        }

        private void GenerateXY(Vector2 vector2)
        {
            var x = vector2.X;
            x -= StartingX;
            x *= 2;
            x /= CellSize;

            X = (int) x;

            var y = vector2.Y;
            y -= StartingY;
            y *= 2;
            y /= CellSize;

            Y = (int) y;
        }

        public void GenerateTruePosition()
        {
            GenerateTruePosition(X, Y);
        }
        
        private void GenerateTruePosition(int x, int y)
        {
            TruePosition = new Vector2(StartingX + x * CellSize / 2, StartingY + y * CellSize / 2);
        }
    }
}