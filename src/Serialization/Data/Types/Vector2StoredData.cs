using System.Numerics;
using Newtonsoft.Json;

namespace Simulation_CSharp.Serialization.Data.Types;

public class Vector2StoredData : TypeStoredData<Vector2>
{
    public Vector2StoredData(Vector2 value) : base(value, true)
    {
    }

    public Vector2StoredData() : this(Vector2.Zero)
    {
    }

    protected override void SerializeValue(JsonWriter writer)
    {
        WriteField(writer, "x", Value.X);
        WriteField(writer, "y", Value.Y);
    }

    public override void Deserialize(JsonSerializer serializer, dynamic value)
    {
        float x = value.x;
        float y = value.y;
        Value = new Vector2(x, y);
    }
}