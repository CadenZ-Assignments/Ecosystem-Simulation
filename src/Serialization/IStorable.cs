using Simulation_CSharp.Serialization.Data.Types;

namespace Simulation_CSharp.Serialization;

/// <summary>
/// Difference between ISerializable and IStorable is that classes/structs that implements IStorable is stored but will NOT automatically be reconstructed when loading from data. Therefore you should construct them yourself and load the data.
/// This exist because I can not get a working static method that constructs a struct for the Load system to reconstruct the object.
/// </summary>
public interface IStorable
{
    public PairStoredData Serialize();
        
    public void Deserialize(PairStoredData data);
}