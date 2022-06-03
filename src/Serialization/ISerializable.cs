using Simulation_CSharp.Serialization.Data.Types;

namespace Simulation_CSharp.Serialization;

/// <summary>
/// All ISerializable MUST have a default constructor with NO parameters. It will be used for reconstructing the object when being loaded from the save files. All data will be loaded through the Load method.
/// </summary>
public interface ISerializable
{
    public PairStoredData Save();

    public void Load(PairStoredData data);
}