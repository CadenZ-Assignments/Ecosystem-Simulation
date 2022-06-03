namespace Simulation_CSharp.Utils;

public struct Optional<T>
{
    public readonly bool HasValue;
    public readonly T Value;

    private Optional(T value)
    {
        Value = value;
        HasValue = true;
    }

    public static Optional<T> Empty()
    {
        return new Optional<T>();
    }

    public static Optional<T> Of(T value)
    {
        return new Optional<T>(value);
    }
}