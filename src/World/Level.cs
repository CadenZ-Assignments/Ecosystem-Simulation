using System;
using System.Collections.Generic;
using Raylib_cs;
using Simulation_CSharp.World.Entities;
using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.World
{
    public class Level
    {
        public const int WorldWidth = 256;
        public const int WorldHeight = 256;
        
        public readonly List<Entity> Entities;
        public readonly Queue<Entity> RemovalQueue;
        public readonly Map Map;
        
        public Level()
        {
            Entities = new List<Entity>();
            RemovalQueue = new Queue<Entity>();
            Map = new Map(WorldWidth, WorldHeight);
        }

        public void CreateEntity(Func<Entity> entity, TileCell position)
        {
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Added Entity");

            var entityCreated = entity.Invoke();
            entityCreated.Position = position;
            Entities.Add(entityCreated);
        }
    }
}