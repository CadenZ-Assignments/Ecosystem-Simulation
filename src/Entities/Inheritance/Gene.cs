namespace Simulation_CSharp.Entities.Inheritance;

public class Gene
{
    public readonly int MaxHealth;
    public readonly int MaxSpeed;
    public readonly int MaxHunger;
    public readonly int MaxThirst;
    public readonly int MaxReproductiveUrge;
    public readonly int MaxSensorRange;
    public readonly int MaxConstitution;
    public readonly int MaxRandomness;

    public Gene(int maxHealth, int maxSpeed, int maxThirst, int maxHunger, int maxReproductiveUrge, int maxSensorRange, int maxConstitution, int maxRandomness)
    {
        MaxHealth = maxHealth;
        MaxSpeed = maxSpeed;
        MaxThirst = maxThirst;
        MaxHunger = maxHunger;
        MaxReproductiveUrge = maxReproductiveUrge;
        MaxSensorRange = maxSensorRange;
        MaxConstitution = maxConstitution;
        MaxRandomness = maxRandomness;
    }

    public virtual void InfluenceStats(Entity entity)
    {
        entity.Health = MaxHealth;
        entity.Hunger = MaxHunger;
        entity.Thirst = MaxThirst;
        entity.ReproductiveUrge = MaxReproductiveUrge;
    }
    
    public static Gene InheritFrom(Gene parent1, Gene parent2)
    {
        return new Gene(
            Average(parent1.MaxHealth, parent2.MaxHealth),
            Average(parent1.MaxSpeed, parent2.MaxSpeed),
            Average(parent1.MaxThirst, parent2.MaxThirst),
            Average(parent1.MaxHunger, parent2.MaxHunger),
            Average(parent1.MaxReproductiveUrge, parent2.MaxReproductiveUrge),
            Average(parent1.MaxSensorRange, parent2.MaxSensorRange),
            Average(parent1.MaxConstitution, parent2.MaxConstitution),
            Average(parent1.MaxRandomness, parent2.MaxRandomness)
        );
    }

    private static int Average(int a, int b)
    {
        return (a + b) / 2;
    }
}