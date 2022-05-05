namespace Simulation_CSharp.Entities.Inheritance;

public class Gene
{
    public readonly ushort MaxHealth;
    public readonly ushort MaxThirst;
    public readonly ushort MaxHunger;
    public readonly ushort MaxReproductiveUrge;
    public readonly ushort MaxSensorRange;

    public Gene(ushort maxHealth, ushort maxThirst, ushort maxHunger, ushort maxReproductiveUrge, ushort maxSensorRange)
    {
        MaxHealth = maxHealth;
        MaxThirst = maxThirst;
        MaxHunger = maxHunger;
        MaxReproductiveUrge = maxReproductiveUrge;
        MaxSensorRange = maxSensorRange;
    }

    public static Gene InheritFrom(Gene parent1, Gene parent2)
    {
        return new Gene(
            Average(parent1.MaxHealth, parent2.MaxHealth),
            Average(parent1.MaxThirst, parent2.MaxThirst),
            Average(parent1.MaxHunger, parent2.MaxHunger),
            Average(parent1.MaxReproductiveUrge, parent2.MaxReproductiveUrge),
            Average(parent1.MaxSensorRange, parent2.MaxSensorRange)
        );
    }

    private static ushort Average(ushort a, ushort b)
    {
        return (ushort) ((a + b) / 2);
    }
}