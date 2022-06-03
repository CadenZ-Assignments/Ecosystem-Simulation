using Newtonsoft.Json;

namespace Simulation_CSharp.Serialization.Data;

public class StorableDataConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var obj = serializer.Deserialize<dynamic>(reader);
        var storedData = StoredData.DeserializeObject(obj);
            
        var type = StoredData.GetNameOfType(objectType);
        var created = StoredDataTypes.Types[type!].Value();
        if (created is ISerializable storable)
        {
            storable.Load(storedData);
        }
            
        return created;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.GetInterfaces().Contains(typeof(ISerializable));
    }

    public override bool CanWrite => false;
}