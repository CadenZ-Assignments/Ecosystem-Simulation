using Simulation_CSharp.World;

namespace Simulation_CSharp.Events.World;

public class WorldEvent
{
    public readonly Level Level;

    public WorldEvent(Level level)
    {
        Level = level;
    }
}