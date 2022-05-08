using System;
using System.Collections.Generic;
using Simulation_CSharp.Entities;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.World;

public interface ILevel
{
    public IMap GetMap();

    public void CreateEntity(Func<Entity> entity, TileCell position);

    public void RemoveEntity(Entity entity);

    public List<Entity> GetEntities();

    public Entity? GetEntityByUuid(Guid guid);

    public void CleanQueues();
}