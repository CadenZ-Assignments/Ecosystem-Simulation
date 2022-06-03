using Newtonsoft.Json;

namespace Simulation_CSharp.Serialization.Data.Types;

public class GuidStoredData : TypeStoredData<Guid>
{
    public GuidStoredData(Guid value) : base(value, false)
    {
    }

    public GuidStoredData() : base(Guid.Empty, false)
    {
    }

    protected override void SerializeValue(JsonWriter writer)
    {
        writer.WriteValue(Value.ToString());
    }

    public override void Deserialize(JsonSerializer serializer, dynamic value)
    {
        Value = value;
    }
}