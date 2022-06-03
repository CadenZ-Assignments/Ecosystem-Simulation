using System.Numerics;
using Newtonsoft.Json;
using Simulation_CSharp.Utils;

namespace Simulation_CSharp.Serialization.Data.Types;

public class PairStoredData : TypeStoredData<Dictionary<string, object>>
{
    public PairStoredData() : base(new Dictionary<string, object>(), true)
    {
    }

    public void Store(string key, Guid value)
    {
        Value.Add(key, new GuidStoredData(value));
    }
        
    public void Store(string key, Vector2 value)
    {
        Value.Add(key, new Vector2StoredData(value));
    }

    public void Store(string key, ISerializable value)
    {
        Value.Add(key, new SerializableStoredData(value));
    }
        
        
    public void Store<T>(string key, T value)
    {
        switch (value)
        {
            case Guid guid:
                Store(key, guid);
                break;
            case Vector2 vector2:
                Store(key, vector2);
                break;
            case ISerializable serializable:
                Store(key, serializable);
                break;
            case IEnumerable<T> list:
                StoreArray(key, list);
                break;
            default:
                if (value is null) break;
                Value.Add(key, value);
                break;
        }
    }

    public void StoreArray<T>(string key, IEnumerable<T> value)
    {
        if (typeof(T).GetInterfaces().Contains(typeof(ISerializable)))
        {
            var list = new ListStoredData<StoredData>();
            foreach (ISerializable v in value)
            {
                list.Add(new SerializableStoredData(v));
            }

            Value.Add(key, list);
            return;
        }

        Value.Add(key, new ListStoredData<T>(value));
    }

    public Optional<T> Read<T>(string key)
    {
        if (Value.ContainsKey(key))
        {
            var value = Value[key];
            switch (value)
            {
                case T data:
                    return Optional<T>.Of(data);
                case TypeStoredData<T> storedData:
                {
                    return Optional<T>.Of(storedData.Value);
                }
            }
        }

        return Optional<T>.Empty();
    }
        
    public Optional<List<T>> ReadArray<T>(string key)
    {
        if (!Value.ContainsKey(key)) return Optional<List<T>>.Empty();

        var value = Value[key];

        if (value is ListStoredData<T> listStoredData)
        {
            return Optional<List<T>>.Of(listStoredData.Value);
        }

        if (!(value is IListStoredData iList)) return Optional<List<T>>.Empty();

        var list = new List<T>();

        foreach (var obj in ((ListStoredData<object>) iList).Value)
        {
            switch (obj)
            {
                case T t:
                    list.Add(t);
                    break;
                case SerializableStoredData storable:
                    if (storable.Value is T t2)
                    {
                        list.Add(t2);
                    }

                    break;
            }
        }

        return !list.Any() ? Optional<List<T>>.Empty() : Optional<List<T>>.Of(list);
    }

    public void SetIfPresent<T>(string key, Action<T> setter)
    {
        var data = Read<T>(key);
        if (data.HasValue)
        {
            setter(data.Value);
        }
    }

    public void SetArrayIfPresent<T>(string key, Action<List<T>> setter)
    {
        var data = ReadArray<T>(key);
        if (data.HasValue)
        {
            setter(data.Value);
        }
    }

    protected override void SerializeValue(JsonWriter writer)
    {
        foreach (var (key, value) in Value)
        {
            writer.WritePropertyName(key);
            writer.WriteRawValue(SerializeObject(value));
        }
    }

    public override void Deserialize(JsonSerializer serializer, dynamic value)
    {
        foreach (var kv in value)
        {
            Value.Add(kv.Name, DeserializeObject(kv.Value));
        }
    }
}