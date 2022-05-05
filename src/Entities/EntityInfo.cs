namespace Simulation_CSharp.Entities;

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