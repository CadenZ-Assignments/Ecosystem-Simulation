using Simulation_CSharp.Entities;
using Simulation_CSharp.World;

namespace Simulation_CSharp.Events.World.Entities;

public class EntityClickedEvent : WorldEvent
{
    public Entity Entity;
    public bool Selected;
    
    public EntityClickedEvent(Level level, Entity entity, bool selected) : base(level)
    {
        Entity = entity;
        Selected = selected;
    }
}