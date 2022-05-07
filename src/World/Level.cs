using Raylib_cs;
using Simulation_CSharp.Entities;
using Simulation_CSharp.Events;
using Simulation_CSharp.Events.World;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.World;

public class Level : ILevel
{
    public const int WorldWidth = 256;
    public const int WorldHeight = 256;
        
    private readonly List<Entity> _entities;
    private readonly Queue<Entity> _removalQueue;
    private readonly IMap _map;
        
    public Level()
    {
        _entities = new List<Entity>();
        _removalQueue = new Queue<Entity>();
        _map = new Map(WorldWidth, WorldHeight, this);
    }

    public void CreateEntity(Func<Entity> entity, TileCell position)
    {
        Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Attempting to create an entity");

        var entityCreated = entity.Invoke();
        var eventInfo = new EntityCreateEvent(this, entityCreated, position);
        var eventRes = EventHook.OnPreEntityCreate(eventInfo);

        if (eventRes is not null && !(bool) eventRes)
        {
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Entity creation request canceled by event");
            return;
        }
            
        entityCreated.Position = position;
        entityCreated.Level = this;
        entityCreated.RefreshGoals();
        _entities.Add(entityCreated);
        Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Added Entity");
        EventHook.OnPostEntityCreate(eventInfo);
    }

    public void RemoveEntity(Entity entity)
    {
        _removalQueue.Enqueue(entity);
    }

    public List<Entity> GetEntities()
    {
        return _entities;
    }

    public void CleanEntityRemovalQueue()
    {
        for (var i = 0; i < _removalQueue.Count; i++)
        {
            _entities.Remove(_removalQueue.Dequeue());
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Removed Entity");
        }
    }

    public IMap GetMap()
    {
        return _map;
    }
}