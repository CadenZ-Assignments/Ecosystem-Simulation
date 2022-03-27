using System;
using System.Numerics;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.Entities
{
    public abstract class Entity
    {
        public readonly EntityInfo EntityInfo;
        public TileCell Position = null!;
        
        protected Entity(EntityInfo entityInfo)
        {
            EntityInfo = entityInfo;
        }

        /// <summary>
        /// Find the closest tile of type in Entity's Sensor Range
        /// </summary>
        /// <param name="tileType">The tile type looking for.</param>
        /// <returns>The position of the closest tile, returns null if not found.</returns>
        protected TileCell? Find(ITileType tileType)
        {
            for (var x = 0; x < EntityInfo.MaxSensorRange; x++)
            {
                for (var y = 0; y < EntityInfo.MaxSensorRange; y++)
                {
                    var tile = SimulationCore.Level.Map.GetTileAtCell(new TileCell(Position.X + x, Position.Y + y));
                    if (tile != null && tile.Type == tileType)
                    {
                        return tile.Position;
                    }
                }
            }

            return null;
        }
        
        public abstract void Render();

        public virtual void Update()
        {
            
        }
    }
    
    public class EntityInfo
    {
        public readonly ushort MaxHealth;
        public readonly ushort MaxThirst;
        public readonly ushort MaxHunger;
        public readonly ushort MaxReproductiveUrge;
        public readonly ushort MaxSensorRange;
        
        public ushort Health { get; set; }
        public ushort Thirst { get; set; }
        public ushort Hunger { get; set; }
        public ushort ReproductiveUrge { get; set; }

        public EntityInfo(ushort maxHealth, ushort maxThirst, ushort maxHunger, ushort maxReproductiveUrge, ushort maxSensorRange)
        {
            MaxHealth = maxHealth;
            MaxThirst = maxThirst;
            MaxHunger = maxHunger;
            MaxReproductiveUrge = maxReproductiveUrge;
            MaxSensorRange = maxSensorRange;

            Health = MaxHealth;
            Thirst = MaxThirst;
            Hunger = MaxHunger;
            ReproductiveUrge = MaxReproductiveUrge;
        }
    }
}