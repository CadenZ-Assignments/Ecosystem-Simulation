using System;
using System.Collections.Generic;
using Raylib_cs;
using Simulation_CSharp.Src.Entities;
using Simulation_CSharp.Src.Tiles;

namespace Simulation_CSharp.Src.World
{
    public class Level
    {
        public readonly List<Entity> Entities;
        public readonly Map Map;
        
        public Level()
        {
            Entities = new List<Entity>();
            Map = new Map();
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