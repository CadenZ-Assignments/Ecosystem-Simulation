namespace Simulation_CSharp.Utils;

public static class Helper
{
    public static bool Chance(int chance)
    {
        return new Random().NextDouble() < chance / 100D;
    }
}