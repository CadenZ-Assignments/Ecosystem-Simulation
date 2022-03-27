using Raylib_cs;

namespace Simulation_CSharp.Src.Entities
{
    public class SheepEntity : Entity
    {
        public SheepEntity() : base(new EntityInfo(20, 100, 100, 100, 40))
        {
        }

        public override void Render()
        {
            Raylib.DrawCircle((int) Position.TruePosition.X, (int) Position.TruePosition.Y, 7, Color.WHITE);
        }

        public override void Update()
        {
            
        }
    }
}