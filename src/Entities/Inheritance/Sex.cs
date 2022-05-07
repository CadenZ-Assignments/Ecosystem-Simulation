namespace Simulation_CSharp.Entities.Inheritance;

public enum Sex
{
    Male,
    Female,
}

public static class SexHelper
{
    public static Sex Opposite(this Sex sex)
    {
        return sex == Sex.Male ? Sex.Female : Sex.Male;
    }
}