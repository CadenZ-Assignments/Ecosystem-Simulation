using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Entities.Inheritance;

public sealed class Gene
{
    public const int MaxReproductiveUrge = 100; 
    
    public readonly int MaxHealth;
    public readonly int MaxSpeed;
    public readonly int MaxHunger;
    public readonly int MaxThirst;
    public readonly int MaxSensorRange;
    public readonly int MaxConstitution;
    public readonly int MaxRandomness;
    public readonly bool WaterBorne;
    public readonly bool LandBorne;
    public readonly bool AirBorne;
    public readonly Sex BiologicalSex;
    
    public Gene(int maxHealth, int maxSpeed, int maxThirst, int maxHunger, int maxSensorRange, int maxConstitution, int maxRandomness, bool waterBorne, bool landBorne, bool airBorne)
    {
        MaxHealth = maxHealth;
        MaxSpeed = maxSpeed;
        MaxThirst = maxThirst;
        MaxHunger = maxHunger;
        MaxSensorRange = maxSensorRange;
        MaxConstitution = maxConstitution;
        MaxRandomness = maxRandomness;
        WaterBorne = waterBorne;
        LandBorne = landBorne;
        AirBorne = airBorne;
        BiologicalSex = Helper.Chance(50) ? Sex.Male : Sex.Female;
    }

    private Gene(int maxHealth, int maxSpeed, int maxThirst, int maxHunger, int maxSensorRange, int maxConstitution, int maxRandomness, bool waterBorne, bool landBorne, bool airBorne, Sex biologicalSex)
    {
        MaxHealth = maxHealth;
        MaxSpeed = maxSpeed;
        MaxThirst = maxThirst;
        MaxHunger = maxHunger;
        MaxSensorRange = maxSensorRange;
        MaxConstitution = maxConstitution;
        MaxRandomness = maxRandomness;
        WaterBorne = waterBorne;
        LandBorne = landBorne;
        AirBorne = airBorne;
        BiologicalSex = biologicalSex;
    }

    public void InfluenceStats(Entity entity)
    {
        entity.Health = MaxHealth;
        entity.Hunger = MaxHunger;
        entity.Thirst = MaxThirst;
        entity.ReproductiveUrge = 0;
    }
    
    public static Gene InheritFrom(Gene parent1, Gene parent2)
    {
        return new Gene(
            Average(parent1.MaxHealth, parent2.MaxHealth),
            Average(parent1.MaxSpeed, parent2.MaxSpeed),
            Average(parent1.MaxThirst, parent2.MaxThirst),
            Average(parent1.MaxHunger, parent2.MaxHunger),
            Average(parent1.MaxSensorRange, parent2.MaxSensorRange),
            Average(parent1.MaxConstitution, parent2.MaxConstitution),
            Average(parent1.MaxRandomness, parent2.MaxRandomness),
            parent1.WaterBorne,
            parent1.LandBorne,
            parent1.AirBorne,
            Helper.Chance(50) ? Sex.Male : Sex.Female
        );
    }

    private static int Average(int a, int b)
    {
        return (a + b) / 2;
    }
}