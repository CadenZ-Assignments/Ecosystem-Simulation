using Newtonsoft.Json;

namespace Simulation_CSharp.Serialization.Data.Types;

public class ListStoredData<T> : TypeStoredData<List<T>>, IListStoredData
{
    public ListStoredData() : this(new List<T>())
    {
    }

    public ListStoredData(IEnumerable<T> value) : base(new List<T>(value), false)
    {
    }
        
    public void Add(T element)
    {
        Value.Add(element);
    }

    public void Remove(int index)
    {
        Value.RemoveAt(index);
    }

    public T this[int index] => Value[index];

    protected override void SerializeValue(JsonWriter writer)
    {
        writer.WriteStartArray();
        foreach (var v in Value)
        {
            writer.WriteRawValue(SerializeObject(v));
        }
        writer.WriteEnd();
    }

    public override void Deserialize(JsonSerializer serializer, dynamic value)
    {
        foreach (var element in value)
        {
            Add(DeserializeObject(element));
        }
    }
}

public interface IListStoredData
{
}