using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Simulation_CSharp.Serialization.Data;

public abstract class StoredData
{
    public abstract void Serialize(JsonWriter jsonWriter);

    public abstract void Deserialize(JsonSerializer serializer, dynamic value);

    public static string SerializeObject(object? value)
    {
        return JsonConvert.SerializeObject(value,
            new StoredDataConverter()
        );
    }

    public static object? DeserializeObject(dynamic value)
    {
        if (value is JValue jv)
        {
            return jv.Value;
        }
            
        return JsonConvert.DeserializeObject(value.ToString(), StoredDataTypes.Types[value.type.ToString()].Key, new StoredDataConverter());
    }

    public static string? GetNameOfType(Type? type)
    {
        var name = type?.Name;
        name = !(name is null) && name.Contains('`') ? name[..^2] : name;
        return name;
    }

    protected static void WriteField(JsonWriter writer, string name, object value)
    {
        writer.WritePropertyName(name);
        writer.WriteValue(value);
    }
}