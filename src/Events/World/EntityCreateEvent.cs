using Simulation_CSharp.Entities;
using Simulation_CSharp.Tiles;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Events.World;

public class EntityCreateEvent : WorldEvent
{
    public readonly Entity Created;
    public readonly TileCell Position;

    public EntityCreateEvent(Level level, Entity created, TileCell position) : base(level)
    {
        Created = created;
        Position = position;
    }
}