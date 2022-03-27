using Simulation_CSharp.Tiles;
using Simulation_CSharp.World;

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

        public void Destroy()
        {
            SimulationCore.Level.RemovalQueue.Enqueue(this);
        }
        
        public abstract void Render();

        public virtual void Update()
        {
            if (Position.X is > Level.WorldWidth or < 0 || Position.Y is > Level.WorldHeight or < 0)
            {
                Destroy();   
            }
        }
        
        /// <summary>
        /// Find the closest tile of type in Entity's Sensor Range
        /// </summary>
        /// <param name="tileType">The tile type looking for.</param>
        /// <returns>The position of the closest tile, returns null if not found.</returns>
        protected TileCell? FindTile(ITileType tileType)
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
    }
    
    public class EntityInfo
    {
        public readonly ushort MaxHealth;
        public readonly ushort MaxThirst;
        public readonly ushort MaxHunger;
        public readonly ushort MaxReproductiveUrge;
        public readonly ushort MaxSensorRange;

        private ushort _health;
        public ushort Health
        {
            get => _health;
            set
            {
                _health = value;
                OnHealthChanged?.Invoke(_health);
            }
        }

        private ushort _thirst;
        public ushort Thirst {
            get => _thirst;
            set
            {
                _thirst = value;
                OnThirstChanged?.Invoke(_thirst);        
            }
        }

        private ushort _hunger;
        public ushort Hunger
        {
            get => _hunger;
            set
            {
                _hunger = value;
                OnHungerChanged?.Invoke(_hunger);
            }
        }

        private ushort _reproductiveUrge;
        public ushort ReproductiveUrge
        {
            get => _reproductiveUrge;
            set
            {
                _reproductiveUrge = value;
                OnReproductiveUrgeChanged?.Invoke(_reproductiveUrge);
            }
        }

        public delegate void UShortChanged(ushort newValue);
        public event UShortChanged? OnHealthChanged;
        public event UShortChanged? OnThirstChanged;
        public event UShortChanged? OnHungerChanged;
        public event UShortChanged? OnReproductiveUrgeChanged;
        
        
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