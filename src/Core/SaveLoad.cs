using System.Text.Json;

namespace Simulation_CSharp.Core;

// TODO
public class SaveLoad
{
    private const string Path = "Saves/";
    private const string FilePath = "Saves/Test.json";
        
    public static void Load(string file)
    {
            
    }

    public static void Save()
    {
        if (!File.Exists(Path)) Directory.CreateDirectory(Path);

        using var fileStream = new FileStream(FilePath, FileMode.Create);
        JsonSerializer.Serialize(fileStream, SimulationCore.Level);
    }
}