using Newtonsoft.Json;

namespace Simulation_CSharp.Serialization.Data.Types;

public abstract class TypeStoredData : StoredData
{
}
    
public abstract class TypeStoredData<T> : TypeStoredData
{
    public T Value { get; protected set; }
    private readonly bool _wrapObject;
    private readonly string? _type;

    protected TypeStoredData(T value, bool wrapObject)
    {
        Value = value;
        _wrapObject = wrapObject;
        _type = GetNameOfType(GetType());
    }

    protected abstract void SerializeValue(JsonWriter writer);
        
    public override void Serialize(JsonWriter jsonWriter)
    {
        jsonWriter.WriteStartObject();
        jsonWriter.WritePropertyName("type");
        jsonWriter.WriteValue(_type);
        jsonWriter.WritePropertyName("value");
        if (_wrapObject)
        {
            jsonWriter.WriteStartObject();
            SerializeValue(jsonWriter);
            jsonWriter.WriteEndObject();
        }
        else
        {
            SerializeValue(jsonWriter);
        }
        jsonWriter.WriteEnd();
    }
}