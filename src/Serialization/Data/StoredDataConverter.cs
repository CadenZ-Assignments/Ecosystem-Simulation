using Newtonsoft.Json;

namespace Simulation_CSharp.Serialization.Data;

public class StoredDataConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is StoredData data)
        {
            data.Serialize(writer);
        }
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var obj = serializer.Deserialize<dynamic>(reader);
        var t = obj.type;
        var v = obj.value;

        var storedData = StoredDataTypes.Types[t.ToString()].Value();
        storedData.Deserialize(serializer, v);

        return storedData;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsSubclassOf(typeof(StoredData));
    }
}