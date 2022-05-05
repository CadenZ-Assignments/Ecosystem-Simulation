using Simulation_CSharp.Events.World;

namespace Simulation_CSharp.Events;

public static class EventHook
{
    public static event Func<EntityCreateEvent, bool>? PreEntityCreate;
    public static event Action<EntityCreateEvent>? PostEntityCreate;
    
    public static bool? OnPreEntityCreate(EntityCreateEvent obj)
    {
        return PreEntityCreate?.Invoke(obj);
    }

    public static void OnPostEntityCreate(EntityCreateEvent obj)
    {
        PostEntityCreate?.Invoke(obj);
    }
}