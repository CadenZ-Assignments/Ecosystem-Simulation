using Newtonsoft.Json;

namespace Simulation_CSharp.Serialization.Data.Types;

public class SerializableStoredData : TypeStoredData<ISerializable?>
{
    public SerializableStoredData(ISerializable? storable) : base(storable, true)
    {
    }

    public SerializableStoredData() : this(null)
    {
    }

    protected override void SerializeValue(JsonWriter writer)
    {
        writer.WritePropertyName("serializableType");
        var name = Value?.GetType().Name;
        name = !(name is null) && name.Contains('`') ? name[..^2] : name;
        writer.WriteValue(name);
        writer.WritePropertyName("serializableValue");
        Value?.Save().Serialize(writer);
    }

    public override void Deserialize(JsonSerializer serializer, dynamic value)
    {
        Value = JsonConvert.DeserializeObject(
            value.serializableValue.ToString(),
            StoredDataTypes.Types[value.serializableType.ToString()].Key,
            new StorableDataConverter()
        );
    }
}