using Raylib_cs;
using Simulation_CSharp.Entities;
using Simulation_CSharp.Events;
using Simulation_CSharp.Events.World;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.World;

public class Level : ILevel
{
    private readonly int _worldWidth;
    private readonly int _worldHeight;
    
    private readonly List<Entity> _entities;
    private readonly Queue<(Func<Entity>, TileCell)> _additionQueue;
    private readonly Queue<Entity> _removalQueue;
    private readonly IMap _map;
        
    public Level(int worldWidth = 256, int worldHeight = 256, float generationFrequency = 0.01f)
    {
        _worldWidth = worldWidth;
        _worldHeight = worldHeight;
        _entities = new List<Entity>();
        _additionQueue = new Queue<(Func<Entity>, TileCell)>();
        _removalQueue = new Queue<Entity>();
        _map = new Map(worldWidth, worldHeight, this, generationFrequency);
    }

    public void CreateEntity(Func<Entity> entity, TileCell position)
    {
        Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Attempting to create an entity");
        _additionQueue.Enqueue((entity, position));
    }

    public void RemoveEntity(Entity entity)
    {
        _removalQueue.Enqueue(entity);
    }

    public List<Entity> GetEntities()
    {
        return _entities;
    }

    public Entity? GetEntityByUuid(Guid guid)
    {
        try
        {
            return _entities.Where((entity, _) => entity.Uuid.Equals(guid)).First();
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public void CleanQueues()
    {
        for (var i = 0; i < _additionQueue.Count; i++)
        {
            var item = _additionQueue.Dequeue();
            var entityCreated = item.Item1.Invoke();
            var eventInfo = new EntityCreateEvent(this, entityCreated, item.Item2);
            var eventRes = EventHook.OnPreEntityCreate(eventInfo);

            if (eventRes is not null && !(bool) eventRes)
            {
                Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Entity creation request canceled by event");
                return;
            }
            
            entityCreated.Position = item.Item2;
            entityCreated.Level = this;
            
            _entities.Add(entityCreated);
            EventHook.OnPostEntityCreate(eventInfo);
            
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Added Entity");
        }
        
        for (var i = 0; i < _removalQueue.Count; i++)
        {
            _entities.Remove(_removalQueue.Dequeue());
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Removed Entity");
        }
    }

    public int GetWorldWidth()
    {
        return _worldWidth;
    }

    public int GetWorldHeight()
    {
        return _worldHeight;
    }

    public IMap GetMap()
    {
        return _map;
    }
}