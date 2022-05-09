using Raylib_cs;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Entities.Inheritance;

public sealed class Gene
{
    public const int MaxReproductiveUrge = 100;

    public readonly int MaxHealth;
    public readonly float MaxSpeed;
    public readonly int MaxHunger;
    public readonly int MaxThirst;
    public readonly int MaxSensorRange;
    public readonly int MaxConstitution;
    public readonly int MaxRandomness;
    public readonly bool WaterBorne;
    public readonly bool LandBorne;
    public readonly bool AirBorne;
    public readonly Sex BiologicalSex;
    public readonly int ReproductiveUrgeModifier;
    public readonly int GrowthAcceleration;

    public Gene(int maxHealth, float maxSpeed, int maxThirst, int maxHunger, int maxSensorRange, int maxConstitution, int maxRandomness, bool waterBorne, bool landBorne, bool airBorne, int reproductiveUrgeModifier, int growthAcceleration)
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
        ReproductiveUrgeModifier = reproductiveUrgeModifier;
        GrowthAcceleration = growthAcceleration;
        BiologicalSex = Helper.Chance(50) ? Sex.Male : Sex.Female;
    }

    private Gene(int maxHealth, float maxSpeed, int maxThirst, int maxHunger, int maxSensorRange, int maxConstitution, int maxRandomness, bool waterBorne, bool landBorne, bool airBorne, Sex biologicalSex, int reproductiveUrgeModifier, int growthAcceleration)
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
        ReproductiveUrgeModifier = reproductiveUrgeModifier;
        GrowthAcceleration = growthAcceleration;
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
            InheritStats(parent1.MaxHealth, parent2.MaxHealth),
            InheritStats(parent1.MaxSpeed, parent2.MaxSpeed),
            InheritStats(parent1.MaxThirst, parent2.MaxThirst),
            InheritStats(parent1.MaxHunger, parent2.MaxHunger),
            InheritStats(parent1.MaxSensorRange, parent2.MaxSensorRange),
            InheritStats(parent1.MaxConstitution, parent2.MaxConstitution),
            InheritStats(parent1.MaxRandomness, parent2.MaxRandomness),
            parent1.WaterBorne,
            parent1.LandBorne,
            parent1.AirBorne,
            Helper.Chance(50) ? Sex.Male : Sex.Female,
            InheritStats(parent1.ReproductiveUrgeModifier, parent2.ReproductiveUrgeModifier),
            InheritStats(parent1.GrowthAcceleration, parent2.GrowthAcceleration)
        );
    }

    private static float InheritStats(float parent1Stat, float parent2Stat)
    {
        if (!Helper.Chance(15)) return Average(parent1Stat, parent2Stat);

        // 15% chance of mutation
        return Helper.Chance(50)
            ? Math.Clamp(Average(parent1Stat, parent2Stat) + Raylib.GetRandomValue(1, 4), 1, float.MaxValue)
            : Math.Clamp(Average(parent1Stat, parent2Stat) - Raylib.GetRandomValue(1, 4), 1, float.MaxValue);
    }

    private static int InheritStats(int parent1Stat, int parent2Stat)
    {
        if (!Helper.Chance(15)) return Average(parent1Stat, parent2Stat);

        // 15% chance of mutation
        return Helper.Chance(50)
            ? Math.Clamp(Average(parent1Stat, parent2Stat) + Raylib.GetRandomValue(1, 4), 1, int.MaxValue)
            : Math.Clamp(Average(parent1Stat, parent2Stat) - Raylib.GetRandomValue(1, 4), 1, int.MaxValue);
    }

    private static float Average(float a, float b)
    {
        return (a + b) / 2;
    }

    private static int Average(int a, int b)
    {
        return (a + b) / 2;
    }
}